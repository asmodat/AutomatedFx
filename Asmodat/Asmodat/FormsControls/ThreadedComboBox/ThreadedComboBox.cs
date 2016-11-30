using System;
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
    public partial class ThreadedComboBox : ComboBox
    {
        public void AddItemsEnum<E, TInvoker>(TInvoker Invoker, bool append = true, int index = 0) where TInvoker : Control
        {
            if (Invoker != null) {// lock (Locker[this]) 
                    Abbreviate.FormsControls.Invoke(Invoker, () => AddItemsEnum<E, TInvoker>(null, append, index));
                return; }

            string[] items = Enums.ToString<E>().ToArray();
            this.AddItems<TInvoker>(Invoker, append, index, items);
        }

        public E GetEnum<E, TInvoker>(TInvoker Invoker) where TInvoker : Control
        {
            if (Invoker != null)// lock (Locker[this])
                    return Abbreviate.FormsControls.Invoke<E, TInvoker>(Invoker, () => GetEnum<E, TInvoker>(null));

            string txt = this.Text;
            if (System.String.IsNullOrEmpty(txt))
                return default(E);

      
            return (E)Enum.Parse(typeof(E), txt);
        }

        public string GetText<TInvoker>(TInvoker Invoker) where TInvoker : Control
        {
            if (Invoker != null)// lock (Locker[this])
                    return Abbreviate.FormsControls.Invoke<string, Control>(Invoker, () => GetText<TInvoker>(null));

            return this.Text;
        }

        public double GetDouble<TInvoker>(TInvoker Invoker) where TInvoker : Control
        {
            if (Invoker != null)// lock (Locker[this])
                    return Abbreviate.FormsControls.Invoke<double, Control>(Invoker, () => GetDouble<TInvoker>(null));

            try
            {
                return double.Parse(this.GetTextValue);
            }
            catch
            {
                this.Text = DoubleDefault + Unit;
                return DoubleDefault;
            }
        }


        public void AddItems<TInvoker>(TInvoker Invoker, bool append = true, int index = 0, params string[] items) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[this])
                Abbreviate.FormsControls.Invoke(Invoker, () => AddItems<TInvoker>(null, append, index, items));
                return;
            }


            bool equals = Objects.EqualsItems(items, this.Items.Cast<object>().ToArray());

            if (!equals)
            {
                if (append) this.Items.Clear();
                
                this.Items.AddRange(items);

                if (index >= 0 && index < this.Items.Count)
                    this.SelectedIndex = index;
            }
        }


        public void AddItems<TInvoker>(TInvoker Invoker, bool append = true, int index = 0, params double[] items) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[this])
                Abbreviate.FormsControls.Invoke(Invoker, () => AddItems<TInvoker>(null, append, index, items));
                return;
            }

            List<string> newItems = new List<string>();

            foreach (double item in items)
                newItems.Add(item + Unit);

            bool equals = Objects.EqualsItems(newItems.ToArray(), this.Items.Cast<object>().ToArray());

            if (!equals)
            {
                if (append) this.Items.Clear();

                this.Items.AddRange(newItems.ToArray());

                if (index >= 0 && index < this.Items.Count)
                    this.SelectedIndex = index;
            }
        }
    }
}
