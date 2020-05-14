namespace Vevidi.FindDiff.GameMediator
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