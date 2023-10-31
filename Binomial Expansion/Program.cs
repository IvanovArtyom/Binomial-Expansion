using System;
using System.Text;
using System.Linq;
using System.Numerics;

public class KataSolution
{
    public static void Main()
    {
        // Test
        var t = Expand("(x+1)^2");
        // ...should return "x^2+2x+1"
    }

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