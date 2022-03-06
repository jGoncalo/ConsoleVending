using System;
using System.Linq;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Enums;
using ConsoleVending.Protocol.Exceptions;
using ConsoleVending.Protocol.Exceptions.Vending;
using ConsoleVending.Protocol.Items;

namespace ConsoleVending.Protocol.Vending
{

    public class VendingMachine : IVendingMachine
    {
        private readonly ICurrencyHolder _currencyHolder;
        private readonly IItemsHolder _itemsHolder;

        public VendingMachine(ICurrencyHolder currencyHolder, IItemsHolder itemsHolder)
        {
            _currencyHolder = currencyHolder ?? throw new ArgumentNullException(nameof(currencyHolder));
            _itemsHolder = itemsHolder ?? throw new ArgumentNullException(nameof(itemsHolder));
        }

        public bool InMaintenance { get; set; } = false;

        public Item? SelectedItem { get; private set; } = null;
        private readonly Transaction _currentTransaction = new (); 
        public IReadOnlyTransaction CurrentTransaction => _currentTransaction;

        public IReadOnlyTransaction? CancelSelection()
        {
            if(SelectedItem == null) return null;

            var returnedTransaction = new Transaction();
            if(_currentTransaction.TotalValue != 0) {
                foreach (var denomination in Enum.GetValues<Denomination>())
                    returnedTransaction.Push(denomination, _currentTransaction.AmountOf(denomination));
            }

            _currentTransaction.Reset();
            SelectedItem = null;

            return returnedTransaction;
        }

        public IReadOnlyTransaction? SelectItem(uint itemCode)
        {
            if (!IsItemAvailable(itemCode)) throw new VendingException("Item not available");
            var transaction = SelectedItem == null ? null : CancelSelection();
            SelectedItem = _itemsHolder.Inspect(itemCode);

            return transaction;
        }

        public VendingTransaction? PushMoney(Denomination denomination, int amount)
        {
            if (amount < 0) throw new ArgumentException("Pushed amount cannot be negative", nameof(amount));
            if (SelectedItem == null) throw new VendingException("No item is selected");
            _currentTransaction.Push(denomination, amount);

            var itemCost = SelectedItem.Value.Cost;
            var transactionAmount = _currentTransaction.TotalValue;

            try
            {
                if (transactionAmount < itemCost) return null;
                //Handle Change
                var change = (uint)transactionAmount - itemCost;
                IReadOnlyTransaction? changeTransaction = null;

                if (change != 0)
                {
                    changeTransaction = _currencyHolder.TransactionFor(change);
                    if (changeTransaction == null) throw new VendingException("Change not available");

                    //remove change
                    _currencyHolder.RemoveCurrency(changeTransaction);
                }
                
                //Take money
                _currencyHolder.AddCurrency(_currentTransaction);
                _currentTransaction.Reset();

                //Item dispensing
                var item = _itemsHolder.Take(SelectedItem.Value.Code);
                return new VendingTransaction(item, changeTransaction);
            }
            catch (VendingException)
            {
                //cancel operation
                var cancelTransaction = CancelSelection();
                return new VendingTransaction(null, cancelTransaction);
            }
        }

        public ItemAmount[] AvailableItems(bool includeSoldOut = false)
        {
            return _itemsHolder.Items
                .Where(ia => includeSoldOut || ia.Amount > 0)
                .ToArray();
        }

        public uint ItemCost(uint itemCode)
        {
            return _itemsHolder.ItemCost(itemCode);
        }

        public bool IsItemAvailable(uint itemCode)
        {
            return _itemsHolder.AmountAvailable(itemCode) != 0;
        }

        public void LoadItem(Item item, uint amount)
        {
            if (!InMaintenance) throw new InOperationException();
            _itemsHolder.Upsert(item, amount);
        }

        public void ReplenishItem(uint itemCode, uint amount)
        {
            if (!InMaintenance) throw new InOperationException();
            _itemsHolder.Replenish(itemCode, amount);
        }

        public IReadOnlyTransaction CurrentMoney()
        {
            var current = _currencyHolder.Currency;
            var transaction = new Transaction();

            foreach (var (denomination, amount) in current)
            {
                transaction.Push(denomination, amount);
            }
            
            return transaction;
        }

        public void LoadMoney(IReadOnlyTransaction transaction)
        {
            if (!InMaintenance) throw new InOperationException();
            _currencyHolder.AddCurrency(transaction);
        }

        public IReadOnlyTransaction RemoveMoney(IReadOnlyTransaction transaction)
        {
            if (!InMaintenance) throw new InOperationException();
            _currencyHolder.RemoveCurrency(transaction);
            return transaction;
        }
    }
}