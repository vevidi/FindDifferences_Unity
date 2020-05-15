using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameMediator.Commands
{
    public class DiffFoundCommand : ICommand
    {
        public TouchableArea Sender { get; private set; }
        public DifferenceInfoModel FoundedDifference { get; private set; }

        public DiffFoundCommand(DifferenceInfoModel foundedDifference, TouchableArea sender)
        {
            FoundedDifference = foundedDifference;
            Sender = sender;
        }
    }
}