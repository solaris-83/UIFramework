using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Interfaces.Adapters;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UITable : UIElement, ITableAdapter
    {
        private bool _isSelectionEnabled;
        [JsonIgnore]
        public bool IsSelectionEnabled
        {
            get => _isSelectionEnabled;
            set => SetStatesProperty(ref _isSelectionEnabled, value, nameof(IsSelectionEnabled));
        }

        public UITable()
        {
            Columns = new List<UITableColumn>();
            Rows = new List<UITableRow>();
            ShowHeader = true;
            IsSelectionEnabled = true;
        }

        private bool _showHeader;
        [JsonIgnore]
        public bool ShowHeader
        {
            get => _showHeader;
            set => SetPropsProperty(ref _showHeader, value, nameof(ShowHeader));
        }

        private UITableRow _selectedRow;
        [JsonIgnore]
        public UITableRow SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (IsSelectionEnabled)
                {
                    SetStatesProperty(ref _selectedRow, value, nameof(SelectedRow));
                }
            }
        }

        private List<UITableRow> _rows;
        [JsonIgnore]
        public List<UITableRow> Rows
        {
            get => _rows;
            set => SetPropsProperty(ref _rows, value, nameof(Rows));
        }

        private List<UITableColumn> _columns;
        [JsonIgnore]
        public List<UITableColumn> Columns
        {
            get => _columns;
            set => SetPropsProperty(ref _columns, value, nameof(Columns));
        }

        public bool RenameColumns(params string[] columns)
        {
            if (columns == null || columns.Length == 0 || columns.Length % 2 != 0)
                throw new InvalidOperationException("Invalid number of parameters. Provide pairs of column key and new header.");

            var newColumns = Columns.ToList();
            for (int i = 0; i < columns.Length; i += 2)
            {
                var foundColumns = newColumns.Find(c => c.Key == columns[i]);
                if (foundColumns != null)
                    foundColumns.Header = columns[i + 1];
            }
            Columns = newColumns;
            return true;
        }

        public bool LoadData(IDataFrame df)
        {
            if (df == null || df.Columns == null || df.Rows == null)
                return false;

            // Map each column name to a UITableColumn (Key = column name, Header = column name)
            Columns = df.Columns.Select(col => new UITableColumn(col, col)).ToList();

            // Map each row: Cells dictionary where key = column name, value = value at that column
            var rows = new List<UITableRow>();
            foreach (var row in df.Rows)
            {
                var cells = new Dictionary<string, object>();
                for (int i = 0; i < df.Columns.Count; i++)
                {
                    var colName = df.Columns[i];
                    object value = (row.Values != null && i < row.Values.Count) ? row.Values[i] : null;
                    cells[colName] = value;
                }
                rows.Add(new UITableRow(cells));
            }
            Rows = rows;
            return true;
        }
    }

    public class UITableRow
    {
        public UITableRow(Dictionary<string, object> cells)
        {
            Cells = cells;
        }

        public string Id => Guid.NewGuid().ToString();
        public IReadOnlyDictionary<string, object> Cells { get; private set; }

        public object GetValue(string columnKey)
        {
            return Cells.TryGetValue(columnKey, out var value) ? value : null;
        }
    }

    public class UITableColumn
    {
        public UITableColumn(string key, string header)
        {
            Key = key;
            Header = header;
        }

        public string Key { get; set; }
        public string Header { get; set; }
    }
}
