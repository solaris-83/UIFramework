using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UIFramework.Interfaces;
using UIFramework.Interfaces.Adapters;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UISequence : ContainerElement, ISequenceAdapter
    {
        [JsonIgnore]
        public List<UISequenceStep> Steps => Children.OfType<UISequenceStep>().ToList();

        [JsonIgnore]
        public UISequenceStep CurrentStep => Steps.SingleOrDefault(step => step.Status == ElementStatusEnum.Active.GetDescription());

        public UISequence(IUIContext context) : base(context) { }


        public UISequenceStep AddStep(string text)
        {
            var step = new UISequenceStep(text);
            Add(step);
            return step;
        }

        public bool UpdateStep(string stepId, string status)
        {
            var foundStep = Steps.FirstOrDefault(s => s.Id == stepId);
            if (foundStep == null)
                return false;
            foundStep.Status = status;
            return true;
        }

        public bool ResetSteps()
        {
            foreach (var step in Steps)
            {
                step.Status = ElementStatusEnum.Normal.GetDescription();
            }
            return true;
        }
    }
}
