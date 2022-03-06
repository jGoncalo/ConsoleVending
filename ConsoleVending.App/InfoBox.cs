using System;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Enums;
using ConsoleVending.Protocol.Items;
using Terminal.Gui;

namespace ConsoleVending.App
{
    internal class InfoBox : Window
    {
        private Label? selectedName;
        private Label? selectedCost;
        private Label? totalInserted;

        private Button? cancelBtn;
        
        private Button? insert1Btn;
        private Button? insert2Btn;
        private Button? insert5Btn;
        private Button? insert10Btn;
        private Button? insert20Btn;
        private Button? insert50Btn;
        private Button? insert100Btn;
        private Button? insert200Btn;

        public event EventHandler<Denomination>? OnPushed;
        public event EventHandler? OnCancel;
            
        private Item? _selectedItem;
        public Item? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                UpdateUi();
            }
        }

        private IReadOnlyTransaction? _inputTransaction;

        public IReadOnlyTransaction? InputTransaction
        {
            get => _inputTransaction;
            set
            {
                _inputTransaction = value;
                UpdateUi();
            }
        }

        private void UpdateUi()
        {
            if (selectedName != null) selectedName.Text = _selectedItem?.Name ?? "NONE";
            if (selectedCost != null) selectedCost.Text = _selectedItem?.CostString ?? "0.00£";
            if (totalInserted != null) totalInserted.Text = _inputTransaction?.TotalValueString ?? "0.00£";
        }
            
        public InfoBox() : base("ItemSelection:")
        {
               
        }

        public void Init()
        {
            Add(
                new Label("SelectedItem:")
                {
                    X = 0, Y = 0,
                    Width = Dim.Fill(), Height = 1
                },
                selectedName = new Label(_selectedItem?.Name ?? "NONE") {
                    X = 0, Y = 1,
                    Width = Dim.Fill(), Height = 1
                },
                selectedCost = new Label(_selectedItem?.CostString ?? "0.00£") {
                    X = 0, Y =2,
                    Width = Dim.Fill(), Height = 1
                },
                totalInserted = new Label(_inputTransaction?.TotalValueString ?? "0.00£")
                {
                    X = 0, Y = 3,
                    Width = Dim.Fill(), Height = 1
                },
                insert1Btn = new Button("1p") {
                    X = 0, Y = 4,
                    Width = Dim.Percent(50, true), Height = 1 
                },
                insert2Btn = new Button("2p") {
                    X = Pos.Percent(50), Y = 4,
                    Width = Dim.Percent(50, true), Height = 1
                },
                insert5Btn = new Button("5p") {
                    X = 0, Y = 5,
                    Width = Dim.Percent(50, true), Height = 1 
                },
                insert10Btn = new Button("10p") {
                    X = Pos.Percent(50), Y = 5,
                    Width = Dim.Percent(50, true), Height = 1
                },
                insert20Btn = new Button("20p") {
                    X = 0, Y = 6,
                    Width = Dim.Percent(50, true), Height = 1 
                },
                insert50Btn = new Button("50p") {
                    X = Pos.Percent(50), Y = 6,
                    Width = Dim.Percent(50, true), Height = 1
                },
                insert100Btn = new Button("1£") {
                    X = 0, Y = 7,
                    Width = Dim.Percent(50, true), Height = 1 
                },
                insert200Btn = new Button("2£") {
                    X = Pos.Percent(50), Y = 7,
                    Width = Dim.Percent(50, true), Height = 1
                },
                cancelBtn = new Button("Cancel Operation")
                {
                    X = 0, Y = 8,
                    Width = Dim.Fill(), Height = 1
                });
            insert1Btn.Clicked += () => OnPushed?.Invoke(this, Denomination.OnePenny);
            insert2Btn.Clicked += () => OnPushed?.Invoke(this, Denomination.TwoPenny);
            insert5Btn.Clicked += () => OnPushed?.Invoke(this, Denomination.FivePenny);
            insert10Btn.Clicked += () => OnPushed?.Invoke(this, Denomination.TenPenny);
            insert20Btn.Clicked += () => OnPushed?.Invoke(this, Denomination.TwentyPenny);
            insert50Btn.Clicked += () => OnPushed?.Invoke(this, Denomination.FiftyPenny);
            insert100Btn.Clicked += () => OnPushed?.Invoke(this, Denomination.OnePound);
            insert200Btn.Clicked += () => OnPushed?.Invoke(this, Denomination.TwoPound);
            cancelBtn.Clicked += () => OnCancel?.Invoke(this, EventArgs.Empty);
        }
            
    }
}