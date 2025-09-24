using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using Windows.Foundation;

namespace Uestc.BBS.WinUI.Common
{
    public abstract class IncrementalLoadingCollection<TKey, TValue>()
        : ObservableCollection<TValue>,
            INotifyPropertyChanged,
            ISupportIncrementalLoading
    {
        protected readonly HashSet<TKey> _keys = new(512);

        /// <summary>
        /// 分页索引
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get;
            init => field = value > 0 ? value : field;
        } = 30;

        /// <summary>
        /// 筛选条目
        /// </summary>
        public Func<TValue, bool>? Filter { get; init; }

        /// <summary>
        /// 获取条目键值
        /// </summary>
        public Func<TValue, TKey>? KeySelector { get; init; }

        /// <summary>
        /// 加载事件
        /// </summary>
        public event Action<int>? LoadedMoreItems;

        /// <summary>
        /// 刷新事件
        /// </summary>
        public event Action? OnRefresh;

        /// <summary>
        /// 异常事件
        /// </summary>
        public event Action<Exception>? OnException;

        /// <summary>
        /// 是否有更多条目
        /// </summary>
        public bool HasMoreItems { get; private set; } = true;

        /// <summary>
        /// 是否正在加载更多条目
        /// </summary>
        public bool IsLoading
        {
            get;
            private set
            {
                field = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        /// <summary>
        /// PropertyChanged 事件
        /// </summary>
        public new event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 加载更多条目
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) =>
            AsyncInfo.Run(async cancellationToken =>
            {
                if (IsLoading)
                {
                    return new LoadMoreItemsResult { Count = 0 };
                }

                IsLoading = true;

                try
                {
                    var pagedItems = await GetPagedItemsAsync(Page, PageSize, cancellationToken);
                    if (!pagedItems.Any())
                    {
                        HasMoreItems = false;
                        return new LoadMoreItemsResult { Count = 0 };
                    }

                    LoadedMoreItems?.Invoke(Page);

                    var filteredItems = pagedItems
                        .Where(i => Filter is null || Filter(i))
                        .Where(i => KeySelector is null || _keys.Add(KeySelector(i)))
                        .ToArray();

                    foreach (var item in filteredItems)
                    {
                        Add(item);
                    }

                    Page++;
                    HasMoreItems = true;

                    return new LoadMoreItemsResult { Count = (uint)filteredItems.Length };
                }
                catch (Exception e)
                {
                    HasMoreItems = false;
                    OnException?.Invoke(e);
                    return new LoadMoreItemsResult { Count = 0 };
                }
                finally
                {
                    IsLoading = false;
                }
            });

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <returns></returns>
        public async Task RefreshAsync()
        {
            Page = 0;
            HasMoreItems = true;

            Clear();
            _keys.Clear();

            OnRefresh?.Invoke();

            await LoadMoreItemsAsync(0);
        }

        /// <summary>
        /// 获取分页条目
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task<IEnumerable<TValue>> GetPagedItemsAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default
        );
    }
}
