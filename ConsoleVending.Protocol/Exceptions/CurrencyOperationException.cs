using System;

namespace ConsoleVending.Protocol.Currency
{
    public class CurrencyOperationException : Exception {
        public CurrencyOperationException(string message) : base(message) {}
    }
}