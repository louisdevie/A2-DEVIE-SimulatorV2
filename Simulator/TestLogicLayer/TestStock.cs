using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLogicLayer
{
    public class TestStock
    {
        [Fact]
        public void TestAdd()
        {
            Stock s = new Stock();
            Product pcar = new LogicLayer.Products.Car();
            Product pbike = new LogicLayer.Products.Bike();
            s.Add(pcar);
            s.Add(pbike);
            int val = pcar.SpaceToStock + pbike.SpaceToStock;
            Assert.Equal(1, s.GetNbOfType("bike"));
            Assert.Equal(1, s.GetNbOfType("car"));
            Assert.Equal(0, s.GetNbOfType("scooter"));
            Assert.Equal(100 * val / Constants.MAXSTOCK, s.TotalStock);

            Assert.ThrowsAsync<UnableToStock>(() =>
            {
                while (true) s.Add(pcar);
            });
        }
    }
}
