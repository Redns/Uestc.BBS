using Avalonia.Controls;
using Uestc.BBS.Core.Services.Forum;
using Uestc.BBS.Desktop.Converters;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class HomeView : UserControl
{
    /// <summary>
    /// 集合非空转换器
    /// </summary>
    public static readonly ObservableCollectionIsNotEmptyConverter<TopicOverview> TopicOverviewObservableCollectionIsNotEmptyConverter =
        new();

#if DEBUG
    public HomeView()
    {
        InitializeComponent();
    }
#endif

    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
