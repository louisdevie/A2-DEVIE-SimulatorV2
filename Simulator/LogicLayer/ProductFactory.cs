using LogicLayer.Products;
using System.Reflection;

namespace LogicLayer
{
    public class ProductFactory
    {
        private Dictionary<string, ConstructorInfo> _constructors;

        /// <summary>
        /// Initiaises a new <see cref="ProductFactory"/>.
        /// </summary>
        public ProductFactory()
        {
            this._constructors = new Dictionary<string, ConstructorInfo>();
        }

        /// <summary>
        /// Register a product class
        /// </summary>
        /// <param name="name">The name of the product type</param>
        /// <param name="type">The product class</param>
        public void Add(String name, Type type)
        {
            if (type.IsSubclassOf(typeof(Product)))
            {
                this._constructors.Add(name, type.GetConstructor(Array.Empty<Type>())!);
            }
            else
            {
                throw new ArgumentException("The type must inherit from Product");                
            }
        }

        /// <summary>
        /// The available product types.
        /// </summary>
        public IEnumerable<String> Types => this._constructors.Keys;

        /// <summary>
        /// Create a new <see cref="Product"/>
        /// </summary>
        /// <param name="type">The type of product to create.</param>
        /// <returns>A new <see cref="Product"/></returns>
        public Product CreateProduct(String type)
        {
            if (this._constructors.ContainsKey(type))
            {
                return (Product)this._constructors[type].Invoke(Array.Empty<object>());
            }
            else
            {
                throw new Exception($"No constructor for {type}");
            }
        }
    }
}
