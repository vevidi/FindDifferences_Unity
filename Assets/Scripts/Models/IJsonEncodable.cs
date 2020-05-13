namespace Vevidi.FindDiff.NetworkModel
{
    public interface IJsonEncodable
    {
        string Encode();
        void Decode(string json);
    }
}