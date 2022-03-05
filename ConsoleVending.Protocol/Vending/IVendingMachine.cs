using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Enums;

namespace ConsoleVending.Protocol.Vending
{

    public interface IVendingMachine
    {
        bool InMaintenance { get; set; }

        #region Item
        Item? SelectedItem { get; }
        IReadOnlyTransaction CurrentTransaction { get; }

        IReadOnlyTransaction CancelSelection();
        void SelectItem(uint itemCode);
        VendingTransaction? PushMoney(Denomination denomination, int amount);
        
        
        ItemAmount[] AvailableItems(bool includeSoldOut = false);
        uint ItemCost(uint itemCode);
        bool IsItemAvailable(uint itemCode);

        void LoadItem(Item item, uint amount);
        void ReplenishItem(uint itemCode, uint amount);
        #endregion

        #region Currency
        IReadOnlyTransaction CurrentMoney();
        void LoadMoney(IReadOnlyTransaction transaction);
        IReadOnlyTransaction RemoveMoney(IReadOnlyTransaction transaction);
        #endregion

    }
}