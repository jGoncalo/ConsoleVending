using System;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Enums;
using ConsoleVending.Protocol.Vending;
using Terminal.Gui;

namespace ConsoleVending.App
{
    public class StorageBox : Window
    {
        private readonly IVendingMachine _vending;

        private Label? DenominationCounter;
        private Label? TotalValue;
        
        public StorageBox(ref IVendingMachine vending) : base("StorageBox:")
        {
            _vending = vending;
        }

        public void Init()
        {
            Add(new Label("Counter:") 
                {
                    X = 0, Y = 0, Width = Dim.Fill(), Height = 1
                },
                DenominationCounter = new Label()
                {
                  X = 0, Y = 1, Width  = Dim.Fill(), Height = 10
                },
                TotalValue = new Label()
                {
                    X = 0,  Y = Pos.Bottom(DenominationCounter), Height = 1, Width = Dim.Fill()
                });
            UpdateUi();
        }

        public void UpdateUi()
        {
            var current = _vending.CurrentMoney();
            if (DenominationCounter != null)
            {
                DenominationCounter.Text = $"1p  -> {current.AmountOf(Denomination.OnePenny)}\n" +
                                           $"2p  -> {current.AmountOf(Denomination.TwoPenny)}\n" +
                                           $"5p  -> {current.AmountOf(Denomination.FivePenny)}\n" +
                                           $"10p -> {current.AmountOf(Denomination.TenPenny)}\n" +
                                           $"20p -> {current.AmountOf(Denomination.TwentyPenny)}\n" +
                                           $"50p -> {current.AmountOf(Denomination.FiftyPenny)}\n" +
                                           $"1£  -> {current.AmountOf(Denomination.OnePound)}\n" +
                                           $"2£  -> {current.AmountOf(Denomination.TwoPound)}\n";
            }

            if (TotalValue != null)
            {
                TotalValue.Text = $"Total: {current.TotalValueString}";
            }
        }
    }
}