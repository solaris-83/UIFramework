using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;
using System;
using UIFramework.Helpers;
using UIFramework.Interfaces;
using UIFramework.Reactive;
using UIFramework.UIElements;

namespace UIFramework.SpecializedPages
{
    public sealed class PageMenu : SpecializedPage
    {
        private readonly UIChoiceGroup _choicegroup;
        private UIButton _buttonContinue;
        private CompositeCondition _compositeCondition;

        [JsonIgnore]
        public bool IsMultipleSelection
        {
            get => _choicegroup.IsMultipleSelection;
            set => _choicegroup.IsMultipleSelection = value;
        }

        [JsonIgnore]
        public int SelectedIndex => 
             _choicegroup.SelectedIndex;


        [JsonIgnore]
        public string SelectedId => _choicegroup.SelectedId;

        [JsonIgnore]
        public IDataArray SelectedIndexes => _choicegroup.SelectedIndexes;
        

        [JsonIgnore]
        public IDataArray SelectedIds => _choicegroup.SelectedIds;


        public PageMenu(IUIContext UIContext) : base("menu", UIContext)
        {
            AddButton("EXIT", true, ElementStatusEnum.Danger.GetDescription());
            _buttonContinue = AddButton("CONTINUE", false);
            _choicegroup = new UIChoiceGroup(UIContext);
            _choicegroup.SetAppearance("checkbox");
        }

        public UIChoice AddItem(string tag, string idStr)
        {
            if (SingleSection != null)
            {
                bool checkBoxGroupAdded = SingleSection.FindById(_choicegroup.Id) != null;
                if (!checkBoxGroupAdded)
                {
                    SingleSection.Add(_choicegroup);
                }

                var item =  _choicegroup.AddItem(tag, idStr);

                if (_compositeCondition == null)
                    _compositeCondition = new OrCondition();

                _compositeCondition.AddCondition(new EqualsCondition(item, nameof(UIChoice.Checked), true));

                var trueReaction = new Reaction(_buttonContinue, nameof(UIButton.Enabled), true);
                var falseReaction = new Reaction(_buttonContinue, nameof(UIButton.Enabled), false);
                CreateBinding(_compositeCondition, new CompositeReaction(trueReaction, falseReaction));

                return item;
            }
            return null;
        }

        public bool SetMessage(params string[] ids)
        {
            foreach (string idStr in ids)
            {
                AddParagraph(idStr);
            }

            return true;
        }
    }
}
