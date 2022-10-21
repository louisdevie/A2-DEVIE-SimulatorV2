using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class SimulatorException : Exception
    {
    }

    public class NotEnoughMoney : SimulatorException
    {

    }

    public class NoEmployee : SimulatorException
    {

    }

    public class UnableToBuild : SimulatorException { }
    public class ProductUnknown : UnableToBuild { }
    public class NotEnoughMaterials : UnableToBuild { }

    public class ProductionRunning : UnableToBuild { }

    public class UnableToStock : SimulatorException {}

    public class EmployeeWorking : SimulatorException { }

}
