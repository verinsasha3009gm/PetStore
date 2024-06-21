namespace PetStore.Users.Domain.Result
{
    public class MessageResult 
    {
        public string HttpMethod;
        public string TypeMessage;
    }
    public class MessageResult<T> : MessageResult
    {
        public T Data;
    }
}
