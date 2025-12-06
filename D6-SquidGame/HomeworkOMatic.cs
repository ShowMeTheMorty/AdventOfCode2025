using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;

public class HomeworkOMatic
{
    public enum Operator
    {
        Multiplication,
        Addition
    }

    public struct Expression(
        IEnumerable<long> terms,
        Operator operation
    )
    {
        public long Solve()
        {
            IEnumerator<long> enumerator = terms.GetEnumerator();
            enumerator.MoveNext();
            long result = enumerator.Current;

            while (enumerator.MoveNext())
            {
                result = operation == Operator.Multiplication ?
                    result * enumerator.Current : result + enumerator.Current;
            }

            return result;
        }
    }

    readonly IEnumerable<Expression> expressions;
    
    void DecodeCephalapodNumbers (ref string[] terms)
    {
        int size = terms.Select(str => str.Length).Max();
        List<string> newTerms = [];
        for (int col = size - 1; col >= 0; col--)
        {
            string termString = string.Empty;
            for (int row = 0; row < terms.Length; row++)
            {
                char charmander = terms[row][col];
                
                if (charmander != ' ') termString += charmander;
                if (row == terms.Length-1 && termString.Length > 0) newTerms.Add(termString);
            }
        }

        terms = newTerms.ToArray();
    }

    IEnumerable<Expression> GenerateExpressions (string[] termsData, int problemCount, int linesCount, bool cephalaopdSyntax)
    {
        for (int nProblem = 0; nProblem<problemCount; nProblem++)
        {
            string[] terms = new string[linesCount - 1];
            Operator operation = Operator.Multiplication;

            for (int nLineTerm = 0, lineNo = 0; nLineTerm < termsData.Length; nLineTerm += problemCount, lineNo++)
            {
                string termData = termsData[nLineTerm + nProblem];

                if (lineNo == linesCount - 1) operation = termData[0] == '*' ? Operator.Multiplication : Operator.Addition;
                else terms[lineNo] = termData;
            }

            if (cephalaopdSyntax) DecodeCephalapodNumbers(ref terms);

            yield return new Expression(terms.Select(long.Parse), operation);
        }
    }

    public HomeworkOMatic(string data, bool cephalapodSyntax=false)
    {
        string[] lines = data.Split('\n')
            // .Select(str => str.Trim())
            .Where(str => !string.IsNullOrWhiteSpace(str))
            .ToArray();

        List<(int, int)> columns = [];
        int currentLength = 0;
        for (int c = 0; c < lines[0].Length; c++)
        {
            bool isBreak = true;
            for (int r = 0; r < lines.Length && isBreak; r++)
            {
                if (lines[r][c] != ' ') isBreak = false;
            }

            if (isBreak)
            {
                if (currentLength > 0)
                    columns.Add((c - currentLength, currentLength));

                currentLength = 0;
            }
            else
            {
                currentLength++;
                if (c == lines[0].Length - 1) 
                {
                    columns.Add((c + 1 - currentLength, currentLength));
                    currentLength = 0;
                }
            }
        }
        
        List<string> termsData = [];
        foreach (string line in lines)
        {
            foreach (var column in columns)
            {
                termsData.Add(line[column.Item1..(column.Item1 + column.Item2)]);
            }
        }

        expressions = GenerateExpressions(termsData.ToArray(), columns.Count, lines.Length, cephalapodSyntax);
        Console.WriteLine();
    }

    public static HomeworkOMatic FromFile(string path, bool cephalaopdSyntax=false) =>
        new HomeworkOMatic(File.ReadAllText(path), cephalaopdSyntax);
    
    public long SolveAndSumAllExpressions ()
    {
        return expressions.Select(exp => exp.Solve()).Sum();
    }
}