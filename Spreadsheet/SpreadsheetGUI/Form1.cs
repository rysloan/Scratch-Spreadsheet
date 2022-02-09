using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

/// <summary>
/// <author> Ryan Sloan </author>
/// </summary>
namespace SpreadsheetGUI
{
    public partial class SpreadsheetGUI : Form
    {

        // Spreadsheet object that is associated to this SpreadsheetGUI
        private Spreadsheet _Sheet;


        public SpreadsheetGUI()
        {
            InitializeComponent();

            // sets the spreadsheet
            _Sheet = new Spreadsheet(x => Regex.IsMatch(x,@"^[A-Z][1-9][0-9]?$"), x => x.ToUpper(), "ps6");

            // sets the SpreadsheetPanel's SelectionChanged event to my changeSelectedCell method
            SpreadsheetCells.SelectionChanged += changeSelectedCell;

            // starts the user at "A1"
            SpreadsheetCells.SetSelection(0, 0);
            TextCellName.Text = "A1";
        }


        /// <summary>
        /// When the user clicks on a new cell in the 'SpreadsheetPanel' then the text boxes represented by Name, Content, and Value
        /// will all be changed based on the cell that was clicked
        /// </summary>
        /// <param name="sender"></param>
        public void changeSelectedCell (object sender)
        {
            int col;
            int row;
            SpreadsheetCells.GetSelection(out col, out row);

            // ALL DATA FOR THIS CELL
            string cellName = "" + (char)(65 + col) + (row + 1);
            string cellContent = _Sheet.GetCellContents(cellName).ToString();
            string cellValue = _Sheet.GetCellValue(cellName).ToString();

            // Sets the Cell Name Text to the currently selected cell
            TextCellName.Text = cellName;

            // Sets the Cell Content Text to the currently selected cells content
            if (_Sheet.GetCellContents(cellName) is Formula)
                TextCellContent.Text = "=" + cellContent;
            else
                TextCellContent.Text = cellContent;

            // Sets the Cell Value Text to the currently selected cells value
            if (_Sheet.GetCellValue(cellName) is FormulaError)
                TextCellValue.Text = "Formula Error";
            else
                TextCellValue.Text = cellValue;
        }


        /// <summary>
        /// Action that is performed when the 'Set Value' button is clicked. This action will set the value of the currently selected cell
        /// along with all cells that depend of the selected cell based on the information in the 'TextCellContent' text box. If a
        /// CircularException or FormulaFormatException is thrown due to the content being invalid then the user will be prompted with
        /// a detail message box the relates to their problem and the spreadsheet will not be changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetValueButton_Click(object sender, EventArgs e)
        {
            string cellName = TextCellName.Text;
            string cellContent = TextCellContent.Text;
            List<string> CellsToRecalc;

            // Try catch block that catches any invalid Formulas or circular dependency and makes a message box popup that displays
            // a specific message relating to why their formula is invalid. Doesn't modify the spreadsheet panel if this is the case.
            try
            {
                CellsToRecalc = new List<string>(_Sheet.SetContentsOfCell(cellName, cellContent));
            }
            catch (Exception k)
            {
                // CIRCULAR EXCEPTION
                if (k is CircularException)
                    MessageBox.Show("Your formula created a circular dependency loop", "Error Found in Content");
                // FORMULA FORMAT EXCEPTION
                else
                    MessageBox.Show(k.Message, "Error Found in Content");
                return;
            }


            // Foreach block that changes the cell boxes for all cells that need to be recalculated based on the change
            foreach (string cell in CellsToRecalc)
            {
                int row;
                int col;
                GetRowAndCol(cell, out col, out row);
                editSpreadsheetPanel(cell, col, row);
            }
        }


        /// <summary>
        /// Helper method that gets the row and column numbers of the given cell
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void GetRowAndCol (string cellName, out int col, out int row)
        {
            col = (int)cellName[0] - 65;
            row = Int32.Parse(cellName.Substring(1)) - 1;
        }


        /// <summary>
        /// Helper method that edits the 'SpreadsheetPanel' of a given cell and will display the current value for that cell
        /// in the correct row and column. If the value is a FormulaError then "Formula Error" is displayed
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void editSpreadsheetPanel (string cellName, int col, int row)
        {
            object cellValue = _Sheet.GetCellValue(cellName);
            bool selectedCell = false;
            if (cellName == TextCellName.Text)
                selectedCell = true;

            // Displays the cell value on the spreadsheet panel and in the value text box
            if (cellValue is FormulaError)
            {
                if (selectedCell)
                    TextCellValue.Text = "Formula Error";

                SpreadsheetCells.SetValue(col, row, "Formula Error");
            }
            else
            {
                if (selectedCell)
                    TextCellValue.Text = cellValue.ToString();
                SpreadsheetCells.SetValue(col, row, cellValue.ToString());
            }
        }


        /// <summary>
        /// Action that is performed when the 'New' tab under 'File' is clicked. It will create a new Spreadsheet form while 
        /// aslo keeping the previous one open.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetGUI gui = new SpreadsheetGUI();
            gui.Show();
        }


        /// <summary>
        /// Action that is performed when the 'Save' tab under 'File' is clicked. It will prompt the user with the save dialog 
        /// that usually pops up when you save something. They can either save the spreadsheet as a .sprd or under all files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSprd();
        }


        /// <summary>
        /// Action that is performed when the 'Close' tab under 'File' is clicked. It will close the current spreadsheet that the user
        /// is using. If the user has unsaved data in the spreadsheet before closing then he will be prompted with a message
        /// asking if the user would like to save before closing, they can either save by clicking yes, exit by clicking no, or cancel the
        /// closing process by clicking cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // closes the current form which will then run the SpreadsheetGUI_FormClosing action
            this.Close();
        }


        /// <summary>
        /// When the form closing action is performed then this code will run. It will either prompt the user with a message
        /// asking if the user would like to save before closing, they can either save by clicking yes, exit by clicking no, or cancel the
        /// closing process by clicking cancel or it will just continue to close the form if no changes are found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result;

            // If the spreadsheet has been changed since its been opened or created then a message will prompt the user 
            // if they want to save the spreadsheet, cancel the close, or close the spreadsheet without saving
            // If not changed then the spreadsheet closes
            if (_Sheet.Changed)
            {
                // Message box shown when not saved
                result = MessageBox.Show("You have unsaved changes for this spreadsheet would you like to save before you close?",
                    "Closing Spreadsheet", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case (DialogResult.Yes):
                        saveSprd();
                        e.Cancel = false;
                        break;
                    case (DialogResult.No):
                        e.Cancel = false;
                        break;
                    case (DialogResult.Cancel):
                        e.Cancel = true;
                        break;
                }

            }
            else
                e.Cancel = false;
        }


        /// <summary>
        /// Action that is performed when the 'Open' tab under 'File' is clicked. It will prompt the user with the open dialog 
        /// that usually pops up when you open something from file. They can either view .sprd file or view all files, however, if the
        /// file is not a valid spreadsheet file then the user will be prompted with a message saying they can't open this type of file.
        /// The opening feature will replace the current spreadsheet that they have open so if they have any changes on their
        ///  current spreadsheet then they will be asked if they would like to save it before opening a previous spreadsheet file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Opens the open file pop up
            OpenFileDialog open = new OpenFileDialog();

            // Makes it so you can ony read .sprd files or all files
            open.Filter = "Spreadsheet (*.sprd)|*.sprd|All files (*.*)|*.*";

            open.Title = "Open a Spreadsheet File";
            DialogResult result = open.ShowDialog();

            // If statement for when the user clicks the open button in the pop up
            if (result == DialogResult.OK)
            {
                bool cancel = false;
                // If the current spreadsheet they have open has any changes to it then the user will be prompted with with a message
                // asking if they want to save the spreadsheet, cancel the opening process, or open the new spreadsheet without saving
                if (_Sheet.Changed)
                {
                    DialogResult result2 = MessageBox.Show("You have unsaved changes for this spreadsheet would you like to " +
                        "save before you open a new file and close this one?", "Opening Spreadsheet", MessageBoxButtons.YesNoCancel);
                    switch (result2)
                    {
                        case (DialogResult.Yes):
                            saveSprd();
                            break;
                        case (DialogResult.No):
                            break;
                        case (DialogResult.Cancel):
                            cancel = true;
                            break;
                    }
                }
                // If the user clicked yes or no to saving the previous spreadsheet or if the user never had changes to save in the 
                // first place then they will open the spreadsheet file an the GUI will changed accordingly
                if (!cancel)
                {
                    // Try catch block to catch and FileReadWriteException when opening a bad file
                    try
                    {
                        _Sheet = new Spreadsheet(open.FileName, x => Regex.IsMatch(x, @"^[A-Z][1-9][0-9]?$"), x => x.ToUpper(), "ps6");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("You cannot open this type of file. Spreadsheets only!");
                    }

                    // Clears the current spreadsheet and fills it out with  the information in the file being opened
                    SpreadsheetCells.Clear();
                    TextCellContent.Text = _Sheet.GetCellContents(TextCellName.Text).ToString();
                    foreach (string cell in _Sheet.GetNamesOfAllNonemptyCells())
                    {
                        int col;
                        int row;
                        GetRowAndCol(cell, out col, out row);
                        editSpreadsheetPanel(cell, col, row);
                    }
                }
                // If the user clicked cancel then the open pop up is disposed and the user get a message telling them that they canceled
                else
                {
                    open.Dispose();
                    MessageBox.Show("You have canceled the opening process", "Open a Spreadsheet File");
                }
                
            }
            // When the user closes the open pop up then they are prompted with a message
            else
            {
                MessageBox.Show("You have canceled the opening process", "Open a Spreadsheet File");
            }
        }


        /// <summary>
        /// A helper method that deals with the saving process of a spreadsheet
        /// </summary>
        private void saveSprd()
        {
            // Opens the save file pop up
            SaveFileDialog save = new SaveFileDialog();

            // Allow them to save to all files or just as a .sprd
            save.Filter = "Spreadsheet (*.sprd)|*.sprd|All files (*.*)|*.*";
            save.Title = "Save a Spreadsheet File";
            DialogResult result = save.ShowDialog();

            if (result == DialogResult.OK)
            {
                _Sheet.Save(save.FileName);
            }
            else
            {
                MessageBox.Show("You have canceled the saving process", "Save a Spreadsheet File");
            }
        }

        /// <summary>
        /// Action that occurs when the GCD tab is clicked. This will make the rest of the GCD feature visable to the user 
        /// and they can calculate the GCD of two cells.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCDbutton.Visible = true;
            GCDlabel.Visible = true;
            GCDtext1.Visible = true;
            GCDtext2.Visible = true;
        }

        /// <summary>
        /// When the user clicks the calculate GCD button then the code will check if the cells both have integer values in them
        /// and if not will prompt the user about the problem and otherwise will prompt the user what the GCD is of the two cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GCDbutton_Click(object sender, EventArgs e)
        {
            string cell1 = GCDtext1.Text;
            string cell2 = GCDtext2.Text;
            object value1;
            object value2;
            int GCD = 1;

            // If the user inputs invalid cell names then a message pops up letting them know
            try
            {
                value1 = _Sheet.GetCellValue(cell1);
                value2 = _Sheet.GetCellValue(cell2);
            }
            catch
            {
                MessageBox.Show("You must enter a valid cell name in order to calculate the GCD!", "Calculating GCD");
                return;
            }


            if (value1 is double && value2 is double)
            {
                int val1 = (int)(double)value1;
                int val2 = (int)(double)value2;
                // Checks if they are integer values
                if (val1 != (double)value1 && val2 != (double)value2)
                {
                    MessageBox.Show("Your cell inputs must have values that are integers", "Calculating GCD");
                    return;
                }
                // finds the GCD
                for (int i = 1; i <= val1 && i <= val2; i++)
                {
                    if (val1 % i == 0 && val2 % i == 0)
                    {
                        GCD = i;
                    }
                }

                MessageBox.Show("The Greates Common Divosor of these two cells is " + GCD.ToString(), "Calculating GCD");
            }
            else
            {
                MessageBox.Show("Your cell inputs must have values that are integers", "Calculating GCD");
            }

            GCDbutton.Visible = false;
            GCDlabel.Visible = false;
            GCDtext1.Visible = false;
            GCDtext2.Visible = false;
        }


        /// <summary>
        /// This is the help menu that pops up when the user clicks on the help tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1) How To Place A Value Into A Cell: \n \n" +
                "-Type a String, Number, or Formula into the Content text box and press the 'Get Value' button to set the value of that cell \n \n" +
                "-A Formula must have an '=' in front of the equation in order to distinguish it from a regular String \n \n" +
                "-Only valid operators for formulas are '+', '-', '/', and '*' \n \n" +
                "-Click on a cell in order to navigate to a different cell \n \n \n" +
                "2) GCD Tab: \n \n" +
                "-Allows the User enter two cell names and the application will calculate the Greatest common divisor of the two cells \n \n \n" +
                "3) File Tab: \n \n" +
                "-Open: Opens a Spreadsheet (.sprd) file or all files, but still must be a spreadsheet file \n \n" +
                "-New: Allows the User to create a new empty Spreadsheet \n \n" +
                "-Save: Saves the current Spreadsheet to a .sprd file or to all files \n \n" +
                "-Close: Closes the current Spreadsheet \n \n \n" +
                "4) Special Feature: \n \n" +
                "-My special feature is the GCD tab that I implemented");
        }
    }
}
