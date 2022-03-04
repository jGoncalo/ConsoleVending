namespace ConsoleVending.Protocol.Items{

    public interface IItemsHolder {
        int AmountAvailable(uint itemCode);
        uint ItemCost(uint itemCode);

        Item Take(uint itemCode);
        void Add(Item item, uint amount);
        void Replenish(uint itemCode, uint amount);
        
        ItemAmount[] Items { get; }
    }
}