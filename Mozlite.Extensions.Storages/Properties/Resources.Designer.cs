﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mozlite.Extensions.Storages.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Mozlite.Extensions.Storages.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 文件夹不存在！ 的本地化字符串。
        /// </summary>
        internal static string DirectoryNotFound {
            get {
                return ResourceManager.GetString("DirectoryNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 文件不存在！ 的本地化字符串。
        /// </summary>
        internal static string FileNotFound {
            get {
                return ResourceManager.GetString("FileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未能获取表单文件实例或者文件长度为0！ 的本地化字符串。
        /// </summary>
        internal static string FormFileInvalid {
            get {
                return ResourceManager.GetString("FormFileInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 定期清理存储文件夹和临时文件夹中得空文件夹或无效文件等 的本地化字符串。
        /// </summary>
        internal static string StorageClearTaskExecutor_Description {
            get {
                return ResourceManager.GetString("StorageClearTaskExecutor_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 存储文件夹清理服务 的本地化字符串。
        /// </summary>
        internal static string StorageClearTaskExecutor_Name {
            get {
                return ResourceManager.GetString("StorageClearTaskExecutor_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 存储文件失败！ 的本地化字符串。
        /// </summary>
        internal static string StoredFileFailured {
            get {
                return ResourceManager.GetString("StoredFileFailured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 成功！ 的本地化字符串。
        /// </summary>
        internal static string Success {
            get {
                return ResourceManager.GetString("Success", resourceCulture);
            }
        }
    }
}
