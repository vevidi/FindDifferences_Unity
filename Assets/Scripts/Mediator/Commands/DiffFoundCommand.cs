
using Vevidi.FindDiff.NetworkModel;

public class DiffFoundCommand : ICommand
{
    public DifferenceInfoModel foundedDifference;

    public DiffFoundCommand(DifferenceInfoModel foundedDifference)
    {
        this.foundedDifference = foundedDifference;
    }
}