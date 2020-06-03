using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ItViteaBoterKaasEieren
{
    /// <summary>
    /// Simple class to hold fieldstates and symbols to be displayed together.
    /// </summary>
    public class FieldSquare : INotifyPropertyChanged
    {
        #region private var for public properties
        private FieldStates _FieldState;
        private string _Symbol;
        #endregion


        #region public properties
        public FieldStates FieldState
        {
            get { return _FieldState; }
            set
            {
                _FieldState = value;
                OnPropertyChanged();
            }
        }
        public string Symbol
        {
            get { return _Symbol; }
            set
            {
                _Symbol = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public enum FieldStates
        {
            None,
            Cross,
            Circle
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
