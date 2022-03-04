using System;

namespace ConsoleVending.Protocol.Exceptions{

public class InMaintenanceException : Exception {
    public InMaintenanceException(string message) : base(message) {} 
}
}