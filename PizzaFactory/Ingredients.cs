using System.Collections.Generic;

namespace PizzaFactory
{
    public class Ingredients
    {
        public static Dictionary<int, string> Bases = new Dictionary<int, string>
        {
            {1, "Deep Pan" },
            {2, "Stuffed Crust" },
            {3, "Thin and Crispy"}
        };

        public static Dictionary<int, string> Toppings = new Dictionary<int, string>
        {
            {1, "Ham and Mushroom" },
            {2, "Pepperoni" },
            {3, "Vegetable"}
        };
    }
}
