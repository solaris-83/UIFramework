using BaseCustomApp.Helpers;
using log4net;
using ScriptLibraries.Data.Interfaces;
using System;
using UIFramework.Interfaces;
using UIFramework.UIElements;

namespace UIFramework.SpecializedPopups
{
    public sealed class UISimpleModalPopup : UIPopup
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UIFeedbackCountdown));

        public UISimpleModalPopup(string commands, string header, IUIContext context, params string[] messageIds) : base(isModal: true, context)
        {
            //_context = context;
            switch (commands.ToUpperInvariant())
            {
                case "OK":
                    AddButton("OK", "TRANSL_MODAL_OK", true);
                    break;
                case "OKCANCEL":
                    AddButton("CANCEL", "TRANSL_MODAL_CANCEL", ElementStatusEnum.LinkLike.GetDescription(), true);
                    AddButton("OK", "TRANSL_MODAL_OK", true);
                    break;
                case "YESNO":
                    AddButton("NO", "TRANSL_MODAL_NO", ElementStatusEnum.Danger.GetDescription(), true);
                    AddButton("YES", "TRANSL_MODAL_YES", true);
                    break;
                case "RETRYCANCEL":
                    AddButton("CANCEL", "TRANSL_MODAL_CANCEL", ElementStatusEnum.LinkLike.GetDescription(), true);
                    AddButton("RETRY", "TRANSL_MODAL_RETRY", true);
                    break;
                default:
                    throw new NotImplementedException($"{commands} not expected.");
            }

            Title = header;
            var section = new UISection(Context);
            foreach (var msg in messageIds)
            {
                section.AddParagraph(msg);
            }
            Add(section);
        }

        internal override void OnButtonClicked(object sender, System.EventArgs e)
        {
            base.OnButtonClicked(sender, e);
            Context.MessageBoxResponseEvent.Set();
            _logger.Debug($"Unblocked WaitForUserEvent in order ESLX to proceed");
        }
    }
}
