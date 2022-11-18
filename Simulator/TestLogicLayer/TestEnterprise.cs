namespace TestLogicLayer
{
    public class TestEnterprise
    {
        [Fact]
        public void TestBuyMaterials()
        {
            Enterprise e = new Enterprise();
            int money = e.Money;
            int materials = e.Materials;
            e.BuyMaterials();
            Assert.True(money > e.Money);
            Assert.True(materials < e.Materials);

            Assert.ThrowsAsync<NotEnoughMoney>(
                () =>{
                while (true)
                {
                    e.BuyMaterials();
                }
            });

        }

        [Fact]
        public void TestHire()
        {
            Enterprise e = new Enterprise();
            int nb = e.Employees;
            e.Hire();
            Assert.Equal(nb + 1, e.Employees);
        }

        [Fact]
        public void TestDismiss()
        {
            Enterprise e = new Enterprise();
            e.Hire();
            int nb = e.Employees;
            int money = e.Money;
            e.Dismiss();
            Assert.True(nb > e.Employees);
            Assert.True(money > e.Money);
        }

        [Fact]
        public void TestMakeProduct()
        {
            Enterprise e = new Enterprise();
            e.BuyMaterials();            
            int free = e.FreeEmployees;
            int mat = e.Materials;
            e.MakeProduct("bike");
            e.UpdateProductions();
            Assert.True(e.FreeEmployees < free);
            Assert.True(e.Materials < mat);
            Assert.Equal(1, e.GetProduction("bike"));
        }

        [Fact]
        public void TestPayEmployees()
        {
            Enterprise e = new Enterprise();
            e.Hire();
            int money = e.Money;
            e.PayEmployees();
            Assert.True(money > e.Money);
            Assert.ThrowsAsync<NotEnoughMoney>(() =>
            {
                while (true) e.PayEmployees();
            });
        }

        [Fact]
        public void TestTimer()
        {
            Enterprise e = new Enterprise();
            int initialMoney = e.Money;
            e.Hire();
            e.Hire();
            Thread.Sleep(Constants.MONTH_TIME * 2);
            Assert.True(e.Money < initialMoney);
        }

        [Fact]
        public void TestNotifications()
        {
            Enterprise e = new Enterprise();

            MockObserver obs = new MockObserver();
            e.Register(obs);

            Assert.False(obs.Notified);

            e.BuyMaterials();
            Assert.Equal(obs.Materials, e.Materials);

            Assert.True(obs.Notified);
        }
    }
}