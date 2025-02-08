using Microsoft.UI.Xaml.Controls;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class PersonProfile : UserControl
    {
        public string Avatar { get; set; } = string.Empty;

        public string Level
        {
            get;
            set
            {
                if (uint.TryParse(value, out _))
                {
                    field = value;
                }
            }
        } = "0";

        public string Group { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Signature { get; set; } = string.Empty;

        public PersonProfile()
        {
            InitializeComponent();
        }
    }
}
