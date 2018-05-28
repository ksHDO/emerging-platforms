namespace Spritist.Commands
{
    public interface ICommand
    {
        void Redo();
        void Undo();
    }
}