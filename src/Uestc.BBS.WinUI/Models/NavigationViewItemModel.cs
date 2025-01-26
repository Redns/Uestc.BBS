using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Documents;

namespace Uestc.BBS.WinUI.Models
{
    public class NavigationViewItemModel:ObservableObject 
    {
        public string Name {  get; set; }

        public Glyphs Glyphs { get; set; } 

        public bool IsOnBottom { get; set; } = false;
    }
}
