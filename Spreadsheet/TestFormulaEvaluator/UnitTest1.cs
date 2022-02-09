using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FormulaEvaluator;

namespace TestFormulaEvaluator
{
    [TestClass]
    public class UnitTest1
    {
        private static int varEval (String var)
        {
            return 3;
        }
        [TestMethod]
        public void TestLongBasicEquation()
        {
            String equation = "8 + 8 * (15 - 10) / 2 - (10 / 2)";
            int result = Evaluator.Evaluate(equation, null);
            Assert.AreEqual(23, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestADivideByZero()
        {
            String equation = "8 + 8 * (15 - 10) / 0 - (10 / 2)";
            int result = Evaluator.Evaluate(equation, null);
        }

        [TestMethod]
        public void TestVariableByItselfBasic()
        {
            String equation = "A3";
            int result = Evaluator.Evaluate(equation, varEval);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestVariableByItselfCrazy()
        {
            String equation = "AaaaAABbbXcAA0123456789";
            int result = Evaluator.Evaluate(equation, varEval);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestVariableInAnEquation()
        {
            String equation = "8 + 10 / 2 + (AASsjsadklHFDHJ01233436545399 * 2)";
            int result = Evaluator.Evaluate(equation, varEval);
            Assert.AreEqual(19, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestVariableThatIsInvalid()
        {
            String equation = "8 + 20 + ASasajk@2193_219";
            int result = Evaluator.Evaluate(equation, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEquationThatIsInvalidFloatingOperator1()
        {
            String equation = "* 8 + 20 + 10";
            int result = Evaluator.Evaluate(equation, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEquationThatIsInvalidFloatingOperator2()
        {
            String equation = "8 + 20 * + - 10";
            int result = Evaluator.Evaluate(equation, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEquationThatIsInvalidParentheses()
        {
            String equation = "8 + 20 * 10)";
            int result = Evaluator.Evaluate(equation, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEquationThatIsInvalid2NumbersZeroOperators()
        {
            String equation = "8   20 * 10";
            int result = Evaluator.Evaluate(equation, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEquationThatIsInvalidNothing()
        {
            String equation = "";
            int result = Evaluator.Evaluate(equation, null);
        }
    }
}
