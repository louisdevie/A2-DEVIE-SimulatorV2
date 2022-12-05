using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Products
{
    public class EScooter : Product
    {
        public override int MaterialsNeeded => 4;
        
        public override int EmployeesNeeded => 2;

        public override int TimeToBuild => 2;

        public override string Name => "telec";

        public override int SpaceToStock => 2;

        public override int Price => 1000;
    }
}
