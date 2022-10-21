using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Products
{
    public class Scooter : Product
    {
        public override int MaterialsNeeded => 5;

        public override int EmployeesNeeded => 2;

        public override int TimeToBuild => 2;

        public override string Name => "scooter";

        public override int SpaceToStock => 3;

        public override int Price => 3000;
    }
}
