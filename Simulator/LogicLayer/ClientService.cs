using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Part of company, who deal with clients needs
    /// </summary>
    internal class ClientService
    {
        private Random r;
        private Dictionary<string, int> needs;
        private Dictionary<string, int> proba;

        public ClientService()
        {
            needs = new Dictionary<string, int>();
            proba = new Dictionary<string, int>();
            r = new Random();
        }

        public void RegisterProduct(String name, int proba)
        {
            this.proba.Add(name, proba);
            this.needs.Add(name, 0);
        }

        private int ProbaToClients(int proba)
        {
            return (int)(r.NextDouble() * proba);
        }
        public void UpdateClients()
        {
            // the values are the probability new clients want a type...
            foreach (var proba in this.proba)
            {
                needs[proba.Key] += ProbaToClients(proba.Value);
            }
        }

        /// <summary>
        /// Get clients needs
        /// </summary>
        /// <param name="type">type of product</param>
        /// <returns>number of potential clients</returns>
        /// <exception cref="ProductUnknown">If product is not known</exception>
        public int GetAskFor(string type)
        {            
            if (!needs.ContainsKey(type))
                throw new ProductUnknown();

            return needs[type];
        }

        /// <summary>
        /// Tells if a client want to buy the product
        /// </summary>
        /// <param name="type">kind of product</param>
        /// <returns>true if one client want to buy (and can buy)</returns>
        /// <exception cref="ProductUnknown">If type unknown</exception>
        public bool WantToBuy(string type)
        {
            if (!needs.ContainsKey(type))
                throw new ProductUnknown();
            return (r.NextDouble() * needs[type])*10 > 1;
        }

        /// <summary>
        /// A product is bought, so a client do not want to buy anymore
        /// </summary>
        /// <param name="type"></param>
        public void Buy(string type)
        {
            needs[type] -= 10;
            if (needs[type] < 0) needs[type] = 0;
        }
    }
}
