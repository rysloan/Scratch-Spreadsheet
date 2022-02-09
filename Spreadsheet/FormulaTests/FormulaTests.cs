using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DevelopmentTests

{


    [TestClass]
    public class FormulaTests
    {

        [TestMethod]
        public void TestDivideByZero()
        {
            Formula f = new Formula("3 / A1");
            Assert.IsInstanceOfType(f.Evaluate(x => 0), typeof(FormulaError));
        }

        [TestMethod]
        public void TestDivideByZero2()
        {
            Formula f = new Formula("3 / 0");
            Assert.IsInstanceOfType(f.Evaluate(x => 0), typeof(FormulaError));
        }

        [TestMethod]
        public void TestDivideByZero3()
        {
            Formula f = new Formula("2 / (3 - 3)");
            Assert.IsInstanceOfType(f.Evaluate(x => 0), typeof(FormulaError));
        }

        [TestMethod]
        public void TestConstructorSimple()
        {
            Formula f = new Formula("4 + 4.3");
        }

        [TestMethod]
        public void TestConstructorVariable()
        {
            Formula f = new Formula("A3 + _dsadsakjdbj4444444____4_8");
        }

        [TestMethod]
        public void TestConstructorParenthesisCanFollowParenthesis()
        {
            Formula f = new Formula("(8 * (4 + 4))");
        }

        /// <summary>
        /// Test if an operator follows an operator the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula()
        {
            Formula f = new Formula("4 + + 8");
        }

        /// <summary>
        /// Test if a variable follows a number the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula2()
        {
            Formula f = new Formula("4 x + 8");
        }

        /// <summary>
        /// Test if a number follows a number the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula3()
        {
            Formula f = new Formula("4.0 4.3 + 8");
        }

        /// <summary>
        /// Test if an operator is the first token the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula4()
        {
            Formula f = new Formula("* 4.0 + 8");
        }

        /// <summary>
        /// Test if an operator is the last token the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula5()
        {
            Formula f = new Formula("4.0 + 8 /");
        }

        /// <summary>
        /// Test if there are more closing parenthesis then open parenthesis the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula6()
        {
            Formula f = new Formula("(4.0 + 10) + 8)");
        }

        /// <summary>
        /// Test if there are more open parenthesis then closing parenthesis at the end the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula7()
        {
            Formula f = new Formula("((4.0 + 10) + (8)");
        }

        /// <summary>
        /// Test if there is an invalid token for the base validator the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula8()
        {
            Formula f = new Formula("4.0 + $2 / 10");
        }

        /// <summary>
        /// Test if the formula is empty the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula9()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// Test if a closed parenthesis follows an operator the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula10()
        {
            Formula f = new Formula("(8 +) + 10");
        }

        /// <summary>
        /// Test if a open parenthesis follows a number the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormula11()
        {
            Formula f = new Formula("8(10 + 3)");
        }

        /// <summary>
        /// Test if when the user passes in their own IsValid method it will define new valid variables
        /// </summary>
        [TestMethod]
        public void TestConstructorProvidedIsValid()
        {
            string pattern = @"^[A-Z][1-9]?$";
            Regex var = new Regex(pattern);
            Formula f = new Formula("4.0 + A1", x => x, x => var.IsMatch(x));
        }

        /// <summary>
        /// Test if when the user passes in their own IsValid method when a variable isn't valid the constructor will throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorProvidedIsValidInvalidVar()
        {
            string pattern = @"^[A-Z][1-9]?$";
            Regex var = new Regex(pattern);
            Formula f = new Formula("4.0 + A12", x => x, x => var.IsMatch(x));
        }

        [TestMethod]
        public void TestToStringSimple()
        {
            Formula f = new Formula("4.0 + A1");
            Assert.AreEqual("4+A1", f.ToString());
            Formula j = new Formula("4.3 + A1");
            Assert.AreEqual("4.3+A1", j.ToString());
        }

        [TestMethod]
        public void TestToStringScientificNotation()
        {
            Formula f = new Formula("4.6e3 + A1");
            Assert.AreEqual("4600+A1", f.ToString());
        }


        [TestMethod]
        public void TestToStringWithNormalizer()
        {
            Formula f = new Formula("4.0 + a1", x => x.ToUpper(), x => true);
            Assert.AreEqual("4+A1", f.ToString());
        }

        [TestMethod]
        public void TestGetVariablesWithNormalizer()
        {
            Formula f = new Formula("4.0 + a1 + b2 + ccd_25", x => x.ToUpper(), x => true);
            List<string> vars = new List<string>(f.GetVariables());
            Assert.AreEqual("A1", vars[0]);
            Assert.AreEqual("B2", vars[1]);
            Assert.AreEqual("CCD_25", vars[2]);
        }

        [TestMethod]
        public void TestEqualsSimple()
        {
            Formula f = new Formula("4.0 + A1");
            Formula j = new Formula("4.0+A1");
            Formula j2 = new Formula("6.0+A1");
            Assert.IsTrue(f.Equals(j));
            Assert.IsTrue(f == j);
            Assert.IsFalse(f != j);
            Assert.IsFalse(f.Equals(j2));
        }

        [TestMethod]
        public void TestEqualsNull()
        {
            int k = 10;
            Formula f = new Formula("4.0 + A1");
            Formula j = null;
            Formula j2 = null;
            Assert.IsFalse(f.Equals(k));
            Assert.IsFalse(f.Equals(j));
            Assert.IsTrue(j2 == j);
            Assert.IsFalse(j2 != j);
        }

        [TestMethod]
        public void TestEqualsAdvanced()
        {
            Formula f = new Formula("40.0 + A1");
            Formula j = new Formula("40.0000+A1");
            Formula j2 = new Formula("4e1+A1");
            Assert.IsTrue(f.Equals(j));
            Assert.IsTrue(f.Equals(j2));
        }



        [TestMethod]
        public void TestEvaluateSimple()
        {
            Formula f = new Formula("4 + 4");
            Assert.AreEqual(8.0, f.Evaluate(null));
        }

        /// <summary>
        /// Test decimal math
        /// </summary>
        [TestMethod]
        public void TestEvaluateDecimal()
        {
            Formula f = new Formula("3.6 - 1.2");
            Assert.AreEqual(2.4, (double) f.Evaluate(null), 0.0001);
        }

        /// <summary>
        /// Test really long formula to hit the edge cases
        /// </summary>
        [TestMethod]
        public void TestEvaluateAdvanced()
        {
            Formula f = new Formula("10 + 5 * (10 / 2 + 3) - 15 + 30 * 2 / 2 + (8 * (2 + 2) / 8)");
            Assert.AreEqual(69.0, f.Evaluate(null));
        }

        /// <summary>
        /// Test really long formula of divides to hit the edge cases
        /// </summary>
        [TestMethod]
        public void TestEvaluateAdvanced2()
        {
            Formula f = new Formula("A1 / 5 / (10 / 2 / 3) / 5 / 30 / 2 / 2 / (8 / (2 / 2) / 8)");
            Assert.AreEqual(0.002, f.Evaluate(x => 10.0));
        }

        /// <summary>
        /// Test that a variable can be first in a formula
        /// </summary>
        [TestMethod]
        public void TestEvaluateAdvanced3()
        {
            Formula f = new Formula("A1 + 8");
            Assert.IsInstanceOfType(f.Evaluate(null), typeof(FormulaError));
        }

        /// <summary>
        /// Test really long formula of variables to hit the edge cases
        /// </summary>
        [TestMethod]
        public void TestEvaluateAllVariables()
        {
            Formula f = new Formula("A1 + B500 * (_ / _2 + ZZ0) - BBBBBBBBBBBBB + tuff * sara / lol_XD + (sheeeeeeesh99 * (ppp + _404) / _1_1_1_1_1)");
            Assert.AreEqual(140.0, f.Evaluate(x => 10.0));
        }
    }
}

namespace GradingTests
{
    [TestClass]
    public class GradingTests
    {

        // Normalizer tests
        [TestMethod(), Timeout(2000)]
        [TestCategory("1")]
        public void TestNormalizerGetVars()
        {
            Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
            HashSet<string> vars = new HashSet<string>(f.GetVariables());

            Assert.IsTrue(vars.SetEquals(new HashSet<string> { "X1" }));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("2")]
        public void TestNormalizerEquals()
        {
            Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("2+X1", s => s.ToUpper(), s => true);

            Assert.IsTrue(f.Equals(f2));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("3")]
        public void TestNormalizerToString()
        {
            Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
            Formula f2 = new Formula(f.ToString());

            Assert.IsTrue(f.Equals(f2));
        }

        // Validator tests
        [TestMethod(), Timeout(2000)]
        [TestCategory("4")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidatorFalse()
        {
            Formula f = new Formula("2+x1", s => s, s => false);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("5")]
        public void TestValidatorX1()
        {
            Formula f = new Formula("2+x", s => s, s => (s == "x"));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("6")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidatorX2()
        {
            Formula f = new Formula("2+y1", s => s, s => (s == "x"));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("7")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidatorX3()
        {
            Formula f = new Formula("2+x1", s => s, s => (s == "x"));
        }


        // Simple tests that return FormulaErrors
        [TestMethod(), Timeout(2000)]
        [TestCategory("8")]
        public void TestUnknownVariable()
        {
            Formula f = new Formula("2+X1");
            Assert.IsInstanceOfType(f.Evaluate(s => { throw new ArgumentException("Unknown variable"); }), typeof(FormulaError));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("9")]
        public void TestDivideByZero()
        {
            Formula f = new Formula("5/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("10")]
        public void TestDivideByZeroVars()
        {
            Formula f = new Formula("(5 + X1) / (X1 - 3)");
            Assert.IsInstanceOfType(f.Evaluate(s => 3), typeof(FormulaError));
        }


        // Tests of syntax errors detected by the constructor
        [TestMethod(), Timeout(2000)]
        [TestCategory("11")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSingleOperator()
        {
            Formula f = new Formula("+");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("12")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraOperator()
        {
            Formula f = new Formula("2+5+");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("13")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraCloseParen()
        {
            Formula f = new Formula("2+5*7)");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("14")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraOpenParen()
        {
            Formula f = new Formula("((3+5*7)");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("15")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperator()
        {
            Formula f = new Formula("5x");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("16")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperator2()
        {
            Formula f = new Formula("5+5x");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("17")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperator3()
        {
            Formula f = new Formula("5+7+(5)8");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("18")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperator4()
        {
            Formula f = new Formula("5 5");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("19")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestDoubleOperator()
        {
            Formula f = new Formula("5 + + 3");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("20")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmpty()
        {
            Formula f = new Formula("");
        }

        // Some more complicated formula evaluations
        [TestMethod(), Timeout(2000)]
        [TestCategory("21")]
        public void TestComplex1()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("22")]
        public void TestRightParens()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("23")]
        public void TestLeftParens()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("53")]
        public void TestRepeatedVar()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
        }

        // Test of the Equals method
        [TestMethod(), Timeout(2000)]
        [TestCategory("24")]
        public void TestEqualsBasic()
        {
            Formula f1 = new Formula("X1+X2");
            Formula f2 = new Formula("X1+X2");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("25")]
        public void TestEqualsWhitespace()
        {
            Formula f1 = new Formula("X1+X2");
            Formula f2 = new Formula(" X1  +  X2   ");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("26")]
        public void TestEqualsDouble()
        {
            Formula f1 = new Formula("2+X1*3.00");
            Formula f2 = new Formula("2.00+X1*3.0");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("27")]
        public void TestEqualsComplex()
        {
            Formula f1 = new Formula("1e-2 + X5 + 17.00 * 19 ");
            Formula f2 = new Formula("   0.0100  +     X5+ 17 * 19.00000 ");
            Assert.IsTrue(f1.Equals(f2));
        }


        [TestMethod(), Timeout(2000)]
        [TestCategory("28")]
        public void TestEqualsNullAndString()
        {
            Formula f = new Formula("2");
            Assert.IsFalse(f.Equals(null));
            Assert.IsFalse(f.Equals(""));
        }


        // Tests of == operator
        [TestMethod(), Timeout(2000)]
        [TestCategory("29")]
        public void TestEq()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsTrue(f1 == f2);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("30")]
        public void TestEqFalse()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("5");
            Assert.IsFalse(f1 == f2);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("31")]
        public void TestEqNull()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsFalse(null == f1);
            Assert.IsFalse(f1 == null);
            Assert.IsTrue(f1 == f2);
        }


        // Tests of != operator
        [TestMethod(), Timeout(2000)]
        [TestCategory("32")]
        public void TestNotEq()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsFalse(f1 != f2);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("33")]
        public void TestNotEqTrue()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("5");
            Assert.IsTrue(f1 != f2);
        }


        // Test of ToString method
        [TestMethod(), Timeout(2000)]
        [TestCategory("34")]
        public void TestString()
        {
            Formula f = new Formula("2*5");
            Assert.IsTrue(f.Equals(new Formula(f.ToString())));
        }


        // Tests of GetHashCode method
        [TestMethod(), Timeout(2000)]
        [TestCategory("35")]
        public void TestHashCode()
        {
            Formula f1 = new Formula("2*5");
            Formula f2 = new Formula("2*5");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        }

        // Technically the hashcodes could not be equal and still be valid,
        // extremely unlikely though. Check their implementation if this fails.
        [TestMethod(), Timeout(2000)]
        [TestCategory("36")]
        public void TestHashCodeFalse()
        {
            Formula f1 = new Formula("2*5");
            Formula f2 = new Formula("3/8*2+(7)");
            Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode());
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("37")]
        public void TestHashCodeComplex()
        {
            Formula f1 = new Formula("2 * 5 + 4.00 - _x");
            Formula f2 = new Formula("2*5+4-_x");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        }


        // Tests of GetVariables method
        [TestMethod(), Timeout(2000)]
        [TestCategory("38")]
        public void TestVarsNone()
        {
            Formula f = new Formula("2*5");
            Assert.IsFalse(f.GetVariables().GetEnumerator().MoveNext());
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("39")]
        public void TestVarsSimple()
        {
            Formula f = new Formula("2*X2");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X2" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("40")]
        public void TestVarsTwo()
        {
            Formula f = new Formula("2*X2+Y3");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "Y3", "X2" };
            Assert.AreEqual(actual.Count, 2);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("41")]
        public void TestVarsDuplicate()
        {
            Formula f = new Formula("2*X2+X2");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X2" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("42")]
        public void TestVarsComplex()
        {
            Formula f = new Formula("X1+Y2*X3*Y2+Z7+X1/Z8");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X1", "Y2", "X3", "Z7", "Z8" };
            Assert.AreEqual(actual.Count, 5);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        // Tests to make sure there can be more than one formula at a time
        [TestMethod(), Timeout(2000)]
        [TestCategory("43")]
        public void TestMultipleFormulae()
        {
            Formula f1 = new Formula("2 + a1");
            Formula f2 = new Formula("3");
            Assert.AreEqual(2.0, f1.Evaluate(x => 0));
            Assert.AreEqual(3.0, f2.Evaluate(x => 0));
            Assert.IsFalse(new Formula(f1.ToString()) == new Formula(f2.ToString()));
            IEnumerator<string> f1Vars = f1.GetVariables().GetEnumerator();
            IEnumerator<string> f2Vars = f2.GetVariables().GetEnumerator();
            Assert.IsFalse(f2Vars.MoveNext());
            Assert.IsTrue(f1Vars.MoveNext());
        }

        // Repeat this test to increase its weight
        [TestMethod(), Timeout(2000)]
        [TestCategory("44")]
        public void TestMultipleFormulaeB()
        {
            TestMultipleFormulae();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("45")]
        public void TestMultipleFormulaeC()
        {
            TestMultipleFormulae();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("46")]
        public void TestMultipleFormulaeD()
        {
            TestMultipleFormulae();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("47")]
        public void TestMultipleFormulaeE()
        {
            TestMultipleFormulae();
        }

        // Stress test for constructor
        [TestMethod(), Timeout(2000)]
        [TestCategory("48")]
        public void TestConstructor()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // This test is repeated to increase its weight
        [TestMethod(), Timeout(2000)]
        [TestCategory("49")]
        public void TestConstructorB()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("50")]
        public void TestConstructorC()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("51")]
        public void TestConstructorD()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod(), Timeout(2000)]
        [TestCategory("52")]
        public void TestConstructorE()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }


    }
}
