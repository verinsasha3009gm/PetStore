namespace PetStore.Markets.Domain.Result
{
    public class CollectionResult<T> : BaseResult<IEnumerable<T>>
    {
        public long Count { get; set; }
    }
}
