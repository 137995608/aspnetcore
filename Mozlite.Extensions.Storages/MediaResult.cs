namespace Mozlite.Extensions.Storages
{
    /// <summary>
    /// �ϴ�/���ؽ����
    /// </summary>
    public class MediaResult
    {
        /// <summary>
        /// ��ʼ����<see cref="MediaResult"/>��
        /// </summary>
        /// <param name="url">�ļ����ʵ�URL��ַ��</param>
        /// <param name="message">������Ϣ��</param>
        public MediaResult(string url, string message = null)
        {
            Url = url;
            Message = message;
            Succeeded = message == null;
        }

        /// <summary>
        /// �ļ����ʵ�URL��ַ��
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// ������Ϣ��
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// �Ƿ�ɹ���
        /// </summary>
        public bool Succeeded { get; } = true;
    }
}