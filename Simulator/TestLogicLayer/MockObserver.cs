using LogicLayer.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLogicLayer
{
    internal class MockObserver : IObserver
    {
        public int Materials { get; private set; }
        public bool Notified { get; private set; }

        public MockObserver()
        {
            Materials = -1;
            Notified = false;
        }

        public void OnEmployeesChanged(int free, int total) { }
            
        public void OnMaterialsChanged(int materials)
        {
            this.Materials = materials;
            this.Notified = true;
        }

        public void OnMoneyChanged(int money) { }

        public void OnStockChanged(int stock) { }
    }
}
