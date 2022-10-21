using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    internal class Workshop
    {
        private int nbEmployees;
        private List<Production> productions;
        
        /// <summary>
        /// Gets the number of employees who work in the workshop
        /// </summary>
        public int NbEmployees { get { return nbEmployees; } }

        public Workshop()
        {
            nbEmployees = 0;
            productions = new List<Production>();
        }

        /// <summary>
        /// Start the production of a product
        /// </summary>
        /// <param name="p">the product</param>
        public void StartProduction(Product p)
        {
            nbEmployees += p.EmployeesNeeded;
            productions.Add(new Production(p));
        }

        /// <summary>
        /// Gets the products terminated
        /// </summary>
        /// <returns>an array with all the products the workshop has build</returns>
        public Product[] ProductsDone()
        {
            List<Product> dones = new List<Product>();
            
            var tab = productions.FindAll((Production p) => p.IsDone);

            foreach(Production p in tab)
            {
                dones.Add(p.Product);
            }
            return dones.ToArray();
        }

        /// <summary>
        /// Remove the product (if is done)
        /// </summary>
        /// <param name="p">the product</param>
        /// <exception cref="ProductUnknown">If the product is not is the workshop</exception>
        /// <exception cref="ProductionRunning">IF the product is not terminated</exception>
        public void Remove(Product p)
        {
            var prod = productions.Find((pro) => pro.Product == p );
            if (prod == null)
                throw new ProductUnknown();
            if (prod.IsDone == false)
                throw new ProductionRunning();
            productions.Remove(prod);
            nbEmployees -= p.EmployeesNeeded; // free employees
        }

        /// <summary>
        /// Gets number of products in production
        /// </summary>
        /// <param name="type">the type of product</param>
        /// <returns>numbers building</returns>
        public int InProduction(string type)
        {
            return productions.Count((p) => (p.Product.Name == type) && (p.IsDone == false));
        }
    }
}
