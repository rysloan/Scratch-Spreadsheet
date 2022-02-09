using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using SpreadsheetUtilities;

/// <author> Ryan Sloan </author>
namespace SS
{
    /// <summary>
    /// Represents a Spreadsheet by setting the contents of cells to a Formula, String, or Double
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        
        // Dependency Graph that keeps track of all cells dependents and dependees
        private DependencyGraph _DG;

        // Dictionary that associates a cells name to a nested cell class that contains this cells contents and value
        private Dictionary<string, Cell> _Cells;

        // Property that lets the user know if the spreadsheet has been changed or not since it was created
        public override bool Changed { get; protected set; }

        /// <summary>
        /// Spreadsheets default constuctor, will make an empty spreadsheetof version 'default' with no normalizer or validator
        /// </summary>
        public Spreadsheet() : this(x => true, x => x, "default")
        {
        }

        /// <summary>
        /// Creates an empty Spreadsheet with the normalizer, validator, and version being chosen by the user
        /// </summary>
        /// <param name="isValid"> A function that checks if a variable is valid according to the user </param>
        /// <param name="normalize"> A function that normalizes the variable to the users liking </param>
        /// <param name="version"> This is the version of the spreadsheet being made </param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            _Cells = new Dictionary<string, Cell>();
            _DG = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// Creates a spreadsheet representation of the xml file that the user provides
        /// </summary>
        /// <param name="filePath"> The file path to the xml file </param>
        /// <param name="isValid"> A function that checks if a variable is valid according to the user </param>
        /// <param name="normalize"> A function that normalizes the variable to the users liking </param>
        /// <param name="version"> This is the version of the spreadsheet being made </param>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            _Cells = new Dictionary<string, Cell>();
            _DG = new DependencyGraph();
            if (GetSavedVersion(filePath) != version)
                throw new SpreadsheetReadWriteException("The version the you specified doesn't match the version of the xml file");
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                using (XmlReader reader = XmlReader.Create(filePath, settings))
                {
                    string cellName = null;
                    string cellContent = null ;
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case ("name"):
                                    reader.Read();
                                    cellName = reader.Value;
                                    break;
                                case ("contents"):
                                    reader.Read();
                                    cellContent = reader.Value;
                                    SetContentsOfCell(cellName, cellContent);
                                    cellName = null;
                                    cellContent = null;
                                    break;
                            }
                        }
                    }
                    if (!(cellName is null) && cellContent is null)
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Error while reading the file");
            }
            Changed = false;
        }

        /// <summary>
        /// Gets the Contents of the Cell represented by name
        /// </summary>
        /// <param name="name"> The cell name of whose contents you want </param>
        /// <returns> returns a double, string, or formula </returns>
        public override object GetCellContents(string name)
        {
            name = Normalize(name);
            if (name is null || !isVariable(name))
            {
                throw new InvalidNameException();
            }
            if (!IsValid(name))
            {
                throw new InvalidNameException();
            }
            if (!_Cells.ContainsKey(name))
            {
                return "";
            }
            else
            {
                return _Cells[name]._Contents;
            }
        }

        /// <summary>
        /// Gets the names of all nonempty cells
        /// </summary>
        /// <returns> returns an IEnumerable of the name of all non empty cells </returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (string cell in _Cells.Keys)
            {
                yield return cell;
            }
        }

        /// <summary>
        /// Sets the Contents to a double of the Cell represented by name
        /// </summary>
        /// <param name="name"> Name of the Cell </param>
        /// <param name="number"> What you want to set the cells contents too </param>
        /// <returns> returns a list of all cells who depend of the cell being set </returns>
        protected override IList<string> SetCellContents(string name, double number)
        {

            IList<string> cellsToRecalc = new List<string>();

            _DG.ReplaceDependees(name, new List<string>());

            Cell c = new Cell(number, lookup);
            _Cells[name] = c;

            foreach (string cell in GetCellsToRecalculate(name))
            {
                _Cells[cell]._Value = _Cells[cell]._Contents;
                cellsToRecalc.Add(cell);
            }
            Changed = true;
            return cellsToRecalc;
        }

        /// <summary>
        /// Sets the Contents to a string of the Cell represented by name
        /// </summary>
        /// <param name="name"> Name of the Cell </param>
        /// <param name="text"> What you want to set the cells contents too </param>
        /// <returns> returns a list of all cells who depend of the cell being set </returns>
        protected override IList<string> SetCellContents(string name, string text)
        {

            IList<string> cellsToRecalc = new List<string>();

            if (text == "")
            {
                return new List<string>(GetCellsToRecalculate(name));
            }

            _DG.ReplaceDependees(name, new List<string>());

            Cell c = new Cell(text, lookup);

            _Cells[name] = c;

            Changed = true;

            foreach (string cell in GetCellsToRecalculate(name))
            {
                _Cells[cell]._Value = _Cells[cell]._Contents;
                cellsToRecalc.Add(cell);
            }
            return cellsToRecalc;
        }

        /// <summary>
        /// Sets the Contents to a formula of the Cell represented by name
        /// </summary>
        /// <param name="name"> Name of the Cell </param>
        /// <param name="text"> What you want to set the cells contents too </param>
        /// <returns> returns a list of all cells who depend of the cell being set </returns>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            List<string> oldDependees = new List<string>(_DG.GetDependees(name));
            IList<string> cellsToRecalc = new List<string>();

            // Creates a copy if the change creates a circular dependency
            Cell copy = null;
            bool empty = false;
            if (_Cells.ContainsKey(name))
                copy = _Cells[name];
            else
                empty = true;


            _Cells[name] = new Cell(formula, lookup);

            _DG.ReplaceDependees(name, formula.GetVariables());

            try
            {
                foreach (string cell in GetCellsToRecalculate(name))
                {
                    _Cells[cell]._Value = _Cells[cell]._Contents;
                    cellsToRecalc.Add(cell);
                }
                Changed = true;
                return cellsToRecalc;
            }
            catch (Exception e)
            {
                // Resets the spreadsheet if the change causes a circular dependency
                if (empty == true)
                    _Cells.Remove(name);
                else
                    _Cells[name] = copy;

                _DG.ReplaceDependees(name, oldDependees);
                throw e;
            }
        }

        /// <summary>
        /// Gets the direct dependents of the cell
        /// </summary>
        /// <param name="name"> Name of the Cell </param>
        /// <returns> return a list of all dependent of cell name </returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            foreach (string depend in _DG.GetDependents(name))
            {
                yield return depend;
            }
        }

        /// <summary>
        /// Check if token is a valid variable by the terms of a spreadsheet
        /// </summary>
        private static bool isVariable(String token)
        {
            String pattern = @"^[A-Z|a-z]+[0-9]+$";
            Regex variable = new Regex(pattern);
            return variable.IsMatch(token);
        }

        /// <summary>
        /// Gets the version of the xml representation of a spreadsheet
        /// </summary>
        /// <param name="filename"> The xml file that we are getting the version of </param>
        /// <returns> The version of the file </returns>
        public override string GetSavedVersion(string filename)
        {
            string _version = null;
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                using (XmlReader reader = XmlReader.Create(filename, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "spreadsheet")
                            {
                                _version = reader.GetAttribute("version");
                            }
                        }
                    }
                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Error accessing this file");
            }
            if (_version is null)
            {
                throw new SpreadsheetReadWriteException("This spreadsheet has an invalid version");
            }
            return _version;
        }

        /// <summary>
        /// Saves the spreadsheet to a xml file following the path of filename
        /// </summary>
        /// <param name="filename"> The directory of the file </param>
        public override void Save(string filename)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "   ";

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);
                    foreach (string cell in GetNamesOfAllNonemptyCells())
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", cell);
                        if (_Cells[cell]._Contents is Formula)
                            writer.WriteElementString("contents", "=" + _Cells[cell]._Contents.ToString());
                        else
                            writer.WriteElementString("contents", _Cells[cell]._Contents.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                Changed = false;
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Error saving this spreadsheet");
            }
        }

        /// <summary>
        /// Get the value of the cell represented by name
        /// </summary>
        /// <param name="name"> the cells name </param>
        /// <returns> returns a string, double, or FormulaError </returns>
        public override object GetCellValue(string name)
        {
            name = Normalize(name);
            if (name is null || !isVariable(name))
            {
                throw new InvalidNameException();
            }
            if (!IsValid(name))
                throw new InvalidNameException();
            if (!_Cells.ContainsKey(name))
            {
                return "";
            }
            else
            {
                return _Cells[name]._Value;
            }
        }

        /// <summary>
        /// The driver method for all other setCellContents methods
        /// </summary>
        /// <param name="name"> the name of the cell </param>
        /// <param name="content"> the cells contents </param>
        /// <returns> returns a list of all cells who depend on this cell </returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = Normalize(name);

            if (content is null)
                throw new ArgumentNullException();
            if (name is null || !isVariable(name))
                throw new InvalidNameException();

            if (!IsValid(name))
                throw new InvalidNameException();

            if (Double.TryParse(content, out double x))
            {
                return SetCellContents(name, x);
            }

            else if (content.Length > 0 && content[0] == '=')
            {
                try
                {
                    Formula f = new Formula(content.Substring(1), Normalize, IsValid);
                    return SetCellContents(name, f);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            else
            {
                return SetCellContents(name, content);
            }

        }

        /// <summary>
        /// Lookups the value of the given cell name
        /// </summary>
        /// <param name="var"> The cell we are looking up </param>
        /// <returns> returns a double or throws an exception </returns>
        private double lookup(string var)
        {
            if (_Cells.ContainsKey(var))
            {
                if (GetCellValue(var) is double)
                    return (double)GetCellValue(var);
                else
                    throw new Exception();
            }
            else
                throw new Exception();
        }
    }


    /// <summary>
    /// Represents a Cell
    /// </summary>
    public class Cell
    {
        // Backing cell contents object
        private object content;

        // Backing cell value object
        private object value;

        // Represents the lookup method to find a cells value
        private Func<string, double> lookup;

        // Property representing a cells content
        public object _Contents
        {
            get { return content; }
            set { content = value; }
        }

        // Property representing a cells value
        public object _Value
        {
            get { return value; }
            set
            {
                if (value is Formula)
                {
                    this.value = new Formula(value.ToString()).Evaluate(lookup);
                }
                else
                    this.value = value;
            }
        }

        /// <summary>
        /// Cronstructor for a Cell with double contents
        /// </summary>
        /// <param name="contents"></param>
        public Cell (Double contents, Func<string, double> look_up)
        {
            content = contents;
            value = contents;
            lookup = look_up;
        }

        /// <summary>
        /// Cronstructor for a Cell with String contents
        /// </summary>
        /// <param name="contents"></param>
        public Cell (String contents, Func<string, double> look_up)
        {
            content = contents;
            value = contents;
            lookup = look_up;
        }

        /// <summary>
        /// Cronstructor for a Cell with Formula contents
        /// </summary>
        /// <param name="contents"></param>
        public Cell (Formula contents, Func<string, double> look_up)
        {
            content = contents;
            value = contents.Evaluate(look_up);
            lookup = look_up;
        }

    }
}
