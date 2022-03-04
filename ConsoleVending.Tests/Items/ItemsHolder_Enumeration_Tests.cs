using System;
using System.Collections.Generic;
using ConsoleVending.Protocol.Items;
using Xunit;

namespace ConsoleVending.Tests.Items
{
    public class ItemsHolder_Enumeration_Tests
    {
        [Fact]
        public void NoElements()
        {
            #region Setup
            var store = new Dictionary<uint, ItemAmount>();
            var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));
            #endregion

            #region Validate
            Assert.Empty(holder.Items);
            #endregion
        }
        [Fact]
        public void HasElements()
        {
            #region Setup
            var store = new Dictionary<uint, ItemAmount>
            {
                { 1, new ItemAmount(new Item("Item1", 1, 100), 2)},
                { 2, new ItemAmount(new Item("Item2", 2, 120), 4)},
                { 3, new ItemAmount(new Item("Item3", 3, 120), 3)}
            };
            var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));
            #endregion

            #region Validate
            var items = holder.Items;
            Assert.NotNull(items);
            Assert.Equal(store.Values.Count, items.Length);
            var pos = 0;
            foreach (var expected in store.Values)
            {
                var current = items[pos];
                Assert.Equal(expected, current);
                pos++;
            }

            #endregion
        }
    }
}