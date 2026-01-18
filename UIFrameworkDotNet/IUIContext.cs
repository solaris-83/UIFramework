using ScriptLibraries.Data.Interfaces;

namespace UIFrameworkDotNet
{
    public interface IUIContext
    {
        ITranslationService Translator { get; }
    }
}
