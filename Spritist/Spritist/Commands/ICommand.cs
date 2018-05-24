namespace Spritist.Commands
{
    public interface ICommand
    {
        void Perform();
        void Undo();
    }
}