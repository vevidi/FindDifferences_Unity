using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameMediator.Commands
{
    public class DiffFoundCommand : ICommand
    {
        public DifferenceInfoModel foundedDifference;
        public TouchableArea sender;

        public DiffFoundCommand(DifferenceInfoModel foundedDifference, TouchableArea sender)
        {
            this.foundedDifference = foundedDifference;
            this.sender = sender;
        }
    }
}