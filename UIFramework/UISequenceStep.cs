using Newtonsoft.Json;
using System.Diagnostics;

namespace UIFramework
{
    // TODO c'è del lavoro da fare anche a livello ESLX non solo JS (cambiamenti ma non ci sono app che la usano mi sa).
    // TODO gestire gli Equals
    public class UISequenceStep : UIElement
    {
        public UISequenceStep(string tag, string text)
        {
            Tag= tag;
            Text = text;
            //  States["subtitle"]
            Status = "inactive";
        }

        private string _text;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            set 
            { 
                if (_text == value) return;
                _text = value;
                Props["text"] = value; 
                OnPropertyChanged(nameof(Text)); 
            }
        }

        private string _status;
        [JsonIgnore]
        public string Status
        {
            get => _status;
            set 
            { 
                if (_status == value) return;
                _status = value;
                States["status"] = value; 
                OnPropertyChanged(nameof(Status)); 
            }
        }

        public override bool Equals(object? obj)
        {
            Debug.Assert(obj != null);
            return obj is UISequenceStep step &&
                   EqualityComparer<object>.Default.Equals(Tag, step.Tag) &&
                   Text == step.Text &&
                   Status == step.Status;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tag, Text, Status);
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

        private UISequenceStep _currentStep;
        [JsonIgnore]
        public UISequenceStep CurrentStep
        {
            get 
            {
                return _currentStep;
            }
            set 
            { 
                if (_currentStep.Equals(value)) return;
                _currentStep = value;
                States["currentStep"] = value; 
                OnPropertyChanged(nameof(CurrentStep)); 
            }
        }
    }
}
