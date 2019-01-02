using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Observer.Models
{
    /// <summary>
    /// Базовый класс реализующий INotifyPropertyChanged
    /// </summary>
    [Magic]
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        protected virtual void RaisePropertyChanged(string propName)
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // для сложных сеттеров
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected static void Raise() { }
    }   

    internal class MagicAttribute : Attribute { }
}
