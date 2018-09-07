using System;
using Microsoft.AspNetCore.Http;
using Mozlite.Extensions.Installers;

namespace Mozlite.Extensions.Extensions
{
    /// <summary>
    /// ʵ�ֵ�ǰ��վ�����ķ���ʵ����
    /// </summary>
    /// <typeparam name="TSite">��վ���͡�</typeparam>
    /// <typeparam name="TSiteContext">��վ�����ġ�</typeparam>
    public abstract class SiteContextAccessor<TSite, TSiteContext> : ISiteContextAccessor<TSite, TSiteContext>
        where TSite : SiteBase, new()
        where TSiteContext : SiteContextBase<TSite>, new()
    {
        private readonly ISiteManager _siteManager;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// ��ʼ����<see cref="SiteContextAccessor{TSite, TSiteContext}"/>��
        /// </summary>
        /// <param name="siteManager">��վ����ӿڡ�</param>
        /// <param name="contextAccessor">HTTP���������ġ�</param>
        protected SiteContextAccessor(ISiteManager siteManager, IHttpContextAccessor contextAccessor)
        {
            _siteManager = siteManager;
            _contextAccessor = contextAccessor;
        }

        [ThreadStatic]
        private static TSiteContext _context;

        /// <summary>
        /// ��ȡ��ǰ��վ�����ġ�
        /// </summary>
        public TSiteContext SiteContext => GetRequestSiteContext() ?? GetThreadSiteContext();

        /// <summary>
        /// ���õ�ǰ������ʵ����
        /// </summary>
        /// <param name="siteKey">��վΨһ����</param>
        /// <returns>���ص�ǰ��վ������ʵ����</returns>
        SiteContextBase ISiteContextAccessorBase.GetThreadSiteContext(string siteKey) => GetThreadSiteContext(siteKey);

        /// <summary>
        /// ���õ�ǰ������ʵ������̨�ֳ���ʹ�á�
        /// </summary>
        /// <param name="siteKey">��վΨһ����</param>
        /// <returns>���ص�ǰ��վ������ʵ����</returns>
        public TSiteContext GetThreadSiteContext(string siteKey = null)
        {
            if (_context != null)
                return _context;
            _context = new TSiteContext();
            if (InstallerHostedService.Current != InstallerStatus.Success) return _context;
            _context.Site = _siteManager.GetSiteByKey<TSite>(siteKey);
            _context.Domain = _siteManager.GetDomain(_context.SiteId);
            return _context;
        }

        private TSiteContext GetRequestSiteContext()
        {
            return _contextAccessor.HttpContext?.Items[typeof(SiteContextBase)] as TSiteContext;
        }

        /// <summary>
        /// ��ȡ��ǰ��վ�����ġ�
        /// </summary>
        SiteContextBase ISiteContextAccessorBase.SiteContext => SiteContext;
    }
}