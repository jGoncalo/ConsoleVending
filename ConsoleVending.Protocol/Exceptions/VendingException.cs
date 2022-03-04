using System;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Items;

namespace ConsoleVending.Protocol.Exceptions
{

    public class VendingException : Exception
    {
        public VendingException(string message) : base(message)
        {
        }
    }

    public class VendingTransactionException : VendingException
    {
        public readonly IReadOnlyTransaction MissingFromTransaction;
        public readonly Item TransactionItem;

        public VendingTransactionException(IReadOnlyTransaction missingFromTransaction, Item transactionItem,
            string message)
            : base(message)
        {
            MissingFromTransaction = missingFromTransaction;
            TransactionItem = transactionItem;
        }
    }
}