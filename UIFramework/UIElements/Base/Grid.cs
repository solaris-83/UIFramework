using System;

namespace UIFramework.UIElements.Base
{
    public sealed class Grid
    {
        public Grid()
        {
            
        }
        public Grid(int rows, int columns)
        {
            if (rows <= 0)
                throw new ArgumentOutOfRangeException(nameof(rows));
            if (columns <= 0)
                throw new ArgumentOutOfRangeException(nameof(columns));
            Rows = rows;
            Columns = columns;
        }

        public event EventHandler GridChanged;

        private int _rows;
        public int Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                GridChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private int _columns;
        public int Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                GridChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
