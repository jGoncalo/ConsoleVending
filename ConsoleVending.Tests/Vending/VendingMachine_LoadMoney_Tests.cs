using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Enums;
using ConsoleVending.Protocol.Exceptions;
using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Vending;
using Moq;
using Xunit;

namespace ConsoleVending.Tests.Vending
{
    public class VendingMachine_LoadMoney_Tests
    {
        [Fact]
        public void InMaintenance()
        {
            #region Setup
            var currencyMock = new Mock<ICurrencyHolder>();
            IReadOnlyTransaction? capturedTransaction = null;
            currencyMock.Setup(it => it.AddCurrency(It.IsAny<IReadOnlyTransaction>()))
                .Callback<IReadOnlyTransaction>(transaction => {
                    capturedTransaction = transaction;
                });
            
            var machine = new VendingMachine(currencyMock.Object, new Mock<IItemsHolder>().Object);
            #endregion
            
            #region Execute
            var transaction = new Transaction()
                .Push(Denomination.TwoPenny, 10);
            machine.InMaintenance = true;
            machine.LoadMoney(transaction);
            #endregion

            #region Validate
            Assert.NotNull(capturedTransaction);
            Assert.Equal(transaction, capturedTransaction);
            #endregion
        }
        [Fact]
        public void OutOfMaintenance()
        {
            #region Setup
            var currencyMock = new Mock<ICurrencyHolder>();
            IReadOnlyTransaction? capturedTransaction = null;
            currencyMock.Setup(it => it.AddCurrency(It.IsAny<IReadOnlyTransaction>()))
                .Callback<IReadOnlyTransaction>(transaction => {
                    capturedTransaction = transaction;
                });
            
            var machine = new VendingMachine(currencyMock.Object, new Mock<IItemsHolder>().Object);
            #endregion
            
            #region Execute
            var transaction = new Transaction()
                .Push(Denomination.TwoPenny, 10);
            machine.InMaintenance = false;
            var exception = Assert.Throws<InOperationException>(() => machine.LoadMoney(transaction));
            #endregion

            #region Validate
            Assert.Equal("Operation is not possible while in operation", exception.Message);
            #endregion
        }
    }
}