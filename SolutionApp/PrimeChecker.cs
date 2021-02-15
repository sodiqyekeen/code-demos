namespace SolutionApp
{
    public class PrimeChecker
    {
        public int PrimeSum(int start, int end)
        {
            int sum = 0;
            for (int i = start; i <= end; i++)
            {
                if (i.IsPrime())
                    sum += i;
            }
            return sum;
        }
    }
}
