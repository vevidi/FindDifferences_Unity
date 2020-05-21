namespace Vevidi.FindDiff.GameMediator.Commands
{
    public class UpdateDiffCountCommand : Command
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

    public class UpdateLivesCountCommand : Command
    {
        public int LivesCount { get; private set; }
        public int MaxLives { get; private set; }

        public UpdateLivesCountCommand(int livesCount, int maxLives)
        {
            LivesCount = livesCount;
            MaxLives = maxLives;
        }
    }
}