using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Products
{
    public class Bike : Product
    {
        public override int MaterialsNeeded => 2;
        
        public override int EmployeesNeeded => 1;

        public override int TimeToBuild => 1;

        public override string Name => "bike";

        public override int SpaceToStock => 1;

        public override int Price => 600;
    }
}
