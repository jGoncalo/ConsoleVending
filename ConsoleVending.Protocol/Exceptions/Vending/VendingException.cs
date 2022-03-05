using System;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Items;

namespace ConsoleVending.Protocol.Exceptions.Vending
{

    public class VendingException : Exception
    {
        public VendingException(string message) : base(message)
        {
        }
    }

    public class InsufficientException : VendingException
    {
        public readonly uint TargetValue;
        public readonly int TransactionTotal;

        public InsufficientException(uint targetValue, int transactionTotal) 
            : base("Intended transaction is short of target value")
        {
            TargetValue = targetValue;
            TransactionTotal = transactionTotal;
        }
    }
    
    public class LackOfChangeException : VendingException
    {
        public readonly IReadOnlyTransaction Transaction;
        public readonly Item TransactionItem;

        public LackOfChangeException(IReadOnlyTransaction transaction, Item transactionItem)
            : base("There is no change available for the intended transaction")
        {
            Transaction = transaction;
            TransactionItem = transactionItem;
        }
    }
}