namespace ConsoleApp
{
    public class Garden
    {
        public int Size { get; set; }
        private ICollection<string> Items { get; } = [];

        public Garden(int size)
        {
            Size = size;
        }

        public bool Plant(string item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (string.IsNullOrWhiteSpace(item))
            {
                throw new ArgumentException("Item cannot be empty or whitespace.", nameof(item));
            }

            if (Items.Count >= Size)
                return false;

            if (Items.Contains(item))
            {
                item = FixDuplicatedName(item);
            }

            Items.Add(item);
            return true;
        }

        //nie testuje się metod prywatnych bezpośrednio
        //są one testowane pośrednio poprzez metody publiczne, które je wywołują
        private string FixDuplicatedName(string item)
        {
            item += Items.Count(x => x.StartsWith(item)) + 1;
            return item;
        }

        internal IEnumerable<string> GetItems()
        {
            return Items.ToList();
        }

    }
}
