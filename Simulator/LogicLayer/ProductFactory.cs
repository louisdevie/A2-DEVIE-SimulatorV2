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

            this._constructors.Add("bike", typeof (Bike).GetConstructor(Array.Empty<Type>())!);
            this._constructors.Add("car", typeof (Car).GetConstructor(Array.Empty<Type>())!);
            this._constructors.Add("scooter", typeof (Scooter).GetConstructor(Array.Empty<Type>())!);
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
