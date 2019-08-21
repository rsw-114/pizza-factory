using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaFactory
{
    public interface IOrderPizzas
    {
        double CalculateCookingTime(int baseTime, Pizza pizza);
        Pizza CookPizza(Pizza pizza, int cookingTime);
        void StartOrder();
    }

    public class OrderPizzas : IOrderPizzas
    {
        public double CalculateCookingTime(int baseTime, Pizza pizza)
        {
            var toppingWithoutSpaces = pizza.Toppping.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .Count();

            var additionalCookingTime = toppingWithoutSpaces * 100;
            return (pizza.CookingMultiplier * baseTime) + additionalCookingTime;
        }

        public Pizza CookPizza(Pizza pizza, int cookingTime)
        {
            if (!string.IsNullOrEmpty(pizza.Toppping) && !pizza.Cooked)
            {
                Console.WriteLine($"Cooking {pizza.Base} {pizza.Toppping}.");
                Task.Delay(cookingTime).Wait();
                pizza.Cooked = true;
            }
            return pizza;
        }

        public void StartOrder()
        {            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            var fileName = configuration.GetSection("PizzaFile").Value;
            
            int.TryParse(configuration.GetSection("Pizzas to cook").Value, out int pizzasToCook);
            int.TryParse(configuration.GetSection("Base cooking time").Value, out int baseCookingTime);
            int.TryParse(configuration.GetSection("Interval").Value, out int interval);

            using (StreamWriter file =
            new StreamWriter(File.Create(fileName)))
            {
                Console.WriteLine($"Starting order of {pizzasToCook} pizzas.");
                for (int i = 1; i <= pizzasToCook; i++)
                {
                    var rnd = new Random();
                    var toppingSelection = rnd.Next(1, 4);
                    var selectedTopping = Ingredients.Toppings[toppingSelection];

                    var baseSelection = rnd.Next(1, 4);
                    var selectedBase = Ingredients.Bases[baseSelection];

                    var pizza = new Pizza
                    {
                        Base = selectedBase,
                        Toppping = selectedTopping,
                        CookingMultiplier = SetCookingMultiplier(selectedBase)
                    };

                    var cookingTime = CalculateCookingTime(baseCookingTime, pizza);
                    CookPizza(pizza, (int)cookingTime);
                    Console.WriteLine($"{pizza.Base} {pizza.Toppping} cooked. {pizzasToCook - i} pizzas remaining.");
                    file.WriteLine($"{i}: {pizza.Base} {pizza.Toppping}");
                    Task.Delay(interval).Wait();
                }
            }
        }

        private double SetCookingMultiplier(string selectedBase)
        {
            switch (selectedBase)
            {
                case "Deep Pan":
                    {
                        return 2;
                    }
                case "Stuffed Crust":
                    {
                        return 1.5;
                    }
                case "Thin and Crispy":
                    {
                        return 1;
                    }
                default:
                    return 0;
            }
        }
    }
}
