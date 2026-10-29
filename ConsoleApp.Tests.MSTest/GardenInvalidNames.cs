namespace ConsoleApp.Tests.MSTest
{
    internal static class GardenInvalidNames
    {
        public static IEnumerable<object[]> Data =>
        [
            [ "" ],
            [ " " ],
            [ " " ], //non-breaking space
            [ "    " ],
            [ "\t" ],
            [ "  \n  \t" ]
        ];
    }
}
