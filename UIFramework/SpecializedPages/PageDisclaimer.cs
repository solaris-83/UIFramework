using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using ScriptLibraries.Data.Interfaces;
using System.Diagnostics;
using UIFramework.Interfaces;
using UIFramework.Interfaces.Adapters;
using UIFramework.Reactive;
using UIFramework.UIElements;

namespace UIFramework.SpecializedPages
{
    public sealed class PageDisclaimer : SpecializedPage, IPageAdapter
    {
        private UIButton _buttonContinue;

        private bool _requiresCompleteRead;
        [JsonIgnore]
        public bool RequiresCompleteRead
        {
            get => _requiresCompleteRead;
            set
            {
                SetProperty(ref _requiresCompleteRead, value, () =>
                {
                    _buttonContinue.Enabled = !_requiresCompleteRead;
                    Props["requiresCompleteRead"] = _requiresCompleteRead;
                    if (RequiresCompleteRead)
                    {
                        // Setto le condizioni di binding: se RequiresCompleteRead è abilitato, allora creo una condizione che lega l'evento di scroll alla fine della section al bottone continue, in modo che quando si arriva a fine scroll il bottone si abiliti
                        // Non vanno gestiti altre variazioni di state perché se RequiresCompleteRead è false il bottone è sempre abilitato, se è true il bottone è abilitato solo quando si arriva a fine scroll, e non può tornare a false (il check è one-shot)
                        var condition = new EqualsCondition(SingleSection, nameof(UISection.IsScrolledToEnd), true);
                        var okReaction = new Reaction(_buttonContinue, nameof(UIButton.Enabled), true);
                        var koReaction = new Reaction(_buttonContinue, nameof(UIButton.Enabled), false);
                        CompositeReaction reaction = new CompositeReaction(okReaction, koReaction);
                        CreateBinding(condition, reaction);
                    }
                },
                nameof(RequiresCompleteRead));
            }
        }

        public PageDisclaimer(IUIContext uicontext) : base("disclaimer", uicontext)
        {
            SetTitle("title", "BCA_INFORMATION", ElementStatusEnum.Info.GetDescription());
            AddButton("EXIT_WITHOUT_REPORT", true, ElementStatusEnum.Danger.GetDescription());
            _buttonContinue = AddButton("CONTINUE", true);
            RequiresCompleteRead = false;
            //Set eventi custom per la section del disclaimer: voglio che quando si arriva a fine scroll si abiliti il bottone CONTINUE
            Debug.Assert(SingleSection != null, "Section instance cannot be null by design.");
        }
    }
}
