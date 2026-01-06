
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace UIFramework
{
    // TODO c'è del lavoro da fare anche a livello ESLX non solo JS (cambiamenti ma non ci sono app che la usano mi sa).
    public class UISequenceStep : UIElement
    {
        public UISequenceStep(string tag, string text)
        {
            Props["tag"] = tag;
            States["text"] = text;
            //  States["subtitle"]
            States["status"] = "inactive";
        }

        [JsonIgnore]
        public string Text
        {
            get => States.ContainsKey("text") ? States["text"].ToString() : "";
            set { States["text"] = value; OnPropertyChanged(nameof(Text)); }
        }

        [JsonIgnore]
        public string Status
        {
            get => States.ContainsKey("status") ? States["status"].ToString() : "";
            set { States["status"] = value; OnPropertyChanged(nameof(Status)); }
            }
    }

    public class UISequence : ContainerElement
    {
        [JsonIgnore]
        public List<UISequenceStep> Steps => Children.OfType<UISequenceStep>().ToList();
        public UISequence()
        {
            States["currentStep"] = null;
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

        [JsonIgnore]
        public UISequenceStep CurrentStep
        {
            get 
            {
                States.TryGetValue("currentStep", out var v);
                if (v is UISequenceStep step)
                {
                    return step;
                }
                return null;
            }
            set { States["currentStep"] = value; OnPropertyChanged(nameof(CurrentStep)); }
        }
    }
}
