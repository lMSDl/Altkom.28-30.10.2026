using AutoFixture;
using Moq;

namespace Shop.Tests
{
    public class OrderProcessorTests
    {
        [Fact]
        public void ProcessOrder_StockIsInsufficient_False()
        {
            //Arrange
            var orderRepository = new Mock<IOrderRepository>();
            var inventoryService = new Mock<IInventoryService>();
            var paymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(orderRepository.Object, inventoryService.Object, paymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Create<Order>();
            var cardNumber = fixture.Create<string>();

            //inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var lastItem = order.Items.Last();

            //var otherItems = order.Items.Take(order.Items.Count - 1).ToList();
            //otherItems.ForEach(item => inventoryService.Setup(x => x.CheckStock(item.ProductId, item.Quantity)).Returns(true).Verifiable(Times.Once));
            //inventoryService.Setup(x => x.CheckStock(lastItem.ProductId, lastItem.Quantity)).Returns(false).Verifiable(Times.Once);

            inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            inventoryService.Setup(x => x.CheckStock(lastItem.ProductId, lastItem.Quantity)).Returns(false);

            //Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            //Assert
            Assert.False(result);

            //inventoryService.VerifyAll();
            inventoryService.Verify(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(order.Items.Count));

            inventoryService.Verify(x => x.ReserveStock(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            orderRepository.Verify(x => x.SaveOrder(It.IsAny<Order>()), Times.Never);
            paymentService.Verify(x => x.ProcessPayment(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void ProcessOrder_PaymentFailed_False()
        {
            //Arrange
            var orderRepository = new Mock<IOrderRepository>();
            var inventoryService = new Mock<IInventoryService>();
            var paymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(orderRepository.Object, inventoryService.Object, paymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Create<Order>();
            var cardNumber = fixture.Create<string>();

            inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            paymentService.Setup(x => x.ProcessPayment(cardNumber, It.IsAny<decimal>())).Returns(false);

            //Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            //Assert
            Assert.False(result);
            paymentService.Verify(x => x.ProcessPayment(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);

            inventoryService.Verify(x => x.ReserveStock(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            orderRepository.Verify(x => x.SaveOrder(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public void ProcessOrder_ConditionsMet_True()
        {
            //Arrange
            var orderRepository = new Mock<IOrderRepository>();
            var inventoryService = new Mock<IInventoryService>();
            var paymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(orderRepository.Object, inventoryService.Object, paymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Create<Order>();
            var cardNumber = fixture.Create<string>();

            inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            paymentService.Setup(x => x.ProcessPayment(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            //Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            //Assert
            Assert.True(result);

            //inventoryService.Verify(x => x.ReserveStock(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(order.Items.Count));
            inventoryService.Verify(x => x.ReserveStock(It.IsIn(order.Items.Select(i => i.ProductId)), It.IsIn(order.Items.Select(i => i.Quantity))), Times.Exactly(order.Items.Count));

            orderRepository.Verify(x => x.SaveOrder(order), Times.Once);
        }

        public static IEnumerable<object[]> OrderItemsData => 
            [
                [new OrderItem[] { new() { ProductId = 1, Quantity = 2, UnitPrice = 10m }, new() { ProductId = 2, Quantity = 1, UnitPrice = 20m } }, 40m],
                [new OrderItem[] { new() { ProductId = 1, Quantity = 3, UnitPrice = 15m }, new() { ProductId = 2, Quantity = 2, UnitPrice = 25m } }, 95m],
                [new OrderItem[] { new() { ProductId = 1, Quantity = 1, UnitPrice = 5m }, new() { ProductId = 2, Quantity = 4, UnitPrice = 10m } }, 45m]
            ];

        [Theory]
        [MemberData(nameof(OrderItemsData))]
        public void ProcessOrder_CorrectTotalPaymentAmount(IEnumerable<OrderItem> items, decimal expectedTotal)
        {
            //Arrange
            var orderRepository = new Mock<IOrderRepository>();
            var inventoryService = new Mock<IInventoryService>();
            var paymentService = new Mock<IPaymentService>();

            var orderProcessor = new OrderProcessor(orderRepository.Object, inventoryService.Object, paymentService.Object);

            var fixture = new Fixture();
            var order = fixture.Build<Order>().With(x => x.Items, [..items]).Create();
            var cardNumber = fixture.Create<string>();

            inventoryService.Setup(x => x.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            //Act
            var result = orderProcessor.ProcessOrder(order, cardNumber);

            //Assert
            paymentService.Verify(x => x.ProcessPayment(cardNumber, expectedTotal));
        }
    }
}
