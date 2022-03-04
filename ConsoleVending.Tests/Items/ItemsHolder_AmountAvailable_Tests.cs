using Xunit;
using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Exceptions;
using System.Collections.Generic;

namespace ConsoleVending.Tests.Items
{

    public class ItemsHolder_AmountAvailable_Tests
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

            var exception = Assert.Throws<ItemOperationException>(() => holder.AmountAvailable(20));

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

            var amount = holder.AmountAvailable(1);

            #endregion

            #region Validate

            Assert.Equal(2, amount);

            #endregion
        }
    }
}