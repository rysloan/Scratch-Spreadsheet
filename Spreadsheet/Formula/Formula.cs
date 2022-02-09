// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        //private delegate bool Validator(string var);
        //private delegate string Normalizer(string var);

        //private Validator _Validator;
        //private Normalizer _Normalizer;
        private string _Expression;

        private Func<string, bool> _Validator;
        private Func<string, string> _Normalizer;

        private HashSet<string> _Variables;


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            _Normalizer = normalize;
            _Validator = isValid;
            _Variables = new HashSet<string>();
            _Expression = "";
            List<string> tokens = new List<string>(GetTokens(formula));
            if (tokens.Count == 0)
            {
                throw new FormulaFormatException("Your formula cannot be empty");
            }

            int openParenthesisCount = 0;
            int closedParenthesisCount = 0;
            bool lastTokenWasOperator = true;

            // Checks if the first token is an illegal token for the start of a formula and throws respectivly
            switch (tokens[0])
            {
                case ("+"):
                case ("-"):
                case ("*"):
                case ("/"):
                case (")"):
                    throw new FormulaFormatException("Cannot start a formula with an invalid token");
            }

            // Checks if the last token is an illegal token for the end of a formula and throws respectivly
            switch (tokens[tokens.Count - 1])
            {
                case ("+"):
                case ("-"):
                case ("*"):
                case ("/"):
                case ("("):
                    throw new FormulaFormatException("Cannot start a formula with an invalid token");
            }


            // Iterates through every token in the formula
            foreach (string token in tokens)
            {
                switch (token)
                {
                    // Case for + , - , / , and *
                    case ("+"):
                    case ("-"):
                    case ("*"):
                    case ("/"):
                        // If the last token in the formula was an operator/opening parenthesis then throw (ex. "4 + + 4" )
                        if (lastTokenWasOperator)
                            throw new FormulaFormatException("Any token that immediately follows an opening parenthesis or an operator must be either " +
                                "a number, a variable, or an opening parenthesis.");
                        else
                        {
                            lastTokenWasOperator = true;
                            // Adds token to this formulas String
                            _Expression += token;
                            break;
                        }

                    // Case for (
                    case ("("):
                        // If the last token in the formula was a number/variable/closing parenthesis then throw (ex. "4 (4 + 4)" )
                        if (lastTokenWasOperator == false)
                            throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either" +
                                " an operator or a closing parenthesis.");
                        else
                        {
                            lastTokenWasOperator = true;
                            _Expression += token;
                            // Increment the amount of open parenthesis
                            openParenthesisCount++;
                            break;
                        }

                    case (")"):
                        // If the last token in the formula was an operator/opening parenthesis then throw (ex. "(4 + 4 *)" )
                        if (lastTokenWasOperator)
                            throw new FormulaFormatException("Any token that immediately follows an opening parenthesis or an operator must be either " +
                                    "a number, a variable, or an opening parenthesis.");
                        else
                        {
                            lastTokenWasOperator = false;
                            _Expression += token;
                            // Increment the amount of closed parenthesis
                            closedParenthesisCount++;
                            // Check if the number of closing parenthsis is more then the opening parenthesis
                            if (closedParenthesisCount > openParenthesisCount)
                            {
                                throw new FormulaFormatException("Ther are more closed parenthesis then open parenthesis");
                            }
                            break;
                        }

                    default:
                        double tokenToDouble;
                        if (Double.TryParse(token, out tokenToDouble))
                        {
                            // If the last token in the formula was a number/variable/closing parenthesis then throw (ex. "4 4 + 4" )
                            if (lastTokenWasOperator == false)
                                throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either" +
                                " an operator or a closing parenthesis.");
                            else
                            {
                                lastTokenWasOperator = false;
                                _Expression += tokenToDouble.ToString();
                            }
                        }
                        else
                        {
                            // Normalize token
                            string normalToken = _Normalizer(token);

                            // Check the standard for a variable
                            if (isVariable(normalToken) == false)
                                throw new FormulaFormatException("There is an invalid token in your formula");

                            // Check the users standard for a variable
                            if (_Validator(normalToken) == false)
                                throw new FormulaFormatException("There is an invalid token in your formula");
                            else
                            {
                                // If the last token in the formula was a number/variable/closing parenthesis then throw (ex. "4 4 + 4" )
                                if (lastTokenWasOperator == false)
                                    throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either" +
                                " an operator or a closing parenthesis.");
                                else
                                {
                                    lastTokenWasOperator = false;
                                    _Expression += normalToken;
                                    _Variables.Add(normalToken);
                                }
                            }
                        }
                        break;

                }
            }
            if (openParenthesisCount != closedParenthesisCount)
                throw new FormulaFormatException("There are an uneven amount of closed and open parenthesis");

        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            // Seperates all tokens in the expression into an array
            List<string> expTokens = new List<string>(GetTokens(_Expression));

            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();

            // Iterates through the array of tokens
            foreach (string token in expTokens)
            {
                ///String token = badToken.Trim();

                switch (token)
                {
                    // Case for if token is a "+" or "-"
                    case "+":
                    case "-":
                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(token);
                            break;
                        }
                        switch (operatorStack.Peek())
                        {

                            // Case for if a "+" is at the top of the stack
                            case "+":
                            // Case for if a "-" is at the top of the stack
                            case "-":
                                valueStack.Push(doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                                operatorStack.Push(token);
                                break;


                            // default case that pushes the operator onto the stack
                            default:
                                operatorStack.Push(token);
                                break;

                        }
                        break;


                    // Case for if token is a "*" , "/" , or "("
                    case "*":
                    case "/":
                    case "(":
                        operatorStack.Push(token);
                        break;


                    // Case for if token is a ")"
                    case ")":

                        // Check for if a specific operator is at the top of the Operator Stack for when the current token is a ")"
                        switch (operatorStack.Peek())
                        {

                            // Case for if a "+" is at the top of the stack
                            case "+":
                            // Case for if a "-" is at the top of the stack
                            case "-":
                                valueStack.Push(doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                                break;

                        }

                        // Finds the "(" token that corresponds to the ")" token
                        if (operatorStack.Count > 0 && operatorStack.Pop() == "(")
                        {

                            if (operatorStack.Count == 0)
                                break;

                            // When the "(" token is found check for if a specific operator is at the top of the Operator Stack
                            switch (operatorStack.Peek())
                            {

                                // Case for if a "*" is at the top of the stack
                                case "*":

                                    valueStack.Push(doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                                    break;


                                // Case for if a "/" is at the top of the stack
                                case "/":

                                    try
                                    {
                                        valueStack.Push(doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                                    }
                                    catch (Exception)
                                    {
                                        return new FormulaError("Cannot divide by Zero");
                                    }
                                    break;

                            }
                        }
                        break;

                    // Case for if the token is a Integer or a Variable
                    default:
                        // Check for if this token is a Integer
                        double tokenToDouble;
                        if (Double.TryParse(token, out tokenToDouble))
                        {
                            if (operatorStack.Count == 0)
                            {
                                valueStack.Push(tokenToDouble);
                                break;
                            }

                            // Check for if a specific operator is at the top of the Operator Stack for when the current token is an Integer
                            switch (operatorStack.Peek())
                            {

                                // Case for if a "*" is at the top of the stack
                                case "*":

                                    valueStack.Push(doOperation(tokenToDouble, valueStack.Pop(), operatorStack.Pop()));
                                    break;


                                // Case for if a "/" is at the top of the stack
                                case "/":

                                    try
                                    {
                                        valueStack.Push(doOperation(tokenToDouble, valueStack.Pop(), operatorStack.Pop()));
                                    }
                                    catch (Exception)
                                    {
                                        return new FormulaError("Cannot divide by Zero");
                                    }
                                    break;


                                // Default case if the first two aren't met
                                // Just pushes the Integer onto the Value Stack
                                default:
                                    valueStack.Push(tokenToDouble);
                                    break;
                            }
                        }

                        // Check for if the token is a variable
                        else
                        {
                            double variableToDouble = 0;

                            // Try and catch for the variableEvaluator parameter
                            try
                            {
                                variableToDouble = lookup(token);
                            }
                            catch (Exception)
                            {
                                return new FormulaError("The variable lookup method you passed in could not find a value for your variable");
                            }

                            if (operatorStack.Count == 0)
                            {
                                valueStack.Push(variableToDouble);
                                break;
                            }

                            // Check for if a specific operator is at the top of the Operator Stack for when the current token is a Variable
                            switch (operatorStack.Peek())
                            {

                                // Case for if a "*" is at the top of the stack
                                case "*":

                                    valueStack.Push(doOperation(variableToDouble, valueStack.Pop(), operatorStack.Pop()));
                                    break;


                                // Case for if a "/" is at the top of the stack
                                case "/":

                                    try
                                    {
                                        valueStack.Push(doOperation(variableToDouble, valueStack.Pop(), operatorStack.Pop()));
                                    }
                                    catch (Exception)
                                    {
                                        return new FormulaError("Cannot divide by Zero");
                                    }
                                    break;


                                // Default case if the first two aren't met
                                // Just pushes the Integer onto the Value Stack
                                default:
                                    valueStack.Push(variableToDouble);
                                    break;
                            }
                        }
                        break;
                }
            }
            // When the last token in the expression has been processed then only two cases can be possible\

            // First case is if the Operator Stack is empty and the Value Stack has only one integer in it
            // If this is the case then the integer on the value stack is returned
            if (operatorStack.Count == 0 && valueStack.Count == 1)
                return valueStack.Pop();

            // Second case is if there is only one operator("+" or "-") in the Operator Stack and only two values in the Value Stack
            // If this is the case then add or subtract the two integers in the Value Stack and return the result
            else
                return doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());

        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return _Variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return _Expression;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Formula) || obj == null)
            {
                return false;
            }
            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (f1 is null && f2 is null)
                return true;
            else if (f1 is null || f2 is null)
            {
                return false;
            }
            else
            {
                return f1.Equals(f2);
            }
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (f1 is null && f2 is null)
                return false;
            else
            {
                return !(f1.Equals(f2));
            }
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return _Expression.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }


        /// <summary>
        /// A helper method for evaluate that does and operation (addition, subtraction, division, or multiplication)
        /// for two numbers and an operator while returning the result
        /// </summary>
        /// 
        /// <param name="secondNum"> The number that goes to the right of the operator </param>
        /// 
        /// <param name="firstNum"> The numbre that goes to the left of the operator </param>
        /// 
        /// <param name="op"> The operator of the operation, Can be a /, *, +, or - </param>
        /// 
        /// <returns> returns the result of the operation </returns>
        private static double doOperation(double secondNum, double firstNum, String op)
        {
            double result = 0.0;
            switch (op)
            {
                case "+":
                    result = firstNum + secondNum;
                    break;


                case "-":
                    result = firstNum - secondNum;
                    break;


                case "*":
                    result = firstNum * secondNum;
                    break;


                case "/":
                    if (secondNum == 0)
                    {
                        throw new ArgumentException("You cannont divide a number by zero");
                    }
                    result = firstNum / secondNum;
                    break;
            }

            return result;
        }


        private static bool isVariable(String token)
        {
            String pattern = @"^[A-Z|a-z|_][0-9|A-Z|a-z|_]*$";
            Regex variable = new Regex(pattern);
            return variable.IsMatch(token);
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}

