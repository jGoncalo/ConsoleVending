using System.Collections.Generic;
using System;
using Xunit;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Enums;

namespace ConsoleVending.Tests.Currency {

    public class ItemsHolder_TransactionFor_Tests
    {
        [Fact]
        public void Empty()
        {
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>();
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            var transaction = holder.TransactionFor(100);
            #endregion

            #region Validate
            Assert.Null(transaction);
            #endregion
        }
        
        [Theory]
        [InlineData(Denomination.OnePenny)]
        [InlineData(Denomination.TwoPenny)]
        [InlineData(Denomination.FivePenny)]
        [InlineData(Denomination.TenPenny)]
        [InlineData(Denomination.TwentyPenny)]
        [InlineData(Denomination.FiftyPenny)]
        [InlineData(Denomination.OnePound)]
        [InlineData(Denomination.TwoPound)]
        public void PreciseChange(Denomination targetDenomination)
        {
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>{
                { Denomination.OnePenny, 1 },
                { Denomination.TwoPenny, 1 },
                { Denomination.FivePenny, 1 },
                { Denomination.TenPenny, 1 },
                { Denomination.TwentyPenny, 1 },
                { Denomination.FiftyPenny, 1 },
                { Denomination.OnePound, 1 },
                { Denomination.TwoPound, 1 }
            };
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            var transaction = holder.TransactionFor((uint) targetDenomination);
            #endregion

            #region Validate
            Assert.NotNull(transaction);
            Assert.Equal((int) targetDenomination, transaction?.TotalValue);
            foreach(var denomination in Enum.GetValues<Denomination>()){
                Assert.Equal(denomination == targetDenomination? 1 : 0, transaction?.AmountOf(denomination));
            }
            #endregion
        }
    
        [Theory]
        [InlineData(135)]
        [InlineData(142)]
        [InlineData(247)]
        [InlineData(347)]
        public void AvaliableChange(uint charge){
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>{
                { Denomination.OnePenny, 100 },
                { Denomination.TwoPenny, 100 },
                { Denomination.FivePenny, 100 },
                { Denomination.TenPenny, 100 },
                { Denomination.TwentyPenny, 100 },
                { Denomination.FiftyPenny, 100 },
                { Denomination.OnePound, 100 },
                { Denomination.TwoPound, 100 }
            };
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            var transaction = holder.TransactionFor(charge);
            #endregion

            #region Validate
            Assert.NotNull(transaction);
            Assert.Equal((int) charge, transaction?.TotalValue);
            #endregion
        }

        [Fact]
         public void ImpossibleChange_CrucialDenomination(){
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>{
                { Denomination.OnePenny, 0 },
                { Denomination.TwoPenny, 0 },
                { Denomination.FivePenny, 0 },
                { Denomination.TenPenny, 100 },
                { Denomination.TwentyPenny, 100 },
                { Denomination.FiftyPenny, 100 },
                { Denomination.OnePound, 100 },
                { Denomination.TwoPound, 100 }
            };
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            var transaction = holder.TransactionFor(152u);
            #endregion

            #region Validate
            Assert.Null(transaction);
            #endregion
        }
         [Fact]
         public void ImpossibleChange_NotEnoughDenomination(){
             #region Setup
             var currencyMap = new Dictionary<Denomination, int>{
                 { Denomination.OnePenny, 1 },
                 { Denomination.TwoPenny, 0 },
                 { Denomination.FivePenny, 0 },
                 { Denomination.TenPenny, 100 },
                 { Denomination.TwentyPenny, 100 },
                 { Denomination.FiftyPenny, 100 },
                 { Denomination.OnePound, 100 },
                 { Denomination.TwoPound, 100 }
             };
             var holder = new CurrencyHolder(ref currencyMap);
             #endregion

             #region Execute
             var transaction = holder.TransactionFor(152u);
             #endregion

             #region Validate
             Assert.Null(transaction);
             #endregion
         }
    }
}