using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Entities;
using Uestc.BBS.WinUI.Common;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class ThreadHistoryOverlay : Page
    {
        public ThreadHistoryViewModel ViewModel { get; private set; }

        public ThreadHistorySource ThreadHistoryEntities { get; private set; }

        public ThreadHistoryOverlay(
            ThreadHistoryViewModel viewModel,
            Repository<ThreadHistoryEntity> threadHistoryRepository
        )
        {
            InitializeComponent();

            ViewModel = viewModel;
            ThreadHistoryEntities = new ThreadHistorySource(threadHistoryRepository);
        }

        public partial class ThreadHistorySource(
            Repository<ThreadHistoryEntity> threadHistoryRepository
        ) : IncrementalLoadingCollection<uint, ThreadHistoryEntity>
        {
            public override async Task<IEnumerable<ThreadHistoryEntity>> GetPagedItemsAsync(
                int page,
                int pageSize,
                CancellationToken cancellationToken = default
            ) =>
                await threadHistoryRepository.GetPageListAsync(
                    page,
                    pageSize,
                    db => db.OrderByDescending(t => t.BrowserDateTime),
                    cancellationToken
                );
        }
    }
}
