using System;
using ConsoleVending.Protocol.Enums;
using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Vending;
using Terminal.Gui;

namespace ConsoleVending.App
{
    public class AppUi
    {
        public event EventHandler<bool>? MaintenanceToggled;
        public event EventHandler<uint>? OnItemSelected;
        public event EventHandler<Denomination>? OnPushed;
        public event EventHandler? OnCancel;

        private Label? _modeLabel;
        private ListView? _itemList;
        private InfoBox? _infoBox;
        private StorageBox? _storageBox;


        private IVendingMachine _vendingMachine;
        public AppUi(ref IVendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
        }
        
        public void ReloadData()
        {
            _itemList?.SetSource(_vendingMachine.AvailableItems());
            if (_infoBox != null)
            {
                _infoBox.SelectedItem = _vendingMachine.SelectedItem;
                _infoBox.InputTransaction = _vendingMachine.CurrentTransaction;
            }
            _storageBox?.UpdateUi();
        }
        
        public void Init(){
            Application.Init();

            var menu = new MenuBar (new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Maintenance", "toogle maintenance", () =>
                    {
                        var toggle = ChangeMode();
                        MaintenanceToggled?.Invoke(this, toggle);
                    }),
                    new MenuItem("_Quit", "quit app", () => {
                        if(Quit()) Application.RequestStop();
                    })
                })
            });

            _modeLabel = new Label("Mode: OPERATIONAL")
            {
                X = 0, Y = 1,
                Width = Dim.Fill(),
                Height = 1
            };
            
            _itemList = new ListView() {
                X = 0,
                Y = 2,
                Width = Dim.Percent(50, true),
                Height = Dim.Fill () - 1
            };

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
                _itemList,
                _infoBox,
                _storageBox);
            
            #region Set Internal Listeners
            MaintenanceToggled += (sender, isMaintenance) => {
                _modeLabel.Text = "Mode: " + (isMaintenance ? "MAINTENANCE" : "OPERATIONAL");
            };
            _itemList.SelectedItemChanged += (args) => {
                if (args.Value is ItemAmount itemAmount)
                {
                    OnItemSelected?.Invoke(this, itemAmount.Item.Code);
                }
            };
            _infoBox.OnPushed += (sender, denomination) => OnPushed?.Invoke(sender, denomination);
            _infoBox.OnCancel += (sender, args) => OnCancel?.Invoke(sender, args);

            #endregion
        }

        public void Run()
        {
            Application.Run();
        }

        private bool ChangeMode()
        {
            var n = MessageBox.Query("Maintenance", "Is the vending machine in maintenance", "Yes", "No");
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
    }
}