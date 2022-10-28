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

        static internal void InitialiseClientService(ClientService clientService)
        {
            clientService.RegisterProduct("bike", 20);
            clientService.RegisterProduct("scooter", 14);
            clientService.RegisterProduct("car", 10);
        }
    }
}
