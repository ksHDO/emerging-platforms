using System.Collections.Generic;

namespace Spritist.Commands
{
    public class CommandHistory
    {
        private readonly Stack<ICommand> undoHistory;
        private readonly Stack<ICommand> redoHistory;

        public bool CanUndo() => undoHistory.Count > 0;
        public bool CanRedo() => redoHistory.Count > 0;

        public CommandHistory()
        {
            undoHistory = new Stack<ICommand>();
            redoHistory = new Stack<ICommand>();
        }

        public void Perform(ICommand command)
        {
            redoHistory.Clear();
            //command.Perform();
            undoHistory.Push(command);
        }

        public void Undo()
        {
            if (undoHistory.Count > 0)
            {
                ICommand command = undoHistory.Pop();
                command.Undo();
                redoHistory.Push(command);
            }
        }

        public void Redo()
        {
            if (redoHistory.Count > 0)
            {
                ICommand command = redoHistory.Pop();
                command.Redo();
                undoHistory.Push(command);
            }
        }

        public void Reset()
        {
            undoHistory.Clear();
            redoHistory.Clear();
        }
    }
}