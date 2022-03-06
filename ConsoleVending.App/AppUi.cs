using System;
using ConsoleVending.Protocol.Enums;
using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Vending;
using ConsoleVending.Protocol.Currency;
using Terminal.Gui;

namespace ConsoleVending.App
{
    public class AppUi
    {
        public event EventHandler<bool>? MaintenanceToggled;
        public event EventHandler<uint?>? OnItemSelected;
        public event EventHandler<Denomination>? OnPushed;
        public event EventHandler? OnCancel;

        private Label? _modeLabel;
        private ListView? _itemList;
        private Label? _inputLabel;
        private TextField? _itemInput;
        private InfoBox? _infoBox;
        private StorageBox? _storageBox;


        private IVendingMachine _vendingMachine;
        public AppUi(ref IVendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
        }
        
        public void ReloadData(bool clearCode = false)
        {
            _itemList?.SetSource(_vendingMachine.AvailableItems());
            if (_infoBox != null)
            {
                _infoBox.SelectedItem = _vendingMachine.SelectedItem;
                _infoBox.InputTransaction = _vendingMachine.CurrentTransaction;
            }
            if(_itemInput != null && clearCode) _itemInput.Text = string.Empty;
            _storageBox?.UpdateUi();
        }
        
        public void Init(){
            Application.Init();

            var menu = new MenuBar (new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Mode", "", () =>
                    {
                        var isInMaintanance = QueryIsInMaintenance();
                        MaintenanceToggled?.Invoke(this, isInMaintanance);
                    }),
                    new MenuItem("_Quit", "", () => {
                        if(Quit()) Application.RequestStop();
                    })
                }),
                new MenuBarItem("_Items", new MenuItem[] {
                    new MenuItem("_Replenish", "", () => {
                        var selectedItem = _vendingMachine.SelectedItem;
                        if(selectedItem == null) {
                            DisplayError("No item Selected", "Please select an item");
                        }
                        else {
                            try {
                                var amount = MessageBox.Query("Replenish", "Pick the amount", "1", "2", "3", "4", "5") +1;
                                _vendingMachine.ReplenishItem(selectedItem.Value.Code, (uint) amount);
                            } catch(Exception exp){
                                DisplayError(exp);
                            }
                        }
                        ReloadData();
                    }),
                    new MenuItem("_Add", "", () => {
                        DisplayAddItemDialog();
                    })
                }),
                new MenuBarItem("_Currency", new MenuItem[] {
                    new MenuItem("_Unload", "", () => {
                        try {
                            var transaction = _vendingMachine.CurrentMoney();
                            transaction = _vendingMachine.RemoveMoney(transaction);
                            DisplayTransaction("Unloaded money:", transaction);
                        } catch(Exception exp){
                            DisplayError(exp);
                        }
                        finally{
                            ReloadData();
                        }
                    })
                })
            });

            _modeLabel = new Label("Mode: OPERATIONAL")
            {
                X = 0, Y = 1,
                Width = Dim.Fill(),
                Height = 1
            };
            
            var itemWindow = new Window("Items:"){
                X = 0, Y = 2,
                Width = Dim.Percent(50, true), Height = Dim.Fill()
            };
            itemWindow.Add(_itemList = new ListView() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill () - 3
            },
            _inputLabel = new Label("Code indput:"){
                X = 0, Y = Pos.Bottom(_itemList),
                Height = 1, Width = Dim.Fill()
            },
            _itemInput = new TextField(){
                X = 0, Y = Pos.Bottom(_inputLabel),
                Height = 1, Width = Dim.Fill()
            });

            _infoBox = new InfoBox()
            {
                X = Pos.Percent(50),
                Y = 2,
                Width = Dim.Fill(), Height = Dim.Percent(50, true)
            };
            _infoBox.Init();
            _storageBox = new StorageBox(ref _vendingMachine)
            {
                X = Pos.Percent(50),
                Y = Pos.Percent(50)+1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            _storageBox.Init();
            
            Application.Top.Add(
                menu,
                _modeLabel,
                itemWindow,
                _infoBox,
                _storageBox);
            
            #region Set Internal Listeners
            MaintenanceToggled += (sender, isMaintenance) => {
                _modeLabel.Text = "Mode: " + (isMaintenance ? "MAINTENANCE" : "OPERATIONAL");
            };
            _infoBox.OnPushed += (sender, denomination) => OnPushed?.Invoke(sender, denomination);
            _infoBox.OnCancel += (sender, args) => OnCancel?.Invoke(sender, args);
            _itemInput.TextChanging += (textArgs) => {
                var couldParse = uint.TryParse(textArgs.NewText.ToString(), out var code);
                if(!couldParse) {
                    DisplayError("Code input", $"input should be numeric only!");
                    OnItemSelected?.Invoke(this, null);
                }
                else {
                    OnItemSelected?.Invoke(this, code);
                }
            };
            #endregion
        }

        public void Run()
        {
            Application.Run();
        }

        private bool QueryIsInMaintenance()
        {
            var n = MessageBox.Query("Machine Mode", "Pick the machine's operation mode", "Maintenance", "Operation");
            return n == 0;
        }
        
        private bool Quit(){
            var n = MessageBox.Query("Quit", "Are you sure you want to leave the vending machine?", "Yes", "No");
            return n == 0;
        }

        public void DisplayError(string title, string message)
        {
            MessageBox.ErrorQuery(title, message, "Ok");
        }
        public void DisplayError(Exception exp)
        {
            DisplayError(exp.GetType().Name, exp.Message);
        }
        public void DisplayTransaction(string title, IReadOnlyTransaction transaction){
            MessageBox.Query(title, transaction.ToString(), "Understood");
        }
        public void DisplayTransaction(string title, VendingTransaction transaction){
            MessageBox.Query(title, transaction.ToString(), "Understood");
        }

        public void DisplayAddItemDialog(){
            var okButton = new Button("Add");

            var nameLabel = new Label("Name:"){
                X = 0, Y = 0, Width = Dim.Fill(), Height = 1
            };
            var nameField = new TextField(){
                X = 0, Y = 1, Width = Dim.Fill(), Height = 1
            };

            var codeLabel = new Label("code:"){
                X = 0, Y = 2, Width = Dim.Fill(), Height = 1
            }; 
            var codeField = new TextField(){
                X = 0, Y = 3, Width = Dim.Fill(), Height = 1
            };

            var costLabel = new Label("cost (in pences):"){
                X = 0, Y = 4, Width = Dim.Fill(), Height = 1
            };
            var costField = new TextField(){
                X = 0, Y = 5, Width = Dim.Fill(), Height = 1
            };

            var dialog = new Dialog("Add Item:", okButton);
            okButton.Clicked += () => {
                try{
                    var name = nameField.Text.ToString();
                    var validCode = uint.TryParse(codeField.Text.ToString(), out var code);
                    var validCost = uint.TryParse(costField.Text.ToString(), out var cost);

                    if(string.IsNullOrEmpty(name)) throw new ArgumentException("Name must not be empty...");
                    if(!validCode) throw new ArgumentException("Code must be a positive number");
                    if(!validCost) throw new ArgumentException("Cost must be a positive number");

                    _vendingMachine.LoadItem(new Item(name, code, cost), 1);
                }
                catch(Exception exp){
                    DisplayError(exp);
                }
                finally{
                    dialog.RequestStop();
                    ReloadData();
                }
            };

            dialog.Add(nameLabel, nameField, codeLabel, codeField, costLabel, costField);

            Application.Run(dialog);
        }
    }
}