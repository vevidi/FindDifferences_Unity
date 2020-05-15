namespace Vevidi.FindDiff.GameMediator.Commands
{
    public class UpdateLevelUiCommand : ICommand
    {
        public int diffFoundValue;

        public UpdateLevelUiCommand(int value)
        {
            diffFoundValue = value;
        }
    }
}