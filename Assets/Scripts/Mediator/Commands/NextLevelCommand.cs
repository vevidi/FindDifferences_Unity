namespace Vevidi.FindDiff.GameMediator.Commands
{
    // later some data will be added here
    public class NextLevelCommand : ICommand
    {
        private readonly int levelID;

        public int LevelID => levelID;

        //public NextLevelCommand()
        //{
        //    this.levelID = 0;
        //}

        public NextLevelCommand(int levelID)
        {
            this.levelID = levelID;
        }
    }
}
