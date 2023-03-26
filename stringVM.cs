using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_bot_multicast
{
    public class stringVM : INotifyPropertyChanged
    {
        string _myString;

        public string MyString
        {
            get { return _myString; }
            set
            {
                _myString = value;
                OnPropertyChanged("MyString");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
