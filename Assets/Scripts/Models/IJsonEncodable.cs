namespace Vevidi.FindDiff.Model
{
    public interface IJsonEncodable
    {
        string Encode();
        void Decode(string json);
    }
}