using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Management of stocks
    /// </summary>
    public class Stock
    {
        private List<Product > products;

        /// <summary>
        /// Init the stock
        /// </summary>
        public Stock()
        {
            products = new List<Product>();
        }

        private int StockProduct
        {
            get
            {
                int stock = 0;
                foreach (Product product in products)
                {
                    stock += product.SpaceToStock;
                }
                return stock;
            }
        }
        /// <summary>
        /// Gets the ratio of stock taken (100=full stock)
        /// </summary>
        public int TotalStock 
        { 
            get
            {
                
                return 100 * StockProduct / Constants.MAXSTOCK;
            }
        } 

        /// <summary>
        /// Gets the number of products of a type
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>number of products in the stock</returns>
        public int GetNbOfType(string type)
        {
            return products.Count((p) => p.Name == type);
        }

        /// <summary>
        /// Add a product in the stock
        /// </summary>
        /// <param name="p">the product to stock</param>
        /// <exception cref="UnableToStock">if no space available</exception>
        public void Add(Product p)
        {
            if (StockProduct + p.SpaceToStock > Constants.MAXSTOCK)
                throw new UnableToStock();
            products.Add(p);
        }

        /// <summary>
        /// Remove a product from the stock
        /// </summary>
        /// <param name="p">the product to remove</param>
        public void Remove(Product p)
        {
            products.Remove(p);
        }

        /// <summary>
        /// Find a product in the stock
        /// </summary>
        /// <param name="type">the type of product</param>
        /// <returns>the first product found (if none, returns null)</returns>
        public Product? Find(string type)
        {            
            return products.Find((p) => p.Name == type);
        }
    }
}
