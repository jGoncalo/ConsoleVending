# Console Vending

A sample application for a vending machine with requirements:
- Once an item is selected and the appropriate amount of money is inserted, the vending machine should return the correct product.
- It should also return change if too much money is provided, or ask for more money if insufficient funds have been inserted.
- The machine should take an initial load of products and change. The change will be of denominations 1p, 2p, 5p, 10p, 20p, 50p, £1, £2. There should be a way of reloading either products or change at a later point.
- The machine should keep track of the products and change that it contains.

# Requirements
To run the program assure the machine meets the following requirements:
 - [DotNet6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (for compilation)
 - [DotNet6 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (for execution)

 # Compile

On the root of this repository run:
```sh
dotnet build
```

# Unit Tests

On the root of this repository run:
```sh
dotnet test
```

# Execute

On the root of the repository run:
```sh
dotnet run --project ./ConsoleVending.App/ConsoleVending.App.csproj
```

# Considerations

- Unit tests should have been made for all methods of IVendingMachine implementation, but where left out as most are "wrappers" for either ICurrencyHolder or IItemHolder
- ConsoleVending.Protocol does not allow for serialization of Data, this can be extended via a simple adition to the protocol for loading and saving to a given format (cloud, local file, etc), or by removing the direct data binding to use another storage pattern (ex: repository pattern) with the needed capabilities (as seen in implementation of IItemHolder)
- Event propagation to UI is very crude, reloading all data, to improve:
    - Add events to IVendingMachine that will update on relevant data events
    - Refactor ConsoleVending.App project to have better data encapsulation (more in line with StorageBox implementation)