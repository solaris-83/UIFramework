//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ProvafiltroInteri
//{
//    public class ArithmeticParser
//    {
//        private string expression;
//        private int position;
//        private Dictionary<string, double> variables;

//        public double Parse(string expr, double value)
//        {
//            expression = expr.Replace(" ", "");
//            position = 0;
//            variables = new Dictionary<string, double>() { { "x", value } };

//            if (string.IsNullOrEmpty(expression))
//                throw new ArgumentException("Espressione vuota");

//            double result = ParseExpression();

//            if (position < expression.Length)
//                throw new FormatException($"Carattere inaspettato alla posizione {position}: '{expression[position]}'");

//            return result;
//        }

//        public double Parse(string expr, Dictionary<string, double> vars = null)
//        {
//            expression = expr.Replace(" ", "");
//            position = 0;
//            variables = vars ?? new Dictionary<string, double>();

//            if (string.IsNullOrEmpty(expression))
//                throw new ArgumentException("Espressione vuota");

//            double result = ParseExpression();

//            if (position < expression.Length)
//                throw new FormatException($"Carattere inaspettato alla posizione {position}: '{expression[position]}'");

//            return result;
//        }

//        // Metodo per applicare la formula ad una lista di valori
//        public List<double> ParseList(string expr, List<double> values)
//        {
//            var results = new List<double>();
//            var vars = new Dictionary<string, double>();
//            string variableName = "x";

//            foreach (var value in values)
//            {
//                vars[variableName] = value;
//                results.Add(Parse(expr, vars));
//            }

//            return results;
//        }

//        // Metodo per applicare la formula ad una lista di valori
//        public List<double> ParseList(string expr, string variableName, List<double> values)
//        {
//            var results = new List<double>();
//            var vars = new Dictionary<string, double>();

//            foreach (var value in values)
//            {
//                vars[variableName] = value;
//                results.Add(Parse(expr, vars));
//            }

//            return results;
//        }

//        // Gestisce addizione e sottrazione (priorità più bassa)
//        private double ParseExpression()
//        {
//            double left = ParseTerm();

//            while (position < expression.Length)
//            {
//                char op = expression[position];

//                if (op == '+')
//                {
//                    position++;
//                    left += ParseTerm();
//                }
//                else if (op == '-')
//                {
//                    position++;
//                    left -= ParseTerm();
//                }
//                else
//                {
//                    break;
//                }
//            }

//            return left;
//        }

//        // Gestisce moltiplicazione e divisione (priorità più alta)
//        private double ParseTerm()
//        {
//            double left = ParseFactor();

//            while (position < expression.Length)
//            {
//                char op = expression[position];

//                if (op == '*')
//                {
//                    position++;
//                    left *= ParseFactor();
//                }
//                else if (op == '/')
//                {
//                    position++;
//                    double divisor = ParseFactor();
//                    if (Math.Abs(divisor) < double.Epsilon)
//                        throw new DivideByZeroException("Divisione per zero");
//                    left /= divisor;
//                }
//                else
//                {
//                    break;
//                }
//            }

//            return left;
//        }

//        // Gestisce numeri, variabili e parentesi (priorità massima)
//        private double ParseFactor()
//        {
//            if (position >= expression.Length)
//                throw new FormatException("Espressione incompleta");

//            char current = expression[position];

//            // Gestione segno negativo
//            if (current == '-')
//            {
//                position++;
//                return -ParseFactor();
//            }

//            // Gestione segno positivo
//            if (current == '+')
//            {
//                position++;
//                return ParseFactor();
//            }

//            // Gestione parentesi
//            if (current == '(')
//            {
//                position++;
//                double result = ParseExpression();

//                if (position >= expression.Length || expression[position] != ')')
//                    throw new FormatException("Parentesi non bilanciata");

//                position++;
//                return result;
//            }

//            // Gestione variabili (lettere)
//            if (char.IsLetter(current))
//            {
//                return ParseVariable();
//            }

//            // Parsing numero
//            return ParseNumber();
//        }

//        private double ParseVariable()
//        {
//            int start = position;

//            while (position < expression.Length && char.IsLetterOrDigit(expression[position]))
//            {
//                position++;
//            }

//            string varName = expression.Substring(start, position - start);

//            if (!variables.ContainsKey(varName))
//                throw new FormatException($"Variabile '{varName}' non definita");

//            return variables[varName];
//        }

//        private double ParseNumber()
//        {
//            int start = position;

//            while (position < expression.Length)
//            {
//                char c = expression[position];
//                if (char.IsDigit(c) || c == '.' || c == ',')
//                    position++;
//                else
//                    break;
//            }

//            if (start == position)
//                throw new FormatException($"Numero atteso alla posizione {position}");

//            string numberStr = expression.Substring(start, position - start).Replace(',', '.');

//            if (!double.TryParse(numberStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
//                throw new FormatException($"Numero non valido: {numberStr}");

//            return result;
//        }
//    }
//}
