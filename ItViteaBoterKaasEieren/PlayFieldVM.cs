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
        private int _TurnCounter, _Player1Points, _Player2Points;
        //Using fieldsize as variable to allow for more flexible code to change easily in one spot instead of throughout entire document.
        private readonly int fieldSize = 3;
        private bool _IsPlayer1;
        #endregion

        public PlayFieldVM ()
        {
            PlayingField = new FieldSquare[fieldSize * fieldSize];
            InitializeGame();
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
        public int Player1Points
        {
            get { return _Player1Points; }
            set
            {
                _Player1Points = value;
                OnPropertyChanged();
            }
        }
        public int Player2Points
        {
            get { return _Player2Points; }
            set
            {
                _Player2Points = value;
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
        /// <summary>
        /// Initialises First Game.
        /// </summary>
        public void InitializeGame()
        {
            Player1Points = 0;
            Player2Points = 0;
            StartGame();
        }
        /// <summary>
        /// Starts and Restarts the game. Resetting all values.
        /// </summary>
        public void StartGame()
        {
            InitializePlayField();
            IsPlayer1 = true;
            isWinner = false;
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
        /// <summary>
        /// Advances turncounter and switches between which player's turn it is.
        /// </summary>
        public void NextTurn()
        {
            TurnCounter++;
            CheckWin();
            //Switch between player 1's turn and player 2's turn.
            if (IsPlayer1)
                IsPlayer1 = false;
            else
                IsPlayer1 = true;
        }

        private bool isWinner;
        public void CheckWin()
        {
            //Check if TurnCounter is above certain value to see if enough turns even passed for a sufficiently long row to be possible.
            //Assuming that theoretical fieldSize and length of continuous row needed increase together.
            if (TurnCounter >= ((fieldSize * 2) - 1))
            {
                int symbolCounterA = 0, symbolCounterB = 0, symbolCounterC = 0, symbolCounterD = 0;
                FieldSquare.FieldStates state;
                if (IsPlayer1)
                    state = FieldSquare.FieldStates.Cross;
                else
                    state = FieldSquare.FieldStates.Circle;

                //Checks from top left to right, each vertical column and horizontal row for fieldSize(3) symbols in a row.
                for (int i = 0; i < fieldSize; i++)
                {
                    //As playingField is a single dimension array it checks fields by multiplying either i or j with the fieldsize.
                    //As to move between rows and columns respectively.
                    for (int j = 0; j < fieldSize; j++)
                    {
                        //Checks Columns. example: 0 + (0 * 3) = 0. 0 + (1 * 3) = 3. Sequence Checked: 0, 3, 6.
                        if (PlayingField[i + (j * fieldSize)].FieldState == state)
                            symbolCounterA++;

                        //Checks rows. example: Second Row. i = 1. 0 + (1 * 3) = 3. Sequence Checked: 3, 4, 5. 
                        if (PlayingField[j + (i * fieldSize)].FieldState == state)
                            symbolCounterB++;
                    }
                    //Checking diagionals.
                    //Example:  0 + 0 * 3 = 0. 
                    //          1 + 1 * 3 = 4. 
                    //          2 + 2 * 3 = 8.  
                    if (PlayingField[i + (i * fieldSize)].FieldState == state)
                        symbolCounterC++;

                    //Example:  ((0 + 1) * 3) - (0 + 1) = 2
                    //          ((1 + 1) * 3) - (1 + 1) = 4
                    //          ((2 + 1) * 3) - (2 + 1) = 6
                    if (PlayingField[((i + 1) * fieldSize) - (i + 1)].FieldState == state)
                        symbolCounterD++;

                    //If any of the counters equal fieldSize invoke HasWon method and break out of for loop.
                    if (symbolCounterA == fieldSize || symbolCounterB == fieldSize ||
                        symbolCounterC == fieldSize || symbolCounterD == fieldSize)
                    {
                        HasWon();
                        break;
                    }

                    //Reset SymbolCounters A & B after each j loop after symbol check.
                    symbolCounterA = 0; symbolCounterB = 0;
                }

            }
        }
        public void HasWon()
        {
            isWinner = true;
            if (IsPlayer1)
                Player1Points++;
            else
                Player2Points++;
        }

        #region SquareField Specific Methods
        /// <summary>
        /// Method for handling Fields being clicked.
        /// If the fieldstate is none. (Aka not used.)
        /// Depending on which player's turn it is,
        /// it will change the field state to either Cross or Circle.
        /// Then calls NextTurn(); method.
        /// </summary>
        /// <param name="sender">The specific FieldSquare that is being clicked.</param>
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
        private ICommand _RestartGameCmd;
        public ICommand RestartGameCmd
        {
            get
            {
                if (_RestartGameCmd == null)
                {
                    _RestartGameCmd = new RelayCommand(
                        p => StartGame());
                }
                return _RestartGameCmd;
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
