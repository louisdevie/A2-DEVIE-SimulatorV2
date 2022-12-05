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
    public partial class MainWindow : Window, LogicLayer.Observer.IObserver
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
            timerWeek = new Timer(TimerWeekTick);
            timerWeek.Change(0, LogicLayer.Constants.WEEK_TIME);

            this.enterprise.Register(this);
            this.enterprise.Init();

            this.InitPanelBuild();
            this.InitPanelProd();
            this.InitPanelStock();
            this.InitPanelNeeds();
        }
        private void InitPanelBuild()
        {
            foreach (String type in this.enterprise.NamesOfProducts)
            {
                // create a button, with a static style
                Button button = new Button();
                button.Style = Application.Current.TryFindResource("resBtn") as Style;
                // when the button is clicked, we call BuildProduct with the good type
                button.Click += (sender, args) => { BuildProduct(type); };
                // create the stack panel inside the button
                var panel = new StackPanel();
                button.Content = panel;
                // create an image with resources, and file with same name than product, and add to the panel
                Image image = new Image();
                string path =
                string.Format("pack://application:,,,/Simulator;component/Images/{0}.png", type);
                BitmapImage bmp = new BitmapImage(new Uri(path));
                image.Source = bmp;
                panel.Children.Add(image);
                // create a label, with the good style and add to the panel
                Label label = new Label();
                label.Content = "Build a " + type;
                label.Style = Application.Current.TryFindResource("legend") as Style;
                panel.Children.Add(label);
                // add the button to the parent panel 
                panelBuild.Children.Add(button);
            }
        }

        private void InitPanel(StackPanel panel, String labelNameSuffix)
        {
            foreach (String type in this.enterprise.NamesOfProducts)
            {
                Border border = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(2),
                };

                StackPanel stackPanel = new StackPanel { };

                String uri = String.Format(
                    "pack://application:,,,/Simulator;component/Images/{0}.png",
                    type
                );
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(uri)),
                    Width = 40,
                };
                stackPanel.Children.Add(image);

                Label label = new Label
                {
                    Name = type + labelNameSuffix,
                    Content = "0",
                    Style = Application.Current.TryFindResource("legend") as Style,
                };
                stackPanel.Children.Add(label);

                border.Child = stackPanel;

                panel.Children.Add(border);
            }
        }

        private void InitPanelProd() => InitPanel(panelProd, "sProd");

        private void InitPanelStock() => InitPanel(panelStock, "sStock");

        private void InitPanelNeeds() => InitPanel(panelNeeds, "sNeeds");

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

        private void EndOfSimulation()
        {
            MessageBox.Show("END OF SIMULATION");
            Close();
        }

        private void UpdateScreen()
        {
            enterprise.UpdateProductions();
            enterprise.UpdateBuying();
        }

        private void BuyMaterials(object sender, RoutedEventArgs e)
        {
            try
            {
                enterprise.BuyMaterials();
                UpdateScreen();
            }
            catch (LogicLayer.NotEnoughMoney)
            {
                MessageBox.Show("Not enough money to buy materials !");
            }
            catch (Exception x)
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
            catch (LogicLayer.NoEmployee)
            {
                MessageBox.Show("There is no employee to dismiss");
            }
            catch (LogicLayer.NotEnoughMoney)
            {
                MessageBox.Show("There is not enough money to puy dismiss bonus");
            }
            catch (LogicLayer.EmployeeWorking)
            {
                MessageBox.Show("You can't dismiss no : employees working");
            }
            catch (Exception x)
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
                MessageBox.Show("You do not have suffisent materials to build a " + s);
            }
            catch (LogicLayer.NoEmployee)
            {
                MessageBox.Show("You do not have enough employees to build a " + s);
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

        public void OnMoneyChanged(int money)
        {
            Dispatcher.Invoke(() =>
            {
                this.money.Content = money.ToString("C");
            });
        }

        public void OnStockChanged(int stock)
        {
            totalStock.Content = stock.ToString() + " %";
            foreach (String type in this.enterprise.NamesOfProducts)
            {
                string name = type + "sStock";
                var maybeLabel = UIChildFinder.FindChild(panelStock, name, typeof(Label));
                if (maybeLabel is Label label)
                {
                    label.Content = enterprise.GetStock(type).ToString();
                }
            }
        }

        public void OnMaterialsChanged(int materials)
        {
            this.materials.Content = materials.ToString();
        }

        public void OnEmployeesChanged(int free, int total)
        {
            employees.Content = free.ToString() + "/" + total.ToString();
        }

        public void OnClientNeedsChanged(string type, int needs)
        {
            string name = type + "sNeeds";
            Dispatcher.Invoke(() =>
            {
                var maybeLabel = UIChildFinder.FindChild(panelNeeds, name, typeof(Label));
                if (maybeLabel is Label label)
                {
                    label.Content = needs.ToString();
                }
            });
        }

        public void OnProductionChanged(string type)
        {
            string name = type + "sProd";
            Dispatcher.Invoke(() =>
            {
                var test = UIChildFinder.FindChild(panelProd, name, typeof(Label));
                if (test is Label label)
                {
                    label.Content = enterprise.GetProduction(type).ToString();
                }
            });
        }
    }
}
