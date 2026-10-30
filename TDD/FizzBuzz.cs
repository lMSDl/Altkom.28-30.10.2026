namespace TDD
{
    public class FizzBuzz
    {
        public static IEnumerable<string> GetSequence(int count)
        {
            return Enumerable.Range(1, count).Select(GetFizzBuzz);
        }

        public static string GetFizzBuzz(int number)
        {
            string result = string.Empty;
            if (IsFizz(number))
                result += "Fizz";
            if (IsBuzz(number))
                result += "Buzz";

            return string.IsNullOrEmpty(result) ? number.ToString() : result;
        }

        private static bool IsFizz(int number)
        {
            return number % 3 == 0;
        }

        private static bool IsBuzz(int number)
        {
            return number % 5 == 0;
        }
    }
}
