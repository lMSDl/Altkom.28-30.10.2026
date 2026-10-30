namespace TDD
{
    public class StringCalculator
    {
        public int Add(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;

            var numbers = Parse(input);
            Validate(numbers);


            return numbers.Sum();
        }

        private static IEnumerable<int> Parse(string input)
        {
            return input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.TryParse(x, out var number) ? number : 0);
        }

        private static void Validate(IEnumerable<int> numbers)
        {
            if (numbers.Any(x => x < 0))
                throw new ArgumentException("Negative numbers are not allowed.");
        }
    }
}
