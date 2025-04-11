

namespace NetDapperWebApi.DTO
{
    public class AddRelationsMM<T, TKey>
    {
        public T EntityId { get; set; }
        public List<TKey> Ids { get; set; }
    }
}