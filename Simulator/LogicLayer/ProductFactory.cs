using LogicLayer.Products;
using System.Reflection;

namespace LogicLayer
{
    internal class ProductFactory
    {
        private Dictionary<string, ConstructorInfo> _constructors;

        public ProductFactory()
        {
            this._constructors = new Dictionary<string, ConstructorInfo>();

            this._constructors.Add("bike", typeof (Bike).GetConstructor(Array.Empty<Type>())!);
            this._constructors.Add("car", typeof (Car).GetConstructor(Array.Empty<Type>())!);
            this._constructors.Add("scooter", typeof (Scooter).GetConstructor(Array.Empty<Type>())!);
        }

        public IEnumerable<String> Types => this._constructors.Keys;

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
