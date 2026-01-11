using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFrameworkDotNet
{
    public sealed class Grid
    {
        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}
