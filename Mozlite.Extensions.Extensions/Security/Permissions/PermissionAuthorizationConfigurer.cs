using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Mozlite.Extensions.Security.Permissions
{
    /// <summary>
    /// ע����֤�����ࡣ
    /// </summary>
    public class PermissionAuthorizationConfigurer : IServiceConfigurer
    {
        /// <summary>
        /// ���÷��񷽷���
        /// </summary>
        /// <param name="services">���񼯺�ʵ����</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }
}