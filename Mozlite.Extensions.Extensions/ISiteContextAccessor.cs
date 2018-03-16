using Mozlite.Extensions.Sites;

namespace Mozlite.Extensions
{
    /// <summary>
    /// ��ǰ��վ�����ķ�������
    /// </summary>
    /// <typeparam name="TSite">��վ���͡�</typeparam>
    /// <typeparam name="TSiteContext">��վ�����ġ�</typeparam>
    public interface ISiteContextAccessor<TSite, TSiteContext> : ISiteContextAccessorBase
        where TSite : SiteBase, new()
        where TSiteContext : SiteContextBase<TSite>, new()
    {
        /// <summary>
        /// ��ȡ��ǰ��վ�����ġ�
        /// </summary>
        new TSiteContext SiteContext { get; }

        /// <summary>
        /// ���õ�ǰ������ʵ����
        /// </summary>
        /// <param name="siteKey">��վΨһ����</param>
        /// <returns>���ص�ǰ��վ������ʵ����</returns>
        new TSiteContext CreateSiteContext(string siteKey = null);
    }
}