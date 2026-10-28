namespace ConsoleApp.Tests.xUnit
{
    internal class GardenInvalidNames : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { " " }; //non-breaking space
            yield return new object[] { "    " };
            yield return new object[] { "\t" };
            yield return new object[] { "  \n  \t" };
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
