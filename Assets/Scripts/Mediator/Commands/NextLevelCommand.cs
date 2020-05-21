namespace Vevidi.FindDiff.GameMediator.Commands
{
    // later some data will be added here
    public class NextLevelCommand : Command
    {
        private readonly int levelID;

        public int LevelID => levelID;

        public NextLevelCommand(int levelID) => this.levelID = levelID;
    }
}
