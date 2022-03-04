using System;

namespace ConsoleVending.Protocol.Exceptions{
    public class ItemOperationException : Exception {
        public ItemOperationException(string message) : base(message) {}
    }
}