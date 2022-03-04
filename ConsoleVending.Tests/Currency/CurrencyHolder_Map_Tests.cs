using System;
using System.Collections.Generic;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Enums;
using Xunit;

namespace ConsoleVending.Tests.Currency
{
    public class CurrencyHolder_Map_Tests
    {
        [Fact]
        public void Empty()
        {
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>();
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            var map = holder.Currency;
            #endregion

            #region Validate
            foreach (var denomination in Enum.GetValues<Denomination>())
            {
                Assert.True(map.ContainsKey(denomination));
                Assert.Equal(currencyMap[denomination], map[denomination]);
            }
            #endregion
        }
        [Fact]
        public void NotEmpty()
        {
            #region Setup
            var currencyMap = new Dictionary<Denomination, int>
            {
                { Denomination.OnePenny, 1 },
                { Denomination.TwoPenny, 2 },
                { Denomination.FivePenny, 4 },
                { Denomination.TenPenny, 5 },
                { Denomination.TwentyPenny, 8 },
                { Denomination.FiftyPenny, 2 },
                { Denomination.OnePound, 3 },
                { Denomination.TwoPound, 12 }
            };
            var holder = new CurrencyHolder(ref currencyMap);
            #endregion

            #region Execute
            var map = holder.Currency;
            #endregion

            #region Validate
            foreach (var denomination in Enum.GetValues<Denomination>())
            {
                Assert.True(map.ContainsKey(denomination));
                Assert.Equal(currencyMap[denomination], map[denomination]);
            }
            #endregion
        }
    }
}