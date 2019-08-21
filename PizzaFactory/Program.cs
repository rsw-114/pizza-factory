using Microsoft.Extensions.DependencyInjection;

namespace PizzaFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddTransient<IOrderPizzas, OrderPizzas>();
            var serviceProvider = collection.BuildServiceProvider();
            var service = serviceProvider.GetService<IOrderPizzas>();
            service.StartOrder();
            serviceProvider.Dispose();            
        }
    }
}
