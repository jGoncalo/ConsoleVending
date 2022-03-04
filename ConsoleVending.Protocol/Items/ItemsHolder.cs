using System.Linq;
using ConsoleVending.Protocol.Repository;
using ConsoleVending.Protocol.Exceptions;

namespace ConsoleVending.Protocol.Items
{

    public class ItemsHolder : IItemsHolder
    {
        private readonly IRepository<uint, ItemAmount> _repo;

        public ItemAmount[] Items => _repo.Contents.ToArray();

        public ItemsHolder(IRepository<uint, ItemAmount>? itemRepo = null)
        {
            _repo = itemRepo ?? new Repository<uint, ItemAmount>();
        }

        public int AmountAvailable(uint itemCode)
        {
            var contains = _repo.TryGet(itemCode, out var target);
            if (!contains) throw new ItemOperationException($"no item with code: {itemCode}");

            return target.Amount;
        }

        public uint ItemCost(uint itemCode)
        {
            var contains = _repo.TryGet(itemCode, out var target);
            if (!contains) throw new ItemOperationException($"no item with code: {itemCode}");

            return target.Item.Cost;
        }

        public Item Take(uint itemCode)
        {
            var contains = _repo.TryGet(itemCode, out var target);
            if (!contains) throw new ItemOperationException($"no item with code: {itemCode}");

            if (target.Amount == 0)
                throw new ItemOperationException($"Item with code: {itemCode} is no longer available");

            _repo[itemCode] = target - 1;

            return target.Item;
        }

        public void Add(Item item, uint amount)
        {
            if (amount == 0) return;
            var contains = _repo.TryGet(item.Code, out var target);
            if (contains) Replenish(item.Code, amount);
            else
            {
                _repo[item.Code] = new ItemAmount(item, (int)amount);
            }
        }

        public void Replenish(uint itemCode, uint amount)
        {
            if (amount == 0) return;
            var contains = _repo.TryGet(itemCode, out var target);
            if (!contains) throw new ItemOperationException($"no item with code: {itemCode}");

            _repo[itemCode] = target + (int)amount;
        }
    }
}