namespace Vevidi.FindDiff.GameMediator.Commands
{
    public class UpdateDiffCountCommand : ICommand
    {
        public int DiffFoundValue { get; private set; }
        public int MaxValue { get; private set; }
        public int LiveCount { get; private set; }
    
        public UpdateDiffCountCommand(int diffFound, int maxDiff)
        {
            DiffFoundValue = diffFound;
            MaxValue = maxDiff;
        }
    }

    public class UpdateLivesCountCommand : ICommand
    {
        public int LivesCount { get; private set; }

        public UpdateLivesCountCommand(int livesCount)
        {
            LivesCount = livesCount;
        }
    }
}