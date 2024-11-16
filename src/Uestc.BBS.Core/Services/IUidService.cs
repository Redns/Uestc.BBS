namespace Uestc.BBS.Core.Services
{
    /// <summary>
    /// 设备唯一识别码服务
    /// </summary>
    public interface IDeviceUidService
    {
        /// <summary>
        /// 生成设备唯一识别码
        /// </summary>
        /// <returns></returns>
        string Generate();
    }
}
