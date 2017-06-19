using System;

namespace BoredGamesWindowsform
{
    public sealed partial class frmPartyGame : BoredGamesWindowsform.frmGame
    {   //Singleton
        private static readonly frmPartyGame Instance = new frmPartyGame();

        private frmPartyGame()
        {
            InitializeComponent();
        }

        public static void Run(clsAllGames prPartyGame)
        {
            Instance.SetDetails(prPartyGame);
        }

        protected override void updateForm()
        {
            base.updateForm();
            txtPlayers.Text = _Game.Players.ToString();
            
        }

        protected override void pushData()
        {
            base.pushData();
            _Game.Players = int.Parse(txtPlayers.Text);

        }

        private void frmPartyGame_Load(object sender, EventArgs e)
        {

        }


    }
}

