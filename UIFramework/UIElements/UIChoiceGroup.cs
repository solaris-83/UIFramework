using BaseCustomApp.Helpers;
using log4net;
using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIChoiceGroup : ContainerElement
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UIChoiceGroup));
        public UIChoiceGroup(IUIContext context) : base(context)
        {
            IsMultipleSelection = false;
        }

        #region ReadOnly properties
        [JsonIgnore]
        public IEnumerable<UIChoice> Choices => Children.OfType<UIChoice>();

        [JsonIgnore]
        public IEnumerable<UIChoice> SelectedChoices
            => Choices.Where(c => c.Checked);

        [JsonIgnore]
        public UIChoice SelectedChoice
            => AnySelected ? SelectedChoices.Single() : null;

        [JsonIgnore]
        public IDataArray SelectedIndexes
        {
            get
            {
                if (!IsMultipleSelection)
                    throw new InvalidOperationException("You cannot retrieve SelectedIndexes if IsMultipleSection is set to FALSE. Use SelectedIndex instead.");
                IDataArray da = new DataArray();
                foreach (var idx in Choices.Select((item, index) => new { item, index })
                             .Where(x => x.item.Checked)
                             .Select(x => x.index))
                    da.Add(idx);
                return da;
            }
        }

        [JsonIgnore]
        public IDataArray SelectedIds
        {
            get
            {
                if (!IsMultipleSelection)
                    throw new InvalidOperationException("You cannot retrieve SelectedIds if IsMultipleSection is set to FALSE. Use SelectedId instead.");
                IDataArray da = new DataArray();
                foreach (var chb in SelectedChoices)
                    da.Add(chb.Tag);
                return da;
            }
        }

        [JsonIgnore]
        public int SelectedIndex => !IsMultipleSelection ?
             (SelectedChoice == null ? -1 : Choices.ToList().IndexOf(SelectedChoice)) :
             throw new InvalidOperationException("You cannot retrieve SelectedIndex if IsMultipleSection is set to TRUE. Use SelectedIndexes instead.");



        [JsonIgnore]
        public string SelectedId => !IsMultipleSelection ?
             (SelectedChoice == null ? "" : SelectedChoice.Tag.ToString()) :
             throw new InvalidOperationException("You cannot retrieve SelectedId if IsMultipleSection is set to TRUE. Use SelectedIds instead.");

        [JsonIgnore]
        public bool AnySelected => SelectedChoices.Any();

        #endregion

        #region Events 

        private EventHandler _selectionChanged;
        public event EventHandler SelectionChanged
        {
            add
            {
                if (_selectionChanged != null)
                {
                    throw new InvalidOperationException("SelectionChanged event already has a subscriber.");
                }

                _selectionChanged += value;
            }
            remove
            {
                _selectionChanged -= value;
            }
        }

        #endregion

        #region Props

        private bool _isMultipleSelection;
        [JsonIgnore]
        public bool IsMultipleSelection
        {
            get => _isMultipleSelection;
            set
            {
                if (value == true && Style.Appearance == ElementStatusEnum.RadioButton.GetDescription())
                {
                    throw new Exception("You cannot use MultipleSelection in a radiobutton.");
                }
                SetPropsProperty(ref _isMultipleSelection, value, nameof(IsMultipleSelection));
            }
        }

        #endregion

        #region Public methods
        public override bool SetAppearance(string appearance)
        {
            base.SetAppearance(appearance);
            if (IsMultipleSelection && Style.Appearance == ElementStatusEnum.RadioButton.GetDescription())
            {
                throw new Exception("You cannot use MultipleSelection in a radiobutton.");
            }
            return true;
        }

        public UIChoice AddItem(string tag, string idStr)
        {
            return AddItem(tag, idStr, false);
        }

        public UIChoice AddItem(string tag, string idStr, bool isChecked)
        {
            var chkbox = new UIChoice(idStr, isChecked: false, isEnabled: true, tag);
            // Subscribing to checkbox SelectionChanged event to evaluate the state of CONTINUE button and to manage single/multiple selection logic
            chkbox.SelectionChanged += Chkbox_SelectionChanged;
            if (chkbox.Checked != isChecked)
                chkbox.Checked = isChecked;
            // If radiobutton we want to have the first item added selected by default to avoid having a radiogroup with no selection which is not valid from a UI/UX perspective
            if (Style.Appearance == ElementStatusEnum.RadioButton.GetDescription() && !AnySelected)
            {
                chkbox.Checked = true;
                _logger.Warn("First item added to a radiobutton is automatically selected to ensure a valid state. Item: " + idStr);
            }
            Add(chkbox);
            return chkbox;
        }

        public override void Dispose()
        {
            // Unsubscribe from SelectionChanged event for all checkboxes to prevent memory leaks
            foreach (var chb in Choices)
            {
                chb.SelectionChanged -= Chkbox_SelectionChanged;
            }
            base.Dispose();
        }

        #endregion

        #region Private methods

        private void Chkbox_SelectionChanged(object sender, EventArgs e)
        {
            if (sender is UIChoice checkbox)
            {
                foreach (var chb in Choices)
                {
                    if (chb.Id == checkbox.Id)
                        continue;
                    if (IsMultipleSelection == false)
                    {
                        chb.SelectionChanged -= Chkbox_SelectionChanged;
                        chb.Checked = false;
                        chb.SelectionChanged += Chkbox_SelectionChanged;
                    }
                }

                OnPropertyChanged(Id, nameof(AnySelected), AnySelected);
                if (IsMultipleSelection == false)
                {
                    OnPropertyChanged(Id, nameof(SelectedId), SelectedId);
                    OnPropertyChanged(Id, nameof(SelectedIndex), SelectedIndex);
                }

                _selectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
