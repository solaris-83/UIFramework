

namespace UIFrameworkDotNet
{
    public sealed class GridPosition
    {
        public int Row { get; }
        public int Column { get; }
        public int RowSpan { get; }
        public int ColSpan { get; }

        public GridPosition(int row, int column, int rowSpan = 1, int colSpan = 1)
        {
            Row = row;
            Column = column;
            RowSpan = rowSpan;
            ColSpan = colSpan;
        }
    }

}
