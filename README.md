## Description:

The purpose of this kata is to write a program that can do some algebra.

Write a function ```expand``` that takes in an expression with a single, one character variable, and expands it. The expression is in the form ```(ax+b)^n``` where ```a``` and ```b``` are integers which may be positive or negative, ```x``` is any single character variable, and ```n``` is a natural number. If a = 1, no coefficient will be placed in front of the variable. If a = -1, a "-" will be placed in front of the variable.

The expanded form should be returned as a string in the form ```ax^b+cx^d+ex^f...``` where ```a```, ```c```, and ```e``` are the coefficients of the term, ```x``` is the original one character variable that was passed in the original expression and ```b```, ```d```, and ```f```, are the powers that ```x``` is being raised to in each term and are in decreasing order.

If the coefficient of a term is zero, the term should not be included. If the coefficient of a term is one, the coefficient should not be included. If the coefficient of a term is -1, only the "-" should be included. If the power of the term is 0, only the coefficient should be included. If the power of the term is 1, the caret and power should be excluded.

### Examples:
```C#
KataSolution.Expand("(x+1)^2");      // returns "x^2+2x+1"
KataSolution.Expand("(p-1)^3");      // returns "p^3-3p^2+3p-1"
KataSolution.Expand("(2f+4)^6");     // returns "64f^6+768f^5+3840f^4+10240f^3+15360f^2+12288f+4096"
KataSolution.Expand("(-2a-4)^0");    // returns "1"
KataSolution.Expand("(-12t+43)^2");  // returns "144t^2-1032t+1849"
KataSolution.Expand("(r+0)^203");    // returns "r^203"
KataSolution.Expand("(-x-1)^2");     // returns "x^2+2x+1"
```

### My solution
```C#
using System;
using System.Text;
using System.Linq;
using System.Numerics;

public class KataSolution
{
    public static BigInteger[] factorials = Array.Empty<BigInteger>();

    public static string Expand(string expr)
    {
        string aAsString = string.Concat(expr.Skip(1).TakeWhile(c => !char.IsLetter(c)));  

        int a = aAsString.Length == 0 ? 1 : aAsString == "-" ? -1 : int.Parse(aAsString);
        char x = expr.First(char.IsLetter);
        int b = int.Parse(expr[(expr.IndexOf(x) + 1)..expr.IndexOf(')')]);
        int n = int.Parse(expr[(expr.IndexOf('^') + 1)..]);

        if (n == 0)
            return "1";

        factorials = CreateFactorialTable(n);
        BigInteger[] odds = new BigInteger[n + 1];

        for (int k = 0; k <= n; k++)
            odds[n - k] += C(n, k) * (BigInteger)Math.Pow(a, n - k) * (BigInteger)Math.Pow(b, k);

        StringBuilder result = new();

        for (int i = n; i > 0; i--)
        {
            string odd = $"{odds[i]}";

            if (odds[i] == 0)
                continue;

            else if (odds[i] == 1)
                odd = "";

            else if (odds[i] == -1)
                odd = "-";

            if (i != n && odds[i] > 0)
                odd = "+" + odd;

            if (i == 1)
                result.Append($"{odd}{x}");

            else result.Append($"{odd}{x}^{i}");
        }

        result.Append(odds[0] == 0 ? "" : odds[0] > 0 ? $"+{odds[0]}" : odds[0]);

        return result.ToString();
    }

    public static BigInteger[] CreateFactorialTable(int n)
    {
        BigInteger[] factorialTable = new BigInteger[n + 1];
        factorialTable[0] = 1;

        for (int i = 1; i <= n; i++)
            factorialTable[i] = factorialTable[i - 1] * i;

        return factorialTable;
    }

    public static BigInteger C(int n, int k)
    {
        return factorials[n] / (factorials[k] * factorials[n - k]);
    }
}
```
