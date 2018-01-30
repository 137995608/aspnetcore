using System;
using Microsoft.AspNetCore.Http;
using Mozlite.Extensions.Properties;
using Mozlite.Extensions.Sites;

namespace Mozlite.Extensions
{
    /// <summary>
    /// ʵ�ֵ�ǰ��վ�����ķ���ʵ����
    /// </summary>
    /// <typeparam name="TSite">��վ���͡�</typeparam>
    /// <typeparam name="TSiteContext">��վ�����ġ�</typeparam>
    public class SiteContextAccessor<TSite, TSiteContext> : ISiteContextAccessor<TSite, TSiteContext>
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
        public SiteContextAccessor(ISiteManager siteManager, IHttpContextAccessor contextAccessor)
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
            if (_context?.Initialized == true)
                throw new Exception(Resources.SiteContextIsInitialized);
            _context = new TSiteContext();
            if (domain == null)
                domain = _contextAccessor.HttpContext.Request.Host.Host;
            var siteDomain = _siteManager.GetDomain(domain);
            if (siteDomain != null)
            {
                _context.IsDefault = siteDomain.IsDefault;
                _context.Disabled = siteDomain.Disabled;
                _context.Domain = siteDomain.Domain;
                _context.Site = _siteManager.GetSite<TSite>(siteDomain.SiteId);
                _context.Initialized = true;
            }
            return _context;
        }

        /// <summary>
        /// ��ȡ��ǰ��վ�����ġ�
        /// </summary>
        SiteContextBase ISiteContextAccessorBase.SiteContext => SiteContext;
    }
}