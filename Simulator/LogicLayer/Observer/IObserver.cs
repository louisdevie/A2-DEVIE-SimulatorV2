using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Observer
{
    public interface IObserver
    {
        void OnMoneyChanged(int money);
    }
}
