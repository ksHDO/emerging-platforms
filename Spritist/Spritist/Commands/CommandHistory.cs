using System.Collections.Generic;

namespace Spritist.Commands
{
    public class CommandHistory
    {
        private readonly Stack<ICommand> undoHistory;
        private readonly Stack<ICommand> redoHistory;

        public CommandHistory()
        {
            undoHistory = new Stack<ICommand>();
            redoHistory = new Stack<ICommand>();
        }

        public void Perform(ICommand command)
        {
            redoHistory.Clear();
            command.Perform();
            undoHistory.Push(command);
        }

        public void Undo()
        {
            if (undoHistory.Count > 0)
            {
                ICommand command = undoHistory.Pop();
                command.Perform();
                redoHistory.Push(command);
            }
        }

        public void Redo()
        {
            if (redoHistory.Count > 0)
            {
                ICommand command = redoHistory.Pop();
                command.Perform();
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