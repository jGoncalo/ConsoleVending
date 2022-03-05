using System;

namespace ConsoleVending.Protocol.Exceptions{

public class InOperationException : Exception {
    public InOperationException() : base("Operation is not possible while in operation") {} 
}
}