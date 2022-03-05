using System;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Vending;

namespace ConsoleVending.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var currencyHolder = new CurrencyHolder();
            var itemHolder = new ItemsHolder();
            
            itemHolder.Upsert(new Item("Fallout New Vegas", 12, 200), 1);
            itemHolder.Upsert(new Item("Star Wars KOTOR II", 231, 45), 5);
            itemHolder.Upsert(new Item("Red Dead Redemption 2", 322, 500), 5);
            itemHolder.Upsert(new Item("Tomb Raider 3", 44, 250), 5);
            
            IVendingMachine vendingMachine = new VendingMachine(currencyHolder, itemHolder);

            var app = new AppUi(ref vendingMachine);
            app.Init();
            
            #region Set Bindings
            app.MaintenanceToggled += (sender, isMaintenance) => vendingMachine.InMaintenance = isMaintenance;
            app.OnItemSelected += (sender, itemCode) => {
                try
                {
                    vendingMachine.SelectItem(itemCode);
                    app.ReloadData();
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
                    var transaction = vendingMachine.PushMoney(denomination, 1);
                    app.ReloadData();
                    if (transaction != null)
                    {
                        
                    }
                }
                catch (Exception exp)
                {
                    app.DisplayError(exp);
                }
            };
            app.OnCancel += (sender, args) =>
            {
                vendingMachine.CancelSelection();
                app.ReloadData();
            };
            app.ReloadData();
            #endregion
            
            app.Run();
        }
    }
}