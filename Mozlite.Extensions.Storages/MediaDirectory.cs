using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mozlite.Data;
using Mozlite.Extensions.Storages.Properties;

namespace Mozlite.Extensions.Storages
{
    /// <summary>
    /// ý���ļ��ṩ��ʵ���ࡣ
    /// </summary>
    public class MediaDirectory : IMediaDirectory
    {
        private readonly IStorageDirectory _directory;
        private readonly IDbContext<MediaFile> _mfdb;
        private readonly IDbContext<StoredFile> _sfdb;

        private const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
        private readonly string _media;

        /// <summary>
        /// ��ʼ����<see cref="MediaDirectory"/>��
        /// </summary>
        /// <param name="directory">�洢�ļ��С�</param>
        /// <param name="mfdb">���ݿ�����ӿڡ�</param>
        /// <param name="sfdb">���ݿ�����ӿڡ�</param>
        public MediaDirectory(IStorageDirectory directory, IDbContext<MediaFile> mfdb, IDbContext<StoredFile> sfdb)
        {
            _directory = directory;
            _mfdb = mfdb;
            _sfdb = sfdb;
            //ý���ļ��С�
            _media = directory.GetPhysicalPath("media");
        }

        /// <summary>
        /// �ϴ��ļ���
        /// </summary>
        /// <param name="file">���ļ���</param>
        /// <param name="extensionName">��չ���ơ�</param>
        /// <param name="targetId">Ŀ��Id��</param>
        /// <param name="uniqueMediaFile">ÿһ���ļ���ý��洢�ļ�һһ��Ӧ��</param>
        /// <returns>�����ϴ���Ľ����</returns>
        public async Task<MediaResult> UploadAsync(IFormFile file, string extensionName, int? targetId = null, bool uniqueMediaFile = true)
        {
            if (file == null || file.Length == 0)
                return new MediaResult(null, Resources.FormFileInvalid);
            var tempFile = _directory.GetTempPath(Guid.NewGuid().ToString());
            using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fs);
            }
            var media = new MediaFile();
            media.ExtensionName = extensionName;
            media.Extension = Path.GetExtension(file.FileName);
            media.Name = file.FileName;
            return await CreateAsync(new FileInfo(tempFile), media, file.ContentType, targetId, uniqueMediaFile);
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="url">�ļ�URL��ַ��</param>
        /// <param name="extensionName">��չ���ơ�</param>
        /// <param name="targetId">Ŀ��Id��</param>
        /// <param name="uniqueMediaFile">ÿһ���ļ���ý��洢�ļ�һһ��Ӧ��</param>
        /// <returns>�����ϴ���Ľ����</returns>
        public async Task<MediaResult> DownloadAsync(string url, string extensionName, int? targetId = null, bool uniqueMediaFile = true)
        {
            var uri = new Uri(url);
            using (var client = new HttpClient())
            {
                var tempFile = _directory.GetTempPath(Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Referrer = new Uri($"{uri.Scheme}://{uri.DnsSafeHost}{(uri.IsDefaultPort ? null : ":" + uri.Port)}/");
                client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                using (var stream = await client.GetStreamAsync(uri))
                {
                    using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                    {
                        await stream.CopyToAsync(fs);
                    }
                }
                var media = new MediaFile();
                media.ExtensionName = extensionName;
                media.Extension = Path.GetExtension(uri.AbsolutePath);
                media.Name = Path.GetFileName(uri.AbsolutePath);
                return await CreateAsync(new FileInfo(tempFile), media, media.Extension.GetContentType(), targetId, uniqueMediaFile);
            }
        }

        private async Task<MediaResult> CreateAsync(FileInfo tempFile, MediaFile file, string contentType, int? targetId, bool uniqueMediaFile)
        {
            var fileId = tempFile.ComputeHash();
            var storage = await _sfdb.FindAsync(fileId);
            if (storage != null)
            {
                //ʵ���ļ��Ѿ����ڣ�ɾ����ʱĿ¼�µ��ļ�
                EnsureStoredFile(storage, tempFile);
                if (uniqueMediaFile)//Ψһ�ļ��洢
                {
                    var dbFile = await _mfdb.FindAsync(x => x.FileId == fileId);
                    if (dbFile != null)
                        return new MediaResult(dbFile.Url);
                }
            }
            else
            {
                //���ʵ���ļ��������򴴽�
                storage = new StoredFile();
                storage.ContentType = contentType;
                storage.FileId = fileId;
                storage.Length = tempFile.Length;
                if (await _sfdb.CreateAsync(storage))
                    EnsureStoredFile(storage, tempFile);
            }
            file.TargetId = targetId;
            file.FileId = fileId;
            if (await _mfdb.CreateAsync(file)) return new MediaResult(file.Url);
            return new MediaResult(null, Resources.StoredFileFailured);
        }

        private void EnsureStoredFile(StoredFile file, FileInfo tempFile)
        {
            //���ļ��ƶ���ý��洢·���¡�
            var mediaPath = Path.Combine(_media, file.Path);
            if (File.Exists(mediaPath))
            {//����Ѿ��д����ļ���ɾ����ʱ�ļ�
                tempFile.Delete();
            }
            else
            {
                var dir = Path.GetDirectoryName(mediaPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                tempFile.MoveTo(mediaPath);
            }
        }

        /// <summary>
        /// ͨ��GUID��ȡ�洢�ļ�ʵ����
        /// </summary>
        /// <param name="id">ý���ļ�Id��</param>
        /// <returns>���ش洢�ļ�ʵ����</returns>
        public async Task<StoredPhysicalFile> FindAsync(Guid id)
        {
            var file = await _sfdb.AsQueryable().InnerJoin<MediaFile>((sf, mf) => sf.FileId == mf.FileId)
                .Where<MediaFile>(x => x.Id == id)
                .Select<MediaFile>(x => x.Name)
                .Select(x => new { x.FileId, x.ContentType })
                .FirstOrDefaultAsync(reader => new StoredPhysicalFile(reader));
            file.PhysicalPath = Path.Combine(_media, file.PhysicalPath);
            return file;
        }
    }
}