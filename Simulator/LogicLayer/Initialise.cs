using LogicLayer.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    static public class Initialise
    {
        static public void InitialiseProductFactory(ProductFactory factory)
        {
            factory.Add("bike", typeof(Bike));
            factory.Add("scooter", typeof(Scooter));
            factory.Add("car", typeof(Car));
        }
    }
}
