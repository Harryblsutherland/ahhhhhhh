namespace Gallery3WinForm
{
    public sealed partial class frmPhotograph : Gallery3WinForm.frmWork
    {   //Singleton
        public static readonly frmPhotograph Instance = new frmPhotograph();

        private frmPhotograph()
        {
            InitializeComponent();
        }

        public static void Run(clsPhotograph prPhotograph)
        {
            Instance.SetDetails(prPhotograph);
        }

        protected override void updateForm()
        {
            base.updateForm();
            clsPhotograph lcWork = (clsPhotograph)this._Work;
            txtWidth.Text = lcWork.Width.ToString();
            txtHeight.Text = lcWork.Height.ToString();
            txtType.Text = lcWork.Type;
        }

        protected override void pushData()
        {
            base.pushData();
            clsPhotograph lcWork = (clsPhotograph)_Work;
            lcWork.Width = float.Parse(txtWidth.Text);
            lcWork.Height = float.Parse(txtHeight.Text);    
            lcWork.Type = txtType.Text;
        }
    }
}

