namespace Uestc.BBS.Mvvm.Services
{
    public interface ICaptureService<TUIElement>
        where TUIElement : class
    {
        /// <summary>
        /// 截取控件内容并保存到指定路径
        /// </summary>
        /// <param name="element"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task Capture(
            TUIElement element,
            string filePath,
            CancellationToken cancellationToken = default
        );
    }
}
