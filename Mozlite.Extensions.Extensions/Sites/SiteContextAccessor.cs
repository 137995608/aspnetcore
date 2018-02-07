using System;
using Microsoft.AspNetCore.Http;
using Mozlite.Data;
using Mozlite.Extensions.Properties;

namespace Mozlite.Extensions.Sites
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
        public TSiteContext SiteContext => _context ?? CreateSiteContext();

        /// <summary>
        /// ���õ�ǰ������ʵ����
        /// </summary>
        /// <param name="domain">������ַ�����Ϊ�����HTTP�������еõ���</param>
        /// <returns>���ص�ǰ��վ������ʵ����</returns>
        SiteContextBase ISiteContextAccessorBase.CreateSiteContext(string domain) => CreateSiteContext(domain);

        /// <summary>
        /// ͨ��������ȡ��ǰ������ʵ����
        /// </summary>
        /// <param name="domain">������ַ�����Ϊ�����HTTP�������еõ���</param>
        /// <returns>���ص�ǰ��վ������ʵ����</returns>
        public TSiteContext CreateSiteContext(string domain = null)
        {
            if (_context != null)
                throw new Exception(Resources.SiteContextIsInitialized);
            _context = new TSiteContext();
            if (domain == null)
                domain = _contextAccessor.HttpContext.Request.Host.Host;
            _context.Domain = domain;
            if (!Database.IsMigrated) return _context;
            var siteDomain = _siteManager.GetDomain(domain);
            if (siteDomain != null)
            {
                _context.IsDefault = siteDomain.IsDefault;
                _context.Disabled = siteDomain.Disabled;
                _context.Site = _siteManager.GetSite<TSite>(siteDomain.SiteId);
            }
            return _context;
        }

        /// <summary>
        /// ��ȡ��ǰ��վ�����ġ�
        /// </summary>
        SiteContextBase ISiteContextAccessorBase.SiteContext => SiteContext;
    }
}