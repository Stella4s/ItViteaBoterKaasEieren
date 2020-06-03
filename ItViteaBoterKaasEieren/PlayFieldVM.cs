using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ItViteaBoterKaasEieren 
{
    public class PlayFieldVM : INotifyPropertyChanged
    {
        #region private var for public properties
        private FieldSquare[,] _PlayingField;
        private int _TurnCounter;
        private bool _IsPlayer1;
        #endregion

        public PlayFieldVM ()
        {
            _PlayingField = new FieldSquare[3,3];
            StartGame();
        }

        #region public properties
        public FieldSquare[,] PlayingField
        {
            get { return _PlayingField; }
            set
            {
                _PlayingField = value;
                OnPropertyChanged();
            }
        }
        public int TurnCounter
        {
            get { return _TurnCounter; }
            set
            {
                _TurnCounter = value;
                OnPropertyChanged();
            }
        }
        public bool IsPlayer1
        {
            get { return _IsPlayer1; }
            set
            {
                _IsPlayer1 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region methods
        public void StartGame()
        {
            ResetPlayField();
            IsPlayer1 = true;
            TurnCounter = 0;
        }
        public void NextTurn()
        {
            //Switch between player 1's turn and player 2's turn.
            if (IsPlayer1)
                IsPlayer1 = false;
            else
                IsPlayer1 = true;
            TurnCounter++;
        }
        public void ResetPlayField()
        {
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //square = new FieldSquare();
                    //square.FieldState = FieldSquare.FieldStates.None;
                }
            }
        }

        #region SquareField Specific Methods
    
        #endregion
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }

}
