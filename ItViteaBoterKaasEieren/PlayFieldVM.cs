using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ItViteaBoterKaasEieren 
{
    public class PlayFieldVM : INotifyPropertyChanged
    {
        #region private var for public properties
        private FieldSquare[] _PlayingField;
        private int _TurnCounter;
        //Using fieldsize as variable to allow for more flexible code to change easily in one spot instead of throughout entire document.
        private readonly int fieldSize = 3;
        private bool _IsPlayer1;
        #endregion

        public PlayFieldVM ()
        {
            PlayingField = new FieldSquare[fieldSize * fieldSize];
            StartGame();
        }

        #region public properties
        public FieldSquare[] PlayingField
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
            InitializePlayField();
            IsPlayer1 = true;
            TurnCounter = 0;
        }
        /// <summary>
        /// Initalises or resets FieldSquares in playingfield. 
        /// If a Square is null a new obj will be made, otherwise it will be reset.
        /// </summary>
        public void InitializePlayField()
        {
            for (int i = 0; i < (fieldSize * fieldSize); i++)
            {
                if (PlayingField[i] is null)
                    PlayingField[i] = new FieldSquare { FieldState = FieldSquare.FieldStates.None, Symbol = "" };
                else
                {
                    PlayingField[i].FieldState = FieldSquare.FieldStates.None;
                    PlayingField[i].Symbol = "";
                }
            }
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
        #region SquareField Specific Methods
        public void ClickField(object sender)
        {
            var field = (FieldSquare)sender;
            if (field.FieldState == FieldSquare.FieldStates.None)
            {
                if (IsPlayer1)
                    field.FieldState = FieldSquare.FieldStates.Cross;
                else
                    field.FieldState = FieldSquare.FieldStates.Circle;
                NextTurn();
            }
        }
        #endregion
        #endregion

        #region ICommands
        private ICommand _ClickFieldCmd;
        public ICommand ClickFieldCmd
        {
            get
            {
                if (_ClickFieldCmd == null)
                {
                    _ClickFieldCmd = new RelayCommand(
                        p => ClickField(p));
                }
                return _ClickFieldCmd;
            }
        }
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
