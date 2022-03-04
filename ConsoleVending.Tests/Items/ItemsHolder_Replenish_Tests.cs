using Xunit;
using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Exceptions;
using System.Collections.Generic;

namespace ConsoleVending.Tests.Items
{

    public class ItemsHolder_Replenish_Tests
    {
        [Fact]
        public void UnknownProduct()
        {
            #region Setup

            var existingProduct = new Item("Product", 1, 100);
            var store = new Dictionary<uint, ItemAmount>
            {
                { 1, new ItemAmount(existingProduct, 1) }
            };
            var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));

            #endregion

            #region Execute

            var exception = Assert.Throws<ItemOperationException>(() => holder.Replenish(20, 10));

            #endregion

            #region Validate

            Assert.Equal("no item with code: 20", exception.Message);

            #endregion
        }

        [Fact]
        public void KnownProduct()
        {
            #region Setup

            var existingProduct = new Item("Product", 1, 100);
            var store = new Dictionary<uint, ItemAmount>
            {
                { 1, new ItemAmount(existingProduct, 1) }
            };
            var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));

            #endregion

            #region Execute

            holder.Replenish(1, 12);

            #endregion

            #region Validate

            Assert.True(store.ContainsKey(1));
            var target = store[1];
            Assert.Equal(existingProduct, target.Item);
            Assert.Equal(13, target.Amount);

            #endregion
        }
    }
}