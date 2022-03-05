using System;

namespace ConsoleVending.Protocol.Exceptions{

public class InMaintenanceException : Exception {
    public InMaintenanceException() : base("Operation is not possible while in maintenance") {} 
}
}