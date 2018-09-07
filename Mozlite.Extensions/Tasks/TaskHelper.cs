﻿using System.Threading;
using System.Threading.Tasks;
using Mozlite.Extensions.Installers;

namespace Mozlite.Extensions.Tasks
{
    /// <summary>
    /// 任务服务扩展类。
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// 等待到安装初始化完成。
        /// </summary>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前任务。</returns>
        public static async Task WaitInstalledAsync(this CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (InstallerHostedService.Current >= InstallerStatus.Success)
                    break;
                await Task.Delay(100, cancellationToken);
            }
        }

        /// <summary>
        /// 等待到安装初始化完成。
        /// </summary>
        /// <returns>返回当前任务。</returns>
        public static async Task WaitInitializingAsync()
        {
            while (true)
            {
                if (InstallerHostedService.Current >= InstallerStatus.Initializing)
                    break;
                await Task.Delay(100);
            }
        }
    }
}