using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIXProject2
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        private string _leetcodeSession;
        public string LeetcodeSession
        {
            get => _leetcodeSession;
            set { _leetcodeSession = value; OnPropertyChanged(nameof(LeetcodeSession)); }
        }

        private string _csrfToken;
        public string CsrfToken
        {
            get => _csrfToken;
            set { _csrfToken = value; OnPropertyChanged(nameof(CsrfToken)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
