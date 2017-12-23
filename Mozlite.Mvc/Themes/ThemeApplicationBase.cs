using System;
using Mozlite.Mvc.Routing;
using Mozlite.Mvc.Themes.Menus;

namespace Mozlite.Mvc.Themes
{
    /// <summary>
    /// ģ��Ӧ�ó������û��ࡣ
    /// </summary>
    public abstract class ThemeApplicationBase : IThemeApplication, IMenuProvider
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

        /// <summary>
        /// �ṩ�����ƣ�ͬһ�����ƹ�Ϊͬһ���˵���
        /// </summary>
        public string Name { get; } = RouteSettings.Dashboard;

        /// <summary>
        /// ��ʼ���˵�ʵ����
        /// </summary>
        /// <param name="root">��Ŀ¼�˵���</param>
        public abstract void Init(MenuItem root);
    }
}