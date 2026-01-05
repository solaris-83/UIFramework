
namespace UIFramework.Helpers
{
    public class DataArray : List<object>
    {
        public bool ContainsAny(params object[] items)
        {
            foreach (object item in items)
            {
                if (base.Contains(item)) return true;
            }
            return false;
        }
        public bool ContainsAll(params object[] items)
        {
            foreach (object item in items)
            {
                if (!base.Contains(items)) return false;
            }
            return true;
        }

        public DataArray(params object[] items) : base()
        {
            this.AddRange(items);
        }
    }
}
