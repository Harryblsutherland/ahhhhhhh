namespace BoredGamesWindowsform
{
    public sealed partial class frmBoardGame : BoredGamesWindowsform.frmGame
    {   //Singleton
        public static readonly frmBoardGame Instance = new frmBoardGame();

        private frmBoardGame()
        {
            InitializeComponent();
        }

        public static void Run(clsAllGames prBoardGame)
        {
            Instance.SetDetails(prBoardGame);
        }

        protected override void updateForm()
        {
            base.updateForm();
            txtGenre.Text = _Game.Genre;
        }

        protected override void pushData()
        {
            base.pushData();
            _Game.Genre = txtGenre.Text;
   
        }
    }
}

