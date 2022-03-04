using Xunit;
using ConsoleVending.Protocol.Items;
using System.Collections.Generic;

namespace ConsoleVending.Tests.Items {

public class ItemsHolder_Add_Tests {
    [Fact]
    public void NewProduct_NoAmount(){
        #region Setup
        var store = new Dictionary<uint, ItemAmount>();
        var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));
        #endregion

        #region Execute
        var insertedProduct = new Item("New Product", 1, 100);
        holder.Add(insertedProduct, 0);
        #endregion

        #region Validate
        Assert.Empty(store);
        #endregion
    }

    [Fact]
    public void NewProduct_WithAmount(){
        #region Setup
        var store = new Dictionary<uint, ItemAmount>();
        var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));
        #endregion

        #region Execute
        var insertedProduct = new Item("New Product", 1, 100);
        holder.Add(insertedProduct, 12);
        #endregion

        #region Validate
        Assert.NotEmpty(store);
        var contains = store.TryGetValue(insertedProduct.Code, out var target);
        Assert.True(contains);
        Assert.Equal(12, target.Amount);
        Assert.Equal(insertedProduct, target.Item);
        #endregion
    }

    [Fact]
    public void ExistingProduct_NoAmount(){
        #region Setup
        var existingProduct = new Item("New Product", 1, 100);
        var store = new Dictionary<uint, ItemAmount>{
            {1,  new ItemAmount(existingProduct, 2)}
        };
        var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));
        #endregion

        #region Execute
        holder.Add(existingProduct, 0);
        #endregion

        #region Validate
        Assert.NotEmpty(store);
        var contains = store.TryGetValue(existingProduct.Code, out var target);
        Assert.True(contains);
        Assert.Equal(2, target.Amount);
        Assert.Equal(existingProduct, target.Item);
        #endregion
    }

    [Fact]
    public void ExistingProduct_WithAmount(){
        #region Setup
        var existingProduct = new Item("New Product", 1, 100);
        var store = new Dictionary<uint, ItemAmount>{
            {1,  new ItemAmount(existingProduct, 2)}
        };
        var holder = new ItemsHolder(TestUtils.Repo.InMemory(ref store));
        #endregion

        #region Execute
        holder.Add(existingProduct, 3);
        #endregion

        #region Validate
        Assert.NotEmpty(store);
        var contains = store.TryGetValue(existingProduct.Code, out var target);
        Assert.True(contains);
        Assert.Equal(5, target.Amount);
        Assert.Equal(existingProduct, target.Item);
        #endregion
    }
}
}