using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{
    class SlowSource : INotifyPropertyChanged
    {
        private int id = 1;
        private string myText = "Initial text";
        private string myText2 = "Initial text2";

        public event PropertyChangedEventHandler PropertyChanged;

        public string MyText
        {
            get
            {
                Debug.WriteLine("Get thread: " + Thread.CurrentThread.ManagedThreadId);
                return myText;
            }
            set
            {
                Debug.WriteLine("Set thread: " + Thread.CurrentThread.ManagedThreadId);
                if (myText != value)
                {
                    myText = value;
                    OnPropertyChanged("MyText");
                }                
            }
        }

        public string MyText2
        {
            get
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                Debug.WriteLine("Get2 thread: " + Thread.CurrentThread.ManagedThreadId);
                return myText2;
            }
            set
            {
                Debug.WriteLine("Set2 thread: " + Thread.CurrentThread.ManagedThreadId);
                if (myText2 != value)
                {
                    myText2 = value;
                    OnPropertyChanged("MyText2");
                }
            }
        }

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void FetchNewData()
        {
            ThreadPool.QueueUserWorkItem(delegate 
                                            {
                                                Debug.WriteLine("Worker thread: " + Thread.CurrentThread.ManagedThreadId);
                                                Thread.Sleep(TimeSpan.FromSeconds(5));

                                                string newValue = "Value " + Interlocked.Increment(ref id);
                                                this.MyText = newValue;
                                            });
        }
    }
}
