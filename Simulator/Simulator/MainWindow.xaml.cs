using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LogicLayer.Enterprise enterprise;
        private Timer timerSecond;
        private Timer timerMonth;
        private Timer timerWeek;
        public MainWindow()
        {
            InitializeComponent();
            enterprise = new LogicLayer.Enterprise();
            DataContext = enterprise;
            timerSecond = new Timer(TimerSecondTick);
            timerSecond.Change(0, LogicLayer.Constants.TIME_SLICE); 
            timerMonth = new Timer(TimerMonthTick);
            timerMonth.Change(0, LogicLayer.Constants.MONTH_TIME);
            timerWeek = new Timer(TimerWeekTick);
            timerWeek.Change(0, LogicLayer.Constants.WEEK_TIME);
        }

        private void TimerSecondTick(object? data)
        {
            Dispatcher.Invoke(() =>
            {
                // every second, to update screen
                UpdateScreen();
            });
            
        }

        private void TimerWeekTick(object? data)
        {
            Dispatcher.Invoke(() =>
            {
                // nothing to do every week...
            });
        }

        private void TimerMonthTick(object? data)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    enterprise.PayEmployees();
                    enterprise.UpdateClients();
                }
                catch (LogicLayer.NotEnoughMoney)
                {
                    timerSecond.Dispose();
                    timerMonth.Dispose();

                    MessageBox.Show("Not enough money to pay employees !");
                    EndOfSimulation();
                }
            });
            
            
        }

        private void EndOfSimulation()
        {
            MessageBox.Show("END OF SIMULATION");
            Close();
        }

        private void UpdateScreen()
        {
            enterprise.UpdateProductions();
            enterprise.UpdateBuying();
            
            totalStock.Content = enterprise.TotalStock.ToString()+" %";
            materials.Content = enterprise.Materials.ToString();
            employees.Content = enterprise.FreeEmployees.ToString()+"/"+enterprise.Employees.ToString();
            money.Content = enterprise.Money.ToString("C");

            bikesProd.Content = enterprise.GetProduction("bike").ToString();
            scootsProd.Content = enterprise.GetProduction("scooter").ToString();
            carsProd.Content = enterprise.GetProduction("car").ToString();

            bikeStock.Content = enterprise.GetStock("bike").ToString();
            scootStock.Content = enterprise.GetStock("scooter").ToString();
            carStock.Content = enterprise.GetStock("car").ToString();

            bikeAsk.Content = enterprise.GetAskClients("bike").ToString();
            scootAsk.Content = enterprise.GetAskClients("scooter").ToString();
            carAsk.Content = enterprise.GetAskClients("car").ToString();
        }

        private void BuyMaterials(object sender, RoutedEventArgs e)
        {
            try
            {
                enterprise.BuyMaterials();
                UpdateScreen();
            }
            catch(LogicLayer.NotEnoughMoney)
            {
                MessageBox.Show("Not enough money to buy materials !");
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void Hire(object sender, RoutedEventArgs e)
        {
            try
            {
                enterprise.Hire();
                UpdateScreen();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void Dismiss(object sender, RoutedEventArgs e)
        {
            try
            {
                enterprise.Dismiss();
                UpdateScreen();
            }
            catch(LogicLayer.NoEmployee)
            {
                MessageBox.Show("There is no employee to dismiss");
            }
            catch(LogicLayer.NotEnoughMoney)
            {
                MessageBox.Show("There is not enough money to puy dismiss bonus");
            }
            catch(LogicLayer.EmployeeWorking)
            {
                MessageBox.Show("You can't dismiss no : employees working");
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void BuildProduct(string s)
        {
            try
            {
                enterprise.MakeProduct(s);
                UpdateScreen();
            }
            catch (LogicLayer.ProductUnknown)
            {
                MessageBox.Show("I don't know how to make " + s);
            }
            catch (LogicLayer.NotEnoughMaterials)
            {
                MessageBox.Show("You do not have suffisent materials to build a "+s);
            }
            catch (LogicLayer.NoEmployee)
            {
                MessageBox.Show("You do not have enough employees to build a "+s);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
        private void BuildBike(object sender, RoutedEventArgs e)
        {
            BuildProduct("bike");
        }

        private void BuildScooter(object sender, RoutedEventArgs e)
        {
            BuildProduct("scooter");
        }

        private void BuildCar(object sender, RoutedEventArgs e)
        {
            BuildProduct("car");
        }
    }
}
