namespace Mozlite.Extensions.Extensions
{
    /// <summary>
    /// ��ǰ��վ�����ķ�������
    /// </summary>
    public interface ISiteContextAccessorBase : ISingletonService
    {
        /// <summary>
        /// ��ȡ��ǰ��վ�����ġ�
        /// </summary>
        SiteContextBase SiteContext { get; }

        /// <summary>
        /// ���õ�ǰ������ʵ������̨�ֳ���ʹ�á�
        /// </summary>
        /// <param name="siteKey">��վΨһ����</param>
        /// <returns>���ص�ǰ��վ������ʵ����</returns>
        SiteContextBase GetThreadSiteContext(string siteKey = null);
    }
}