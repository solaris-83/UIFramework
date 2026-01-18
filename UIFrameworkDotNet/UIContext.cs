using ScriptLibraries.Data.Interfaces;

namespace UIFrameworkDotNet
{
    public sealed class UIContext : IUIContext
    {
        public ITranslationService Translator { get; }

        public UIContext(ITranslationService translator)
        {
            Translator = translator;
        }
    }
}
