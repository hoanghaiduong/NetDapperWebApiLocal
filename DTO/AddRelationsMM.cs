

namespace NetDapperWebApi_local.DTO
{
    public class AddRelationsMM<T, TKey>
    {
        public T EntityId { get; set; }
        public List<TKey> Ids { get; set; }
    }
}