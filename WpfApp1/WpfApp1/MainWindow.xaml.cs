using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WpfApp1
{
    public class MyDataContext : INotifyPropertyChanged
    {
        private int someValue;
        private string someText;
        private bool alert;

        public MyDataContext()
        {
            someValue = 1;
            someText = "Test";
        }

        public bool Alert
        {
            get { return alert; }
            set {
                if (this.alert != value)
                {
                    this.alert = value;
                    OnPropertyChanged("Alert");
                }                
            }
        }

        public string SomeText
        {
            get
            {
                return this.someText;
            }

            set
            {
                if (this.someText != value)
                {
                    this.someText = value;
                    OnPropertyChanged("SomeText");
                }
            }
        }
        
        public int SomeValue
        {
            get { return this.someValue; }
            set
            {
                if (this.someValue != value)
                {
                    this.someValue = value;
                    OnPropertyChanged("SomeValue");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MyDataSource
    {
        public ObservableCollection<MyDataContext> Values { get; set; }

        public MyDataSource()
        {
            this.Values = new ObservableCollection<MyDataContext>();

            for (int i = 0; i < 5; i++)
            {
                this.Values.Add(new MyDataContext() { SomeText = "Test" + i, SomeValue = i });
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<MyDataContext> myData = new List<MyDataContext>();

        private SlowSource slowSource = new SlowSource();

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 5; i++)
            {
                myData.Add(new MyDataContext() { SomeText = "Test" + i });
            }
            this.myListBox.DataContext = myData;
            this.slowSourceTestTextBlock.DataContext = this.slowSource;
            this.slowSourceTestTextBlock2.DataContext = this.slowSource;
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hi");
        }

        private void incButton_Click(object sender, RoutedEventArgs e)
        {
            var obj = this.Resources["myDataContext"] as MyDataContext;
            if (obj != null)
            {
                obj.SomeValue += 1;

                obj.Alert = (obj.SomeValue % 3 == 0);
            }
        }

        private void listAddButton_Click(object sender, RoutedEventArgs e)
        {
            (this.Resources["myDataProvider"] as MyDataSource).Values.Add(new MyDataContext { SomeText = "Added", SomeValue = DateTime.Now.Second });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.myProgressBar.Value += 1;
            if (myProgressBar.Value >= myProgressBar.Maximum)
            {
                myProgressBar.Value = myProgressBar.Minimum;
            }
        }

        private void dispatcherTestButton_Click(object sender, RoutedEventArgs e)
        {
            //Approach #1 with v1 Asynchronous Pattern
            //ThreadPool.QueueUserWorkItem(DoSlowWork); 

            //Approach with async / await
            this.Dispatcher.Invoke(async () =>
            {
                this.dispatcherTestTextBlock.Text = await DoSlowWorkAsync();
            });
        }

        private void DoSlowWork(object state)
        {
            string data = (new SlowThing()).Data;
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<string>(DoUpdateUiThread), data);
        }

        private void DoUpdateUiThread(string data)
        {
            this.dispatcherTestTextBlock.Text = data;
        }

        private Task<string> DoSlowWorkAsync()
        {
            return Task.Factory.StartNew<string>(() => (new SlowThing().Data));
        }

        private void slowSourceTestButton_Click(object sender, RoutedEventArgs e)
        {
            slowSource.FetchNewData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var waveData = new WaveData();
            waveData.Show();
        }
    }
}
