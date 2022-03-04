using System.Collections.Generic;
using System.Linq;
using Xunit;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Enums;

namespace ConsoleVending.Tests.Currency {

    public class ItemsHolder_RemoveCurrency_Tests
    {
        [Fact]
        public void RemoveNoAmount()
        {
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>();
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            holder.RemoveCurrency(new Transaction());
            #endregion

            #region Validate
            Assert.True(currencyMap.All(kv => kv.Value == 0));
            #endregion
        }
        [Fact]
        public void RemoveAmount()
        {
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>
            {
                { Denomination.FivePenny, 10 }
            };
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            holder.RemoveCurrency(new Transaction()
                .Push(Denomination.FivePenny, 10));
            #endregion

            #region Validate
            Assert.All(currencyMap, kv => Assert.Equal(0, kv.Value));
            #endregion
        }
        [Fact]
        public void RemoveNegativeAmount()
        {
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>
            {
                { Denomination.FivePenny, 0 }
            };
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            holder.RemoveCurrency(new Transaction()
                .Push(Denomination.FivePenny, -10));
            #endregion

            #region Validate
            Assert.Equal(10, currencyMap[Denomination.FivePenny]);
            Assert.All(currencyMap.Where(kv => kv.Key != Denomination.FivePenny), 
                kv => Assert.Equal(0, kv.Value));
            #endregion
        }
        [Fact]
        public void AttemptBellowZero()
        {
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>
            {
                { Denomination.FivePenny, 0 }
            };
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            var exception = Assert.Throws<CurrencyOperationException>(() => holder.RemoveCurrency(new Transaction()
                .Push(Denomination.FivePenny, 10)));
            #endregion

            #region Validate
            Assert.Equal("Transaction would result in negative amount of denomination", exception.Message);
            #endregion
        }
    }
}