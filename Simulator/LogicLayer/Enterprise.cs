using LogicLayer.Observer;

namespace LogicLayer
{
    /// <summary>
    /// Enterprise simulation
    /// </summary>
    public class Enterprise : Subject
    {
        private Timer _timer;

        #region associations
        private Workshop workshop;
        private Stock stock;
        private ClientService clients;
        private ProductFactory _factory;
        #endregion

        #region Properties
        private int money;
        /// <summary>
        /// Gets the amount of money that enterprise disposes
        /// </summary>
        public int Money
        {
            get => money;
            private set
            {
                money = value;
                this.NotifyMoneyChanged(this.Money);
            }
        }

        private int materials;
        /// <summary>
        /// Gets the amount of materials that enterprise disposes
        /// </summary>
        public int Materials
        {
            get => materials; 
            private set
            {
                materials = value;
                this.NotifyMaterialsChanged(this.Materials);
            }
        }

        private int employees;
        /// <summary>
        /// Gets the number of employees
        /// </summary>
        public int Employees
        {
            get => employees;
            private set
            {
                employees = value;
                this.NotifyEmployeesChanged(this.FreeEmployees, employees);
            }
        }

        /// <summary>
        /// Gets the number of free employees (they can work)
        /// </summary>
        public int FreeEmployees
        {
            get => Employees - EmployeesWorkshop;
        }
       
        /// <summary>
        /// Gets the number of employees working in the workshop
        /// </summary>
        public int EmployeesWorkshop { get => workshop.NbEmployees; } 

        /// <summary>
        /// Gets the total amount of stock
        /// </summary>
        public int TotalStock { get => stock.TotalStock; }

        public IEnumerable<String> NamesOfProducts => this._factory.Types;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialize the enterprise
        /// </summary>
        public Enterprise() : base()
        {
            workshop = new Workshop();
            Money = 300000;
            Employees = 4;
            Materials = 100;  
            stock = new Stock();

            clients = new ClientService();
            Initialise.InitialiseClientService(this.clients);

            this._factory = new ProductFactory();
            Initialise.InitialiseProductFactory(this._factory);

            this._timer = new Timer(this.EndOfMonth, new object(), 0, Constants.MONTH_TIME);
        }

        ~Enterprise()
        {
            this._timer.Dispose();
        }
        #endregion

        #region methods
        public void Init()
        {
            this.NotifyStockChanged(this.TotalStock);
            this.NotifyEmployeesChanged(this.FreeEmployees, this.Employees);
            this.NotifyMaterialsChanged(this.Materials);
            this.NotifyMoneyChanged(this.Money);
        }

        private void EndOfMonth(object? state)
        {
            this.PayEmployees();
            this.UpdateClients();
        }

        /// <summary>
        /// Buy some materials
        /// </summary>
        /// <exception cref="NotEnoughMoney">If insufisant funds</exception>
        public void BuyMaterials()
        {
            int cost = Constants.MATERIALS * Constants.COST_MATERIALS;
            if (Money < cost)
                throw new NotEnoughMoney();
            Money -= cost;
            Materials += Constants.MATERIALS;
        }

        /// <summary>
        /// Hire a new emloyee
        /// </summary>        
        public void Hire()
        {
            ++Employees;
        }

        /// <summary>
        /// DIsmiss an employee
        /// </summary>
        /// <exception cref="NoEmployee">If no employee to dismiss</exception>
        /// <exception cref="NotEnoughMoney">If not enough money to pay the bonus</exception>
        /// <exception cref="EmployeeWorking">If all employees worked, no dismiss is possible</exception>
        public void Dismiss()
        {
            if (Employees < 1) throw new NoEmployee();
            int cost = Constants.BONUS;
            if (Money < cost)
                throw new NotEnoughMoney();
            if (FreeEmployees < 1)
                throw new EmployeeWorking();
            Money -= cost;
            Employees--;
        }

        /// <summary>
        /// Start a product production
        /// </summary>
        /// <param name="type">a string identifying kind of product</param>
        /// <exception cref="ProductUnknown">the type is unknown</exception>
        /// <exception cref="NotEnoughMaterials">Not enough materials to build</exception>
        /// <exception cref="NoEmployee">Not enough employee to build</exception>
        public void MakeProduct(string type)
        {
            Product p = this._factory.CreateProduct(type);
            // test if the product can be build
            if (Materials < p.MaterialsNeeded)
                throw new NotEnoughMaterials();
            if (Employees - EmployeesWorkshop < p.EmployeesNeeded)
                throw new NoEmployee();

            Materials -= p.MaterialsNeeded; // consume materials
            // start the building...
            workshop.StartProduction(p);
            this.NotifyEmployeesChanged(this.FreeEmployees, this.Employees);
        }

        /// <summary>
        /// Update the productions & the stock
        /// </summary>
        /// <exception cref="UnableToStock">If stock is full</exception>
        public void UpdateProductions()
        {
            // update informations about productions
            var list = workshop.ProductsDone(); 
            // add finish products in stock
            foreach(var product in list)
            {
                stock.Add(product);
                workshop.Remove(product);
                this.NotifyStockChanged(this.TotalStock);
                this.NotifyEmployeesChanged(this.FreeEmployees, this.Employees);
            }

        }

        /// <summary>
        /// Get the numbers of products of a type workshop build
        /// </summary>
        /// <param name="v">kind of product</param>
        /// <returns>number of products building</returns>        
        public int GetProduction(string v)
        {
            return workshop.InProduction(v);
        }

        /// <summary>
        /// Gets the number of products stocked
        /// </summary>
        /// <param name="v">type of product</param>
        /// <returns>number stocked</returns>
        public int GetStock(string v)
        {
            return stock.GetNbOfType(v);
        }

        /// <summary>
        /// Pay all the employees
        /// </summary>
        /// <exception cref="NotEnoughMoney">if money is not enough !</exception>
        public void PayEmployees()
        {
            int cost = Employees * Constants.SALARY;
            if (cost > Money)
                throw new NotEnoughMoney();
            Money -= cost;
        }

        /// <summary>
        /// Update the buying status
        /// </summary>
        public void UpdateBuying()
        {            
            if(clients.WantToBuy("bike"))
            {
                TrySell("bike");
            }
            else if(clients.WantToBuy("scooter"))
            {
                TrySell("scooter");
            }
            else if(clients.WantToBuy("car"))
            {
                TrySell("car");
            }
        }

        private void TrySell(string type)
        {
            Product? p = stock.Find(type);
            if(p!=null)
            {
                stock.Remove(p);
                Money += p.Price;
                clients.Buy(type);
                this.NotifyStockChanged(this.TotalStock);
            }
        }

        /// <summary>
        /// update client needs
        /// </summary>
        public void UpdateClients()
        {            
            clients.UpdateClients();
            foreach (String type in this._factory.Types)
            {
                this.NotifyClientNeedsChanged(type, this.clients.GetAskFor(type));
            }
        }

        /// <summary>
        /// Get clients needs
        /// </summary>
        /// <param name="type">type of product clients wanted</param>
        /// <returns>number of potential clients</returns>
        /// <exception cref="ProductUnknown">If type unknown</exception>
        public int GetAskClients(string type)
        {
            return clients.GetAskFor(type);
        }
        #endregion



    }
}