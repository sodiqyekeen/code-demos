using System;
using System.Linq;

namespace SolutionApp
{
    public class MathsSolver
    {
        public string MissingDigit(string str)
        {
            var equation = new Equation(str);
            var xValue = equation switch
            {
                { } e when e.LeftOperand.Contains('x') => SolveForX(e.Result, e.RightOperand, e.Operator, e.LeftOperand.IndexOf('x'), true),
                { Operator: '-' or '/' } e when e.RightOperand.Contains('x') => SolveForX(e.LeftOperand, e.Result, e.Operator, e.RightOperand.IndexOf('x')),
                { } e when e.RightOperand.Contains('x') => SolveForX(equation.Result, equation.LeftOperand, equation.Operator, equation.RightOperand.IndexOf('x'), true),
                _ => SolveForX(equation.LeftOperand, equation.RightOperand, equation.Operator, equation.Result.IndexOf('x'))
            };

            return xValue.ToString();
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
                Operator = operators.Where(o => str.Contains(o)).FirstOrDefault();

                if (!operators.Any(x => str.Contains(x)))
                    throw new ArgumentException($"{nameof(str)} does not contain a valid operator.");

                var strArray = str.Split(new char[] { Operator, '=' });

                LeftOperand = strArray[0].Trim();
                RightOperand = strArray[1].Trim();
                Result = strArray[2].Trim();
            }

            public override string ToString() => $"{LeftOperand} {Operator} {RightOperand} = {Result}";
        }
    }
}
