using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// A production of the workshop
    /// </summary>
    internal class Production
    {
        private Product product;
        private bool done;
        /// <summary>
        /// Init the production
        /// </summary>
        /// <param name="product">the product to build</param>
        public Production(Product product)
        {
            this.product = product;
            done=false;
            Timer timer = new Timer(ProdDone);
            timer.Change(product.TimeToBuild*Constants.TIME_SLICE, Timeout.Infinite);
        }

        private void ProdDone(object? state)
        {
            done = true;
        }

        /// <summary>
        /// Tell if the production is done
        /// </summary>
        public bool IsDone { get => done; }

        /// <summary>
        /// Gets the product 
        /// </summary>
        public Product Product { get => product; }
        
    }
}
