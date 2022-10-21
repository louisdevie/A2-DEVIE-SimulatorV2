using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Products
{
    public class Car : Product
    {
        public override int MaterialsNeeded => 20;

        public override int EmployeesNeeded => 10;

        public override int TimeToBuild => 8;

        public override string Name => "car";

        public override int SpaceToStock => 10;

        public override int Price => 15000;
    }
}
