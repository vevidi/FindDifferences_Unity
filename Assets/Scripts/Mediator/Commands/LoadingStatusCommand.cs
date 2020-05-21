namespace Vevidi.FindDiff.GameMediator.Commands
{
    public class LoadingStatusCommand : Command
    {
        public enum eLoadingStatus
        {
            Ok,
            Error
        }

        public string Message { get; private set; }
        public eLoadingStatus Status { get; private set; }

        public LoadingStatusCommand(eLoadingStatus status, string message)
        {
            Message = message;
            Status = status;
        }
    }
}