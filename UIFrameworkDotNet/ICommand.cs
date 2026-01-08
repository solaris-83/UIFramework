
namespace UIFrameworkDotNet
{
    public interface ICommandOld
    {
        void Execute();
    }

    public interface ICommand
    {
        void Execute(object newValue);
    }
}
