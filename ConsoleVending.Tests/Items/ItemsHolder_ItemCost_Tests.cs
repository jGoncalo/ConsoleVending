using Xunit;
using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Exceptions;
using System.Collections.Generic;

namespace ConsoleVending.Tests.Items
{

    public class ItemsHolder_ItemCost_Tests
    {
        [Fact]
        public void UnknownProduct()
        {
            #region Setup

            var existingProduct = new Item("Product", 1, 100);
            var store = new Dictionary<uint, ItemAmount>
            {
                { 1, new ItemAmount(existingProduct, 2) }
            };
            var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));

            #endregion

            #region Execute

            var exception = Assert.Throws<ItemOperationException>(() => holder.ItemCost(20));

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
                { 1, new ItemAmount(existingProduct, 2) }
            };
            var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));

            #endregion

            #region Execute

            var cost = holder.ItemCost(1);

            #endregion

            #region Validate

            Assert.Equal(100u, cost);

            #endregion
        }
    }
}