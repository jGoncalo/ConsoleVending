namespace ConsoleVending.Protocol.Items{

    public interface IItemsHolder {
        int AmountAvailable(uint itemCode);
        uint ItemCost(uint itemCode);

        Item Inspect(uint itemCode);
        Item Take(uint itemCode);
        void Upsert(Item item, uint amount);
        void Replenish(uint itemCode, uint amount);
        
        ItemAmount[] Items { get; }
    }
}