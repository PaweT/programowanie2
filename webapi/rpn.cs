using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rpn_api
{
    class RPN
        {
            private string formula;
            public RPN(string Formula)
            {
                formula = Formula;
            }
            public string IsCorrect()//poprawność formuły
            {
                int lb = 0;
                int rb = 0;
                if (formula.Length == 0)
                {
                    return "Please enter the formula!";
                }
                else if (formula[formula.Length - 1] == '+' || formula[formula.Length - 1] == '-' || formula[formula.Length - 1] == '*' || formula[formula.Length - 1] == '/' || formula[formula.Length - 1] == '^')
                {
                    return "Operator at the end!";
                }
                else if (formula[0] == '+' || formula[0] == '*' || formula[0] == '/' || formula[0] == '^')
                {
                    return "Operator at the beginning!";
                }
                for (int i = 0; i < formula.Length; i++)
                {
                    if (formula[i] == '(')
                    {
                        if(formula[i + 1] ==')')
                        {
                            return "Empty brackets!";
                        }
                        lb += 1;
                    }
                    else if (formula[i] == ')')
                    {
                        rb += 1;
                    }
                    else if (formula[i] == '/' && formula[i + 1] == '0')
                    {
                        return "Don't divide by zero!";
                    }
                    else if ((formula[i] == '+' || formula[i] == '-' || formula[i] == '*' || formula[i] == '/' || formula[i] == '^') && (formula[i + 1] == '+' || formula[i + 1] == '-' || formula[i + 1] == '*' || formula[i + 1] == '/' || formula[i + 1] == '^' || formula[i + 1] == ')'))
                    {
                        return "Two operators side by side!";
                    }
                }
                if (lb != rb)
                {
                    return "Wrong number of brackets!";
                }
                return "true";
            }
            public List<string> Tokens()
            {
                List<string> list = new List<string>();
                if (formula[0] == '-')
                {
                    formula = formula.Insert(0, "0");
                }
                for (int i = 0; i < formula.Length; i++)
                {
                    if (formula[i] == '-' && formula[i - 1] == '(')
                    {
                        list.Add(formula[i].ToString() + formula[i + 1].ToString());
                        i++;
                    }
                    else if (Char.IsNumber(formula[i]) == true || formula[i] == 'x' || formula[i] == '+' || formula[i] == '-' || formula[i] == '*' || formula[i] == '/' || formula[i] == '^' || formula[i] == '(' || formula[i] == ')')
                    {
                        list.Add(formula[i].ToString());
                    }
                    else if (formula[i] == 's' && formula[i + 1] == 'i' && formula[i + 2] == 'n' && formula[i + 3] == 'h' || formula[i] == 'c' && formula[i + 1] == 'o' && formula[i + 2] == 's' && formula[i + 3] == 'h' || formula[i] == 't' && formula[i + 1] == 'a' && formula[i + 2] == 'n' && formula[i + 3] == 'h' || formula[i] == 'a' && formula[i + 1] == 's' && formula[i + 2] == 'i' && formula[i + 3] == 'n' || formula[i] == 'a' && formula[i + 1] == 'c' && formula[i + 2] == 'o' && formula[i + 3] == 's' || formula[i] == 'a' && formula[i + 1] == 't' && formula[i + 2] == 'a' && formula[i + 3] == 'n' || formula[i] == 's' && formula[i + 1] == 'q' && formula[i + 2] == 'r' && formula[i + 3] == 't')
                    {
                        list.Add(formula[i].ToString() + formula[i + 1].ToString() + formula[i + 2].ToString() + formula[i + 3].ToString());
                        i++; i++; i++;
                    }
                    else if (formula[i] == 's' && formula[i + 1] == 'i' && formula[i + 2] == 'n' || formula[i] == 'c' && formula[i + 1] == 'o' && formula[i + 2] == 's' || formula[i] == 't' && formula[i + 1] == 'a' && formula[i + 2] == 'n' || formula[i] == 'a' && formula[i + 1] == 'b' && formula[i + 2] == 's' || formula[i] == 'e' && formula[i + 1] == 'x' && formula[i + 2] == 'p' || formula[i] == 'l' && formula[i + 1] == 'o' && formula[i + 2] == 'g')
                    {
                        list.Add(formula[i].ToString() + formula[i + 1].ToString() + formula[i + 2].ToString());
                        i++; i++;
                    }
                    if (i > 0)
                    {
                        if (formula[i] == '.')
                        {
                            list.Add(list[list.Count - 1] + "," + formula[i + 1].ToString());
                            list.RemoveAt(list.Count - 2);
                            i++;
                        }
                        else if (Char.IsNumber(formula[i - 1]) == true && Char.IsNumber(formula[i]) == true)
                        {
                            list.Add(list[list.Count - 2] + formula[i].ToString());
                            list.RemoveAt(list.Count - 2);
                            list.RemoveAt(list.Count - 2);
                        }
                    }
                }
                return list;
            }//zamiana na tokeny
            public  string[] InfixToPostfix(List<string> formula)
            {
                Stack<string> S = new Stack<string>();
                Queue<string> Q = new Queue<string>();
                Dictionary<string, int> D = new Dictionary<string, int>();
                {
                    D.Add("+", 1);
                    D.Add("-", 1);
                    D.Add("*", 2);
                    D.Add("/", 2);
                    D.Add("^", 3);
                    D.Add("sin", 4);
                    D.Add("cos", 4);
                    D.Add("tan", 4);
                    D.Add("sinh", 4);
                    D.Add("cosh", 4);
                    D.Add("tanh", 4);
                    D.Add("asin", 4);
                    D.Add("acos", 4);
                    D.Add("atan", 4);
                    D.Add("abs", 4);
                    D.Add("exp", 4);
                    D.Add("log", 4);
                    D.Add("sqrt", 4);
                    D.Add("(", 0);
                }//Wypełnienie słownika
                for (int i = 0; i < formula.Count; i++)
                {
                    if (!("()^*/+-".Contains(formula[i])) && !(D.ContainsKey(formula[i])))
                        Q.Enqueue(formula[i].ToString());
                    else if (formula[i] == "(")
                    {
                        S.Push(formula[i].ToString());
                    }
                    else if (formula[i] == ")")
                    {
                        if (S.Peek() != "(")
                        {
                            Q.Enqueue(S.Pop());
                        }
                        S.Pop();
                    }
                    else if (D.ContainsKey(formula[i].ToString()) == true)
                    {
                        if (S.Count > 0 && D[formula[i].ToString()] <= D[S.Peek()])
                        {
                            Q.Enqueue(S.Pop());
                        }
                        S.Push(formula[i].ToString());
                    }
                    else if (Char.IsNumber((formula[i]), 0) == true || formula[i] == "x")
                    {
                        Q.Enqueue(formula[i].ToString());
                    }
                }
                while (S.Count > 0)
                {
                    Q.Enqueue(S.Pop());
                }
                string[] A = new string[Q.Count];
                Q.CopyTo(A, 0);
                return A;
            }//zamiana z postaci infixowej na postfixowa
            public static double PostfixCalc(string[] str)
            {
                Stack<double> S = new Stack<double>();
                for (int i = 0; i < str.Length; i++)
                {
                    if (Char.IsNumber((str[i]), 0) == true || str[i].Length > 1 && str[i][0] == '-')
                    {
                        S.Push(Double.Parse(str[i]));
                    }
                    else if (str[i] == "sin" || str[i] == "cos" || str[i] == "tan" || str[i] == "sinh" || str[i] == "cosh" || str[i] == "tanh" || str[i] == "asin" || str[i] == "acos" || str[i] == "atan" || str[i] == "abs" || str[i] == "exp" || str[i] == "log" || str[i] == "sqrt")
                    {
                        double tmp = S.Pop();
                        if (str[i] == "sin") { tmp = Math.Sin(tmp); }
                        else if (str[i] == "cos") { tmp = Math.Cos(tmp); }
                        else if (str[i] == "tan") { tmp = Math.Tan(tmp); }
                        else if (str[i] == "sinh") { tmp = Math.Sinh(tmp); }
                        else if (str[i] == "cosh") { tmp = Math.Cosh(tmp); }
                        else if (str[i] == "tanh") { tmp = Math.Tanh(tmp); }
                        else if (str[i] == "asin") { tmp = Math.Asin(tmp); }
                        else if (str[i] == "acos") { tmp = Math.Acos(tmp); }
                        else if (str[i] == "atan") { tmp = Math.Atan(tmp); }
                        else if (str[i] == "abs") { tmp = Math.Abs(tmp); }
                        else if (str[i] == "exp") { tmp = Math.Exp(tmp); }
                        else if (str[i] == "log") { tmp = Math.Log(tmp); }
                        else if (str[i] == "sqrt") { tmp = Math.Sqrt(tmp); }
                        S.Push(tmp);
                    }
                    else if (str[i] == "+" || str[i] == "-" || str[i] == "*" || str[i] == "/" || str[i] == "^")
                    {
                        double a = S.Pop();
                        double b = S.Pop();
                        if (str[i] == "+") { a = a + b; }
                        else if (str[i] == "-") { a = b - a; }
                        else if (str[i] == "*") { a = a * b; }
                        else if (str[i] == "/") { a = b / a; }
                        else if (str[i] == "^") { a = Math.Pow(b, a); }
                        S.Push(a);
                    }
                }
                return S.Pop();
            }//liczenie formuły
            public double CalX(List<string> f, double arg)
            {
                List<int> w = new List<int>();
                for (int i = 0; i < f.Count; i++)
                {
                    if (f[i] == "x")
                    {
                        w.Add(i);
                    }
                }
                for (int k = 0; k < w.Count; k++)
                {
                    f[w[k]] = arg.ToString();
                }
                string[] Tab = InfixToPostfix(f);
                return PostfixCalc(Tab);
            }//wartość formuły dla podanego X
            public List<double> MinMax(List<string> f, double xMin, double xMax, double n)
            {
                List<int> w = new List<int>();
                List<double> r = new List<double>();
                for (int i = 0; i < f.Count; i++)
                {
                    if (f[i] == "x")
                    {
                        w.Add(i);
                    }
                }
                double diff = ((xMax - xMin) / (n - 1));
                while (xMin <= xMax)
                {
                    for (int k = 0; k < w.Count; k++)
                    {
                        f[w[k]] = xMin.ToString();
                    }
                    string[] Tab = InfixToPostfix(f);
                    r.Add(xMin);
                    r.Add(PostfixCalc(Tab));
                    xMin = xMin + diff;
                }
            return r;
        }//wartość formuły w n punktach w przedziale od xMin do xMax
    }
}