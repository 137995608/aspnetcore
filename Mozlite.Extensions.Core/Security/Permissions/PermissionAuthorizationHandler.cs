using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Mozlite.Extensions.Security.Permissions
{
    /// <summary>
    /// Ȩ����֤�������ࡣ
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IPermissionManager _permissionManager;
        /// <summary>
        /// ��ʼ����<see cref="PermissionAuthorizationHandler"/>��
        /// </summary>
        /// <param name="permissionManager">Ȩ�޹���ӿ�ʵ������</param>
        public PermissionAuthorizationHandler(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// ��֤��ǰȨ�޵ĺϷ��ԡ�
        /// </summary>
        /// <param name="context">��֤�����ġ�</param>
        /// <param name="requirement">Ȩ��ʵ����</param>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            var permissionName = requirement?.Name;
            if (permissionName == null && context.Resource is ControllerActionDescriptor resource)
            {
                if (resource.RouteValues.TryGetValue("area", out string area))
                    permissionName = area + ".";
                permissionName += $"{resource.ControllerName}.{resource.ActionName}";
            }
            if (permissionName == null)
            {
                context.Fail();
                return;
            }
            if (await _permissionManager.IsAuthorizedAsync(permissionName))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}