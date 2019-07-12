namespace EL.Http
{
    public interface IRequestSerializer
    {
        string SerializeBody(object body);
    }
}