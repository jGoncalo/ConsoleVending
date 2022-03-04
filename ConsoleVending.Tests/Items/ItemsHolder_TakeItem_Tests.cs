using Xunit;
using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Exceptions;
using System.Collections.Generic;

namespace ConsoleVending.Tests.Items
{

    public class ItemsHolder_TakeItem_Tests
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

            var exception = Assert.Throws<ItemOperationException>(() => holder.Take(20));

            #endregion

            #region Validate

            Assert.Equal("no item with code: 20", exception.Message);

            #endregion
        }

        [Fact]
        public void IsAvailable()
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

            var item = holder.Take(1);

            #endregion

            #region Validate

            Assert.Equal(existingProduct, item);

            #endregion
        }

        [Fact]
        public void IsNotAvailable()
        {
            #region Setup

            var existingProduct = new Item("Product", 1, 100);
            var store = new Dictionary<uint, ItemAmount>
            {
                { 1, new ItemAmount(existingProduct, 0) }
            };
            var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));

            #endregion

            #region Execute

            var exception = Assert.Throws<ItemOperationException>(() => holder.Take(1));

            #endregion

            #region Validate

            Assert.Equal("Item with code: 1 is no longer available", exception.Message);

            #endregion
        }
    }
}