using System;

namespace Mozlite.Mvc.Themes
{
    /// <summary>
    /// ģ��Ӧ�ó������û��ࡣ
    /// </summary>
    public abstract class ThemeApplicationBase : IThemeApplication
    {
        /// <summary>
        /// Ӧ�ó������ơ�
        /// </summary>
        public virtual string ApplicationName
        {
            get
            {
                var applicationName = GetType().Namespace;
                var index = applicationName.IndexOf(".Extensions.", StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    applicationName = applicationName.Substring(index + 12);
                    index = applicationName.IndexOf('.');
                    if (index != -1)
                        applicationName = applicationName.Substring(0, index);
                }
                return applicationName.ToLower();
            }
        }

        /// <summary>
        /// ��ʾ���ơ�
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// ������
        /// </summary>
        public virtual string Description { get; } = null;

        /// <summary>
        /// ��ʽ��
        /// </summary>
        public virtual string CssClass => null;

        /// <summary>
        /// ���ӵ�ַ��
        /// </summary>
        public virtual string LinkUrl => $"/dashboard/{ApplicationName}";

        /// <summary>
        /// ͼ����ʽ��
        /// </summary>
        public virtual string IconClass => "fa ";

        /// <summary>
        /// ���ȼ���Խ��Խ��ǰ��
        /// </summary>
        public virtual int Priority { get; } = 0;

        /// <summary>
        /// ����ģʽ��
        /// </summary>
        public virtual NavigateMode Mode { get; } = NavigateMode.Module;
    }
}