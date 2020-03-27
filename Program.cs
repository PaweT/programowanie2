using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN
{
    class Program
    {
        public class RPN
        {
            public RPN(string formula, int x, double xmin, double xmax, double n)
            {
                List<string> list = new List<string>();
                list = Tokens(formula);
                for (int i = 0; i < list.Count; i++)
                {
                    if(list[i] == "0") { continue; }
                    Console.Write(list[i] + " ");
                }
                Console.Write("\n");
                if (IsCorrect(formula) == true)
                {
                    string[] PostfixTab = InfixToPostfix(Tokens(formula));
                    for (int i = 0; i < PostfixTab.Length; i++)
                    {
                        if (PostfixTab[i] == "0") { continue; }
                        Console.Write(PostfixTab[i] + " ");
                    }
                    Console.Write("\n");
                    Console.WriteLine(CalX(Tokens(formula), x));
                    MinMax(Tokens(formula), xmin, xmax, n);
                }
                else
                {
                    Console.Write("Something is not working\n");
                }
            }
            public static bool IsCorrect(string formula)//poprawność formuły
            {
                int lb = 0;
                int rb = 0;
                if (formula.Length == 0)
                {
                    Console.WriteLine("Please enter the formula!");
                    return false;
                }
                else if (formula[formula.Length - 1] == '+' || formula[formula.Length - 1] == '-' || formula[formula.Length - 1] == '*' || formula[formula.Length - 1] == '/' || formula[formula.Length - 1] == '^')
                {
                    Console.WriteLine("Operator at the end!");
                    return false;
                }
                else if (formula[0] == '+' || formula[0] == '*' || formula[0] == '/' || formula[0] == '^')
                {
                    Console.WriteLine("Operator at the beginning!");
                    return false;
                }
                for (int i = 0; i < formula.Length; i++)
                {
                    if (formula[i] == '(')
                    {
                        lb += 1;
                    }
                    else if (formula[i] == ')')
                    {
                        rb += 1;
                    }
                    else if (formula[i] == '/' && formula[i + 1] == '0')
                    {
                        Console.WriteLine("Don't divide by zero!");
                        return false;
                    }
                    else if ((formula[i] == '+' || formula[i] == '-' || formula[i] == '*' || formula[i] == '/' || formula[i] == '^') && (formula[i + 1] == '+' || formula[i + 1] == '-' || formula[i + 1] == '*' || formula[i + 1] == '/' || formula[i + 1] == '^' || formula[i + 1] == ')'))
                    {
                        Console.WriteLine("Two operators side by side!");
                        return false;
                    }
                }
                if (lb != rb)
                {
                    Console.WriteLine("Wrong number of brackets!");
                    return false;
                }
                return true;
            }
            public static List<string> Tokens(string T)
            {
                List<string> list = new List<string>();
                if (T[0] == '-')
                {
                    T = T.Insert(0, "0");
                }
                for (int i = 0; i < T.Length; i++)
                {
                    if (T[i] == '-' && T[i - 1] == '(')
                    {
                        list.Add(T[i].ToString() + T[i + 1].ToString());
                        i++;
                    }
                    else if (Char.IsNumber(T[i]) == true || T[i] == 'x' || T[i] == '+' || T[i] == '-' || T[i] == '*' || T[i] == '/' || T[i] == '^' || T[i] == '(' || T[i] == ')')
                    {
                        list.Add(T[i].ToString());
                    }
                    else if (T[i] == 's' && T[i + 1] == 'i' && T[i + 2] == 'n' && T[i + 3] == 'h' || T[i] == 'c' && T[i + 1] == 'o' && T[i + 2] == 's' && T[i + 3] == 'h' || T[i] == 't' && T[i + 1] == 'a' && T[i + 2] == 'n' && T[i + 3] == 'h' || T[i] == 'a' && T[i + 1] == 's' && T[i + 2] == 'i' && T[i + 3] == 'n' || T[i] == 'a' && T[i + 1] == 'c' && T[i + 2] == 'o' && T[i + 3] == 's' || T[i] == 'a' && T[i + 1] == 't' && T[i + 2] == 'a' && T[i + 3] == 'n' || T[i] == 's' && T[i + 1] == 'q' && T[i + 2] == 'r' && T[i + 3] == 't')
                    {
                        list.Add(T[i].ToString() + T[i + 1].ToString() + T[i + 2].ToString() + T[i + 3].ToString());
                        i++; i++; i++;
                    }
                    else if (T[i] == 's' && T[i + 1] == 'i' && T[i + 2] == 'n' || T[i] == 'c' && T[i + 1] == 'o' && T[i + 2] == 's' || T[i] == 't' && T[i + 1] == 'a' && T[i + 2] == 'n' || T[i] == 'a' && T[i + 1] == 'b' && T[i + 2] == 's' || T[i] == 'e' && T[i + 1] == 'x' && T[i + 2] == 'p' || T[i] == 'l' && T[i + 1] == 'o' && T[i + 2] == 'g')
                    {
                        list.Add(T[i].ToString() + T[i + 1].ToString() + T[i + 2].ToString());
                        i++; i++;
                    }
                    if (i > 0)
                    {
                        if (T[i] == '.')
                        {
                            list.Add(list[list.Count - 1] + "," + T[i + 1].ToString());
                            list.RemoveAt(list.Count - 2);
                            i++;
                        }
                        else if (Char.IsNumber(T[i - 1]) == true && Char.IsNumber(T[i]) == true)
                        {
                            list.Add(list[list.Count - 2] + T[i].ToString());
                            list.RemoveAt(list.Count - 2);
                            list.RemoveAt(list.Count - 2);
                        }
                    }
                }
                return list;
            }//zamiana na tokeny
            public static string[] InfixToPostfix(List<string> formula)
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
            public static double CalX(List<string> f, int arg)
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
            public static void MinMax(List<string> f, double xMin, double xMax, double n)
            {
                List<int> w = new List<int>();
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
                    Console.WriteLine(xMin + " => " + PostfixCalc(Tab));
                    xMin = xMin + diff;
                }
            }//wartość formuły w n punktach w przedziale od xMin do xMax
        }
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter arguments.");
            }
            else
            {
               RPN odwrotna_notacja_polska = new RPN(args[0], Convert.ToInt32(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3]), Convert.ToDouble(args[4]));
            }
            Console.ReadKey();
        }
    }
}
