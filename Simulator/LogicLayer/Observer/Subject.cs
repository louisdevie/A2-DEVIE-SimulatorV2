using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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

        protected void NotifyStockChanged(int stock)
        {
            foreach (IObserver observer in this._observers)
            {
                observer.OnStockChanged(stock);
            }
        }

        protected void NotifyMaterialsChanged(int materials)
        {
            foreach (IObserver observer in this._observers)
            {
                observer.OnMaterialsChanged(materials);
            }
        }

        protected void NotifyEmployeesChanged(int free, int total)
        {
            foreach (IObserver observer in this._observers)
            {
                observer.OnEmployeesChanged(free, total);
            }
        }

        protected void NotifyClientNeedsChanged(String type, int needs)
        {
            foreach (IObserver observer in this._observers)
            {
                observer.OnClientNeedsChanged(type, needs);
            }
        }

        protected void NotifyProductionChanged(String type)
        {
            foreach (IObserver observer in this._observers)
            {
                observer.OnProductionChanged(type);
            }
        }
    }
}
