using ConsoleVending.Protocol.Enums;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Vending;

namespace ConsoleVending.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var itemHolder = new ItemsHolder();
            itemHolder.Upsert(new Item("Fallout New Vegas",         12,     155),   1);
            itemHolder.Upsert(new Item("Star Wars KOTOR",           231,    500),   5);
            itemHolder.Upsert(new Item("Red Dead Redemption 2",     322,    750),   5);
            itemHolder.Upsert(new Item("Tomb Raider 3",             44,     53),    5);
            itemHolder.Upsert(new Item("Halo 3",                    117,    700),   5);

            var currencyHolder = new CurrencyHolder();
            currencyHolder.AddCurrency(new Transaction()
                .Push(Denomination.OnePenny, 5)
                .Push(Denomination.TwoPenny, 5)
                .Push(Denomination.FivePenny, 5)
                .Push(Denomination.TenPenny, 5)
                .Push(Denomination.TwentyPenny, 5)
                .Push(Denomination.FiftyPenny, 5)
                .Push(Denomination.OnePound, 5)
                .Push(Denomination.TwoPound, 5));
            
            IVendingMachine vendingMachine = new VendingMachine(currencyHolder, itemHolder);

            var app = new AppUi(ref vendingMachine);
            app.Init();
            
            #region Set Bindings
            app.MaintenanceToggled += (sender, isMaintenance) => vendingMachine.InMaintenance = isMaintenance;
            app.OnItemSelected += (sender, itemCode) => {
                try
                {
                    IReadOnlyTransaction? transaction = null;
                    if(itemCode == null) transaction = vendingMachine.CancelSelection();
                    else {
                        try { transaction = vendingMachine.SelectItem(itemCode.Value); }
                        catch { transaction = vendingMachine.CancelSelection(); }
                    }
                    app.ReloadData();
                    if(transaction != null) app.DisplayTransaction("Returned", transaction);
                }
                catch (Exception exp)
                {
                    app.DisplayError(exp);
                }
            };
            app.OnPushed += (sender, denomination) =>
            {
                try
                {
                    if(vendingMachine.InMaintenance){
                        vendingMachine.LoadMoney(new Transaction()
                            .Push(denomination, 1));
                    }
                    else {
                        var transaction = vendingMachine.PushMoney(denomination, 1);
                        if (transaction != null)
                        {
                            app.DisplayTransaction("Machine output:", transaction.Value);
                        }
                    }
                }
                catch (Exception exp)
                {
                    app.DisplayError(exp);
                }
                finally{
                    app.ReloadData();
                }
            };
            app.OnCancel += (sender, args) =>
            {
                var transcation = vendingMachine.CancelSelection();
                app.ReloadData(true);
                if(transcation != null) app.DisplayTransaction("Returned", transcation);
            };
            app.ReloadData();
            #endregion
            
            app.Run();
        }
    }
}