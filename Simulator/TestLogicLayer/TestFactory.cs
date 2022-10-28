using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLogicLayer
{
    public class TestFactory
    {
        [Fact]
        public void TestCreateProduct()
        {
            ProductFactory factory = new ProductFactory();
            Initialise.InitialiseProductFactory(factory);

            Product bike = factory.CreateProduct("bike");
            Assert.IsType<Bike>(bike);

            Product scooter = factory.CreateProduct("scooter");
            Assert.IsType<Scooter>(scooter);

            Product car = factory.CreateProduct("car");
            Assert.IsType<Car>(car);
        }
    }
}
