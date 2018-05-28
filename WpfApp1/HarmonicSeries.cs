using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Amplitude
    {
        private double val = 0;

        public event EventHandler<EventArgs> ValueChanged;

        public double Value
        {
            get { return val; }
            set
            {
                if (val != value)
                {
                    val = value;
                    OnValueChanged();
                }
            }
        }

        private void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new EventArgs());
            }
        }
    }

    class HarmonicSeries : INotifyPropertyChanged
    {
        private readonly Amplitude[] amplitudeValues;
        private readonly double[] outputValues;

        public event PropertyChangedEventHandler PropertyChanged;

        public Amplitude[] Amplitudes
        {
            get { return amplitudeValues; }
        }

        public double[] Output
        {
            get { return outputValues; }
        }

        public HarmonicSeries(int steps)
        {
            amplitudeValues = new Amplitude[steps];
            outputValues = new double[steps];

            for (int i = 0; i < steps; i++)
            {
                var ampl = new Amplitude { Value = 0 };
                ampl.ValueChanged += Ampl_ValueChanged;
                amplitudeValues[i] = ampl;
            }
        }

        private void Ampl_ValueChanged(object sender, EventArgs e)
        {
            RecalculateOutput();
            OnPropertyChanged("Output");
        }

        private void RecalculateOutput()
        {
            for (int i = 0; i < amplitudeValues.Length; i++)
            {
                outputValues[i] = amplitudeValues[i].Value;
            }
        }

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
