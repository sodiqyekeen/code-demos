using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolutionApp
{
    public class MathsSolver
    {
        public string MissingDigit(string str)
        {
            var equation = new Equation(str);
            string xValue;
            if (equation.LeftOperand.Contains('x'))
            {
                xValue = SolveForX(leftOperand: equation.Result,
                    rightOperand: equation.RightOperand,
                    _operator: equation.Operator,
                    xPos: equation.LeftOperand.IndexOf('x'),
                    invert: true).ToString();
            }
            else if (equation.RightOperand.Contains('x'))
            {
                if (equation.Operator.Equals('-') || equation.Operator.Equals('/'))
                {
                    xValue = SolveForX(equation.LeftOperand, equation.Result, equation.Operator, equation.RightOperand.IndexOf('x')).ToString();
                }
                else
                {
                    xValue = SolveForX(equation.Result, equation.LeftOperand, equation.Operator, equation.RightOperand.IndexOf('x'), true).ToString();
                }
            }
            else
            {
                xValue = SolveForX(equation.LeftOperand, equation.RightOperand, equation.Operator, equation.Result.IndexOf('x')).ToString();
            }

            return xValue;
        }

        private static char SolveForX(string leftOperand, string rightOperand, char _operator, int xPos, bool invert = false)
        {
            double _leftOperand = Convert.ToDouble(leftOperand);
            double _rightOperand = Convert.ToDouble(rightOperand);
            var result = _operator switch
            {
                '+' => invert ? _leftOperand - _rightOperand : _leftOperand + _rightOperand,
                '-' => invert ? _leftOperand + _rightOperand : _leftOperand - _rightOperand,
                '*' => invert ? _leftOperand / _rightOperand : _leftOperand * _rightOperand,
                '/' => invert ? _leftOperand * _rightOperand : _leftOperand / _rightOperand,
                _ => throw new ArgumentOutOfRangeException(nameof(_operator))
            };
            return result.ToString()[xPos];
        }


        class Equation
        {
            public string LeftOperand { get; set; }
            public string RightOperand { get; set; }
            public char Operator { get; set; }
            public string Result { get; set; }

            public Equation(string str)
            {
                if (string.IsNullOrEmpty(str)) throw new ArgumentNullException("Equation cannot be null or empty!");
                var operators = new char[] { '/', '-', '+', '*' };

                str = str.ToLower();
                if (!str.Contains("x")) throw new ArgumentException($"{nameof(str)} does not contain 'x'.");
                //Get operator
                Operator = operators.Where(o => str.Contains(o)).FirstOrDefault();

                //Split str with the operator and equal sign
                var strArray = str.Split(new char[] { Operator, '=' });

                LeftOperand = strArray[0].Trim();
                RightOperand = strArray[1].Trim();
                Result = strArray[2].Trim();
            }

            public override string ToString() => $"{LeftOperand} {Operator} {RightOperand} = {Result}";
        }
    }
}
