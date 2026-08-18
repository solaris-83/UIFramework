using ScriptLibraries.Data.Interfaces;
using System.Threading;

namespace UIFramework.Interfaces
{
    public interface IUIContext
    {
        ITranslationService Translator { get; }
        IGlobalVariables GlobalVariables { get; }
        IFileService FileService { get; set; }
        ManualResetEvent WaitForUserEvent { get; }
        ManualResetEvent MessageBoxResponseEvent { get; }
    }
}
