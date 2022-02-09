using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <author>
    /// Ryan Sloan
    /// </author>
    
    /// <summary>
    /// A class that contains a public Evaluate method and 2 private helper methods for the Evaluate method.
    /// Evaluator should be used to only call the Evaluate method that evaluates an expression and returns the 
    /// result of that expression
    /// </summary>
    public static class Evaluator
    {
        // Follow PS1 instructions
        public delegate int Lookup(String v);

        /// <summary>
        /// A public method that is used to evaluate an expression and return the result value of that expression.
        /// The expression can contain operators, operand, and variables that start with letters and end with numbers
        /// </summary>
        /// 
        /// <param name="exp"> A String representation of a math expression that can contain variables </param>
        /// 
        /// <param name="variableEvaluator"> A method that is used to find the value of a variable in exp </param>
        /// 
        /// <returns> 
        /// Returns the result value of the expression exp, but instead is now an int otherwise 
        /// it will throw an ArgumentException
        /// </returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            // Seperates all tokens in the expression into an array
            String[] expTokens = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            Stack<int> valueStack = new Stack<int>();
            Stack<String> operatorStack = new Stack<String>();

            // Iterates through the array of tokens
            foreach (String badToken in expTokens)
            {
                String token  = badToken.Trim();

                switch (token)
                {

                    // Case for if token is an empty string
                    case "":
                        break;


                    // Case for if token is a "+" or "-"
                    case "+":
                    case "-":
                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(token);
                            break;
                        }
                        switch(operatorStack.Peek())
                        {

                            // Case for if a "+" is at the top of the stack
                            case "+":

                                // If the  Value Stack doesn't have two values in the stack then there are two(or more)
                                // operators next to each other
                                if (valueStack.Count < 2)
                                    throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                        "to operate on");

                                valueStack.Push(doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                                goto default;


                            // Case for if a "-" is at the top of the stack
                            case "-":

                                // If the  Value Stack doesn't have two values in the stack then there are two(or more)
                                // operators next to each other
                                if (valueStack.Count < 2)
                                    throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                        "to operate on");

                                valueStack.Push(doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                                goto default;

                            
                            // default case that pushes the operator onto the stack
                            // this case is always called in this switch
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
                        if (operatorStack.Count == 0)
                            break;

                        // Check for if a specific operator is at the top of the Operator Stack for when the current token is a ")"
                        switch (operatorStack.Peek())
                        {

                            // Case for if a "+" is at the top of the stack
                            case "+":

                                // If the  Value Stack doesn't have two values in the stack then there are two(or more)
                                // operators next to each other
                                if (valueStack.Count < 2)
                                    throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                        "to operate on");

                                valueStack.Push(doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                                break;


                            // Case for if a "-" is at the top of the stack
                            case "-":

                                // If the  Value Stack doesn't have two values in the stack then there are two(or more)
                                // operators next to each other
                                if (valueStack.Count < 2)
                                    throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                        "to operate on");

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
                                    // If the  Value Stack doesn't have two values in the stack then there are two(or more)
                                    // operators next to each other
                                    if (valueStack.Count < 2)
                                        throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                            "to operate on");

                                    valueStack.Push(doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                                    break;


                                // Case for if a "/" is at the top of the stack
                                case "/":
                                    // If the  Value Stack doesn't have two values in the stack then there are two(or more)
                                    // operators next to each other
                                    if (valueStack.Count < 2)
                                        throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                            "to operate on");
                                    valueStack.Push(doOperation(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                                    break;

                            }
                        }

                        // If there is no "(" token found for the ")" token then an exception is thrown
                        else
                            throw new ArgumentException("In this formula there is a closed parenthesis " +
                                "that does not have an open parenthesis come before it!");

                        break;

                    // Case for if the token is a Integer or a Variable
                    default:
                        // Check for if this token is a Integer
                        int tokenToInt;
                        if (int.TryParse(token, out tokenToInt))
                        {
                            if (operatorStack.Count == 0)
                            {
                                valueStack.Push(tokenToInt);
                                break;
                            }

                            // Check for if a specific operator is at the top of the Operator Stack for when the current token is an Integer
                            switch (operatorStack.Peek())
                            {

                                // Case for if a "*" is at the top of the stack
                                case "*":

                                    // If the  Value Stack doesn't have a value in the stack then there are two(or more)
                                    // operators next to each other
                                    if (valueStack.Count < 1)
                                        throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                            "to operate on");

                                    valueStack.Push(doOperation(tokenToInt, valueStack.Pop(), operatorStack.Pop()));
                                    break;


                                // Case for if a "/" is at the top of the stack
                                case "/":

                                    // If the  Value Stack doesn't have a value in the stack then there are two(or more)
                                    // operators next to each other
                                    if (valueStack.Count < 1)
                                        throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                            "to operate on");

                                    valueStack.Push(doOperation(tokenToInt, valueStack.Pop(), operatorStack.Pop()));
                                    break;


                                // Default case if the first two aren't met
                                // Just pushes the Integer onto the Value Stack
                                default:
                                    valueStack.Push(tokenToInt);
                                    break;
                            }
                        }

                        // Check for if the token is a variable
                        else if (isVariable(token))
                        {
                            int variableToInt = 0;

                            // Try and catch for the variableEvaluator parameter
                            try
                            {
                                variableToInt = variableEvaluator(token);
                            }
                            catch (Exception e)
                            {
                                throw new ArgumentException("The variableEvaluator method passed in threw an exception");
                            }

                            if (operatorStack.Count == 0)
                            {
                                valueStack.Push(variableToInt);
                                break;
                            }

                            // Check for if a specific operator is at the top of the Operator Stack for when the current token is a Variable
                            switch (operatorStack.Peek())
                            {

                                // Case for if a "*" is at the top of the stack
                                case "*":

                                    // If the  Value Stack doesn't have a value in the stack then there are two(or more)
                                    // operators next to each other
                                    if (valueStack.Count < 1)
                                        throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                            "to operate on");

                                    valueStack.Push(doOperation(variableToInt, valueStack.Pop(), operatorStack.Pop()));
                                    break;


                                // Case for if a "/" is at the top of the stack
                                case "/":

                                    // If the  Value Stack doesn't have a value in the stack then there are two(or more)
                                    // operators next to each other
                                    if (valueStack.Count < 1)
                                        throw new ArgumentException("You have one or multiple floating operators that dont have an operand " +
                                            "to operate on");

                                    valueStack.Push(doOperation(variableToInt, valueStack.Pop(), operatorStack.Pop()));
                                    break;


                                // Default case if the first two aren't met
                                // Just pushes the Integer onto the Value Stack
                                default:
                                    valueStack.Push(variableToInt);
                                    break;
                            }
                        }

                        // If the token is not a valid opeator, integer, or variable (or even an empty string) then an exception is thrown
                        else
                            throw new ArgumentException("There is an illegal token in this formula!");
                        break;
                }
            }


            // When the last token in the expression has been processed then only two cases can be possible\

            // First case is if the Operator Stack is empty and the Value Stack has only one integer in it
            // If this is the case then the integer on the value stack is returned
            if (operatorStack.Count == 0 && valueStack.Count == 1)
            {
                return valueStack.Pop();
            }

            // Second case is if there is only one operator("+" or "-") in the Operator Stack and only two values in the Value Stack
            // If this is the case then add or subtract the two integers in the Value Stack and return the result
            else if (operatorStack.Count == 1 && valueStack.Count == 2)
            {
                switch (operatorStack.Peek())
                {
                    
                    // Case for if the last operator is a "+"
                    case "+":
                        operatorStack.Pop();
                        int secondValue = valueStack.Pop();
                        int firstValue = valueStack.Pop();
                        return (firstValue + secondValue);


                    // Case for if the last operator is a "+"
                    case "-":
                        operatorStack.Pop();
                        secondValue = valueStack.Pop();
                        firstValue = valueStack.Pop();
                        return (firstValue - secondValue);

                    
                    // If the last operator is neither a "+" or "-" then there is an error in the expression
                    default:
                        throw new ArgumentException("There is an error in your formula");

                }
            }

            // If non of those two cases are true then there is a floating token in the expression
            else
                throw new ArgumentException("There is a floating operator, number, or variable in your formula!");

        }


        /// <summary>
        /// A helper method for Evaluate that checks if a token is a variable or not.
        /// A valid variable is one that consists of one or more letters(upper or lower case) followed 
        /// by one or more numbers
        /// </summary>
        /// 
        /// <param name="token"> A token from the expression(exp) passed into Evaluate </param>
        /// 
        /// <returns> returns true if token is a valid variable and false otherwise </returns>
        private static bool isVariable (String token)
        {
            String pattern = @"^[A-Z|a-z]+[0-9]+$";
            Regex variable = new Regex(pattern);
            return variable.IsMatch(token);
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
        private static int doOperation (int secondNum, int firstNum, String op)
        {
            int result = 0;
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
    }
}
