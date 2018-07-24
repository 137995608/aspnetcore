using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Mozlite.Extensions.Storages.Configuration
{
    /// <summary>
    /// ���ù���
    /// </summary>
    public class ConfigurationDataManager : IConfigurationDataManager
    {
        private readonly IMemoryCache _cache;
        private const string ConfigDir = "configdata";

        private string GetPath(string name)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), ConfigDir);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return Path.Combine(path, $"{name}.json");
        }

        private string GetCacheKey(string name) => $"{ConfigDir}:[{name}]";

        /// <summary>
        /// ��ʼ����<see cref="ConfigurationDataManager"/>��
        /// </summary>
        /// <param name="cache">����ӿڡ�</param>
        public ConfigurationDataManager(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        /// <summary>
        /// �������á�   
        /// </summary>
        /// <typeparam name="TConfiguration">�������͡�</typeparam>
        /// <param name="name">���ƣ��������ļ���չ����</param>
        /// <param name="minutes">�����������</param>
        /// <returns>��������ʵ����</returns>
        public virtual TConfiguration LoadConfiguration<TConfiguration>(string name, int minutes = 3)
        {
            if (minutes <= 0) return LoadConfiguration<TConfiguration>(name);
            return _cache.GetOrCreate(GetCacheKey(name), ctx =>
            {
                ctx.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
                return LoadConfiguration<TConfiguration>(name);
            });
        }

        private TConfiguration LoadConfiguration<TConfiguration>(string name)
        {
            var path = GetPath(name);
            if (!File.Exists(path))
                return default;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
                return JsonConvert.DeserializeObject<TConfiguration>(sr.ReadToEnd());
        }

        /// <summary>
        /// �������á�   
        /// </summary>
        /// <typeparam name="TConfiguration">�������͡�</typeparam>
        /// <param name="name">���ƣ��������ļ���չ����</param>
        /// <param name="minutes">�����������</param>
        /// <returns>��������ʵ����</returns>
        public virtual async Task<TConfiguration> LoadConfigurationAsync<TConfiguration>(string name, int minutes = 3)
        {
            if (minutes <= 0) return await LoadConfigurationAsync<TConfiguration>(name);
            return await _cache.GetOrCreateAsync(GetCacheKey(name), async ctx =>
            {
                ctx.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
                return await LoadConfigurationAsync<TConfiguration>(name);
            });
        }

        private async Task<TConfiguration> LoadConfigurationAsync<TConfiguration>(string name)
        {
            var path = GetPath(name);
            if (!File.Exists(path))
                return default;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
                return JsonConvert.DeserializeObject<TConfiguration>(await sr.ReadToEndAsync());
        }

        /// <summary>
        /// �������á�
        /// </summary>
        /// <param name="name">���ƣ��������ļ���չ����</param>
        /// <param name="configuration">����ʵ����</param>
        public virtual void SaveConfiguration(string name, object configuration)
        {
            var path = GetPath(name);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
                sw.Write(JsonConvert.SerializeObject(configuration));
            _cache.Remove(GetCacheKey(name));
        }

        /// <summary>
        /// �������á�
        /// </summary>
        /// <param name="name">���ƣ��������ļ���չ����</param>
        /// <param name="configuration">����ʵ����</param>
        public virtual async Task SaveConfigurationAsync(string name, object configuration)
        {
            var path = GetPath(name);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
                await sw.WriteAsync(JsonConvert.SerializeObject(configuration));
            _cache.Remove(GetCacheKey(name));
        }
    }
}