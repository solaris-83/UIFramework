using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UIFrameworkDotNet
{
    // TODO c'è del lavoro da fare anche a livello ESLX non solo JS (cambiamenti ma non ci sono app che la usano mi sa).
    // TODO gestire gli Equals
    public class UISequenceStep : UITextElement
    {
        public UISequenceStep(string tag, string text) : base(text)
        {
            Tag = tag;
            //  States["subtitle"]
            Status = "inactive";
        }

        #region Props
       
        #endregion

        #region States
        private string _status;
        [JsonIgnore]
        public string Status
        {
            get => _status;
            set
            {
                SetStatesProperty(ref _status, value, nameof(Status));
            }
        }

        #endregion

        public override bool Equals(object obj)
        {
            Debug.Assert(obj != null);
            return obj is UISequenceStep step &&
                   EqualityComparer<object>.Default.Equals(Tag, step.Tag) &&
                   Text == step.Text &&
                   Status == step.Status;
        }

        // Pseudocode / Plan:
        // 1. Compute hash based on the same fields used in Equals: Tag, Text, Status.
        // 2. Handle nulls by using 0 when a field is null.
        // 3. Use an unchecked block and prime multipliers to reduce collisions.
        // 4. Return the combined hash as an int.
        //
        // Implementation follows the above plan.

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Tag != null ? Tag.GetHashCode() : 0);
                hash = hash * 23 + (Text != null ? Text.GetHashCode() : 0);
                hash = hash * 23 + (Status != null ? Status.GetHashCode() : 0);
                return hash;
            }
        }
    }

    public class UISequence : ContainerElement
    {
        [JsonIgnore]
        public List<UISequenceStep> Steps => Children.OfType<UISequenceStep>().ToList();
        public UISequence()
        {
            CurrentStep = null;
        }

        public UISequenceStep AddStep(string tag, string text)
        {
            var step = new UISequenceStep(tag, text);
            Add(step);
            return step;
        }

        public bool UpdateStatusStep(string stepId, string status)
        {
            var foundStep = Steps.FirstOrDefault(s => s.Id == stepId);
            if (foundStep == null)
                return false;
            foundStep.Status = status;
            return true;
        }

        // Setto lo stato dello step X come attivo. Suppongo che i precedenti siano in stato completed.
        public bool SetActiveStep(string stepId)
        {
            var foundIndex = Steps.FindIndex(s => s.Id == stepId);
            if (foundIndex < 0)
                return false;

            // Mark previous steps as completed if they were active
            for (int i = 0; i < foundIndex; i++)
            {
                var prev = Steps[i];
                if (prev != null && prev.Status == "active")
                    prev.Status = "completed";
            }

            // Set the found step as active
            var foundStep = Steps[foundIndex];
            if (foundStep == null)
                return false;
            else
                foundStep.Status = "active";

            // Update current step reference
            CurrentStep = foundStep;

            return true;
        }

        #region States
        private UISequenceStep _currentStep;
        [JsonIgnore]
        public UISequenceStep CurrentStep
        {
            get => _currentStep;
            set
            {
                SetStatesProperty(ref _currentStep, value, nameof(CurrentStep));
            }
        }

        #endregion
    }
}
