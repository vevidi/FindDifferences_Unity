namespace Vevidi.FindDiff.GameMediator.Commands
{
    public class LoadingStatusCommand : ICommand
    {
        public enum eLoadingStatus
        {
            Ok,
            Error
        }

        public string message;
        public eLoadingStatus status;

        public LoadingStatusCommand(eLoadingStatus status, string message)
        {
            this.message = message;
            this.status = status;
        }
    }
}