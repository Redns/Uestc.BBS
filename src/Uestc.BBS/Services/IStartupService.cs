namespace Uestc.BBS.Services
{
    /// <summary>
    /// ����������
    /// </summary>
    public interface IStartupService
    {
        void Enable();

        void Disable();
    }

    /// <summary>
    /// ��������Ϣ
    /// </summary>
    public class StartupInfo
    {
        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// ����
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Ӧ������
        /// </summary>
        public string ApplicationName { get; set; } = string.Empty;

        /// <summary>
        /// ����·��
        /// </summary>
        public string WorkingDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Ӧ��·��
        /// </summary>
        public string ApplicationPath { get; set; } = string.Empty;
    }
}
