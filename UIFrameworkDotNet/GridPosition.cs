

namespace UIFrameworkDotNet
{
    public sealed class GridPosition
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }

        public GridPosition(int rowIndex, int columnIndex, int rowSpan = 1, int colSpan = 1)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            RowSpan = rowSpan;
            ColumnSpan = colSpan;
        }
    }
}
