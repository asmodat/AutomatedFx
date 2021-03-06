﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;


namespace Asmodat.FormsControls
{
    public class ThreadedTextBox : TextBox
    {
        public void InitMode()
        {
            this.TextChanged += ThreadedTextBox_TextChanged;
        }

        void ThreadedTextBox_TextChanged(object sender, EventArgs e)
        {
            this.ModeTest();
        }

        public enum Mode
        {
            None = 0,
            Text = 1,
            Integer = 3,
            Double = 4,
            DateTime = 5
        }

        private Mode _DisplayMode = Mode.Text;
        public Mode DisplayMode { get { return _DisplayMode; } set { _DisplayMode = value; } }

        public int IntegerDefault = 0;
        public int IntegerMin = int.MinValue;
        public int IntegerMax = int.MaxValue;

        public int Decimals = int.MaxValue;
        public double DoubleDefault = 0;
        public double DoubleMin = double.MinValue;
        public double DoubleMax = double.MaxValue;

        public string Unit = "";

        public void SetDoubleMode(double Min = double.MinValue, double Max = double.MaxValue, double Default = 0, int Decimals = 2, string Unit = "")
        {
            DoubleMin = Min;
            DoubleMax = Max;
            DoubleDefault = Default;
            this.Decimals = Decimals;
            DisplayMode = Mode.Double;
            this.Unit = Unit;
            this.InitMode();
        }

        public void SetIntegerMode(int Min = int.MinValue, int Max = int.MaxValue, int Default = 0)
        {
            IntegerMin = Min;
            IntegerMax = Max;
            IntegerDefault = Default;
            DisplayMode = Mode.Integer;
            this.InitMode();
        }



        public string GetText<TInvoker>(TInvoker Invoker) where TInvoker : Control
        {
            if (Invoker != null)
                return Abbreviate.FormsControls.Invoke<string, TInvoker>(Invoker, () => GetText<TInvoker>(null));

            this.ModeTest();
            return this.Text;
        }

        public int GetInt<TInvoker>(TInvoker Invoker) where TInvoker : Control
        {
            if (Invoker != null)
                return Abbreviate.FormsControls.Invoke<int, TInvoker>(Invoker, () => GetInt<TInvoker>(null));

            this.ModeTest();
            return int.Parse(this.GetTextValue);
        }

        public double GetDouble<TInvoker>(TInvoker Invoker) where TInvoker : Control
        {
            if (Invoker != null)
                return Abbreviate.FormsControls.Invoke<double, TInvoker>(Invoker, () => GetDouble<TInvoker>(null));

            this.ModeTest();
            return double.Parse(this.GetTextValue);
        }

        public void SetValue<TInvoker>(TInvoker Invoker, double value) where TInvoker : Control
        {
            this.SetText<TInvoker>(Invoker, value.ToString());
        }

        public void SetText<TInvoker>(TInvoker Invoker, string text) where TInvoker : Control
        {
            if (Invoker != null)
            {
                Abbreviate.FormsControls.Invoke(Invoker, () => SetText<TInvoker>(null, text));
                return;
            }

            this.Text = text;
            this.ModeTest();
        }


        public string GetTextValue
        {
            get
            {
            string text = this.Text;
            if (!Objects.IsNullOrEmpty(text, Unit))
                text = text.Replace(Unit, "");

                return text;
            }
        }

        public void ModeTest()
        {
            string text = this.GetTextValue;

            switch (DisplayMode)
            {
                case Mode.Double:
                    {

                        double value = Doubles.ParseAny(text, DoubleDefault);
                        //try
                        //{
                        //    value = double.Parse(text);
                        //    value = Math.Round(value, Decimals);

                        //    if (value > DoubleMax || value < DoubleMin)
                        //        value = DoubleDefault;
                        //}
                        //catch { value = DoubleDefault; }
                        //value = ;

                        this.Text = Doubles.ToString(value, DoubleDefault.ToString(), Decimals, DoubleMin, DoubleMax, ',') + Unit;
                    };
                    break;

                case Mode.Integer:
                    {
                        int value;
                        try
                        {
                            value = int.Parse(text);
                            if (value > IntegerMax || value < IntegerMin)
                                value = IntegerDefault;
                        }
                        catch { value = IntegerDefault; }

                        this.Text = value + Unit;
                    };
                    break;
                default: return;

            }


        }
    }
}
