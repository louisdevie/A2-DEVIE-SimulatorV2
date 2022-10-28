using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Observer
{
    public class Subject
    {
        List<IObserver> _observers;


        public Subject()
        {
            this._observers = new List<IObserver>(); 
        }

        public void Register(IObserver observer) => this._observers.Add(observer);

        public void Unregister(IObserver observer) => this._observers.Remove(observer);

        protected void NotifyMoneyChanged(int money)
        {
            foreach (IObserver observer in this._observers)
            {
                observer.OnMoneyChanged(money);
            }
        }
    }
}
