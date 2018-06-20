using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using wp.Collections.Extensions;

namespace wp.Modules
{
    public abstract class WpModule
    {
        /// <summary>
        /// 首次启动时调用，在依赖注入前调用
        /// </summary>
        public virtual void PreInitialize()
        {
        }

        /// <summary>
        /// 用于给模块添加依赖注入
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 模块启动后调用
        /// </summary>
        public virtual void PostInitialize()
        {
        }

        /// <summary>
        /// 模块关闭时调用
        /// </summary>
        public virtual void Shutdown()
        {
        }

        public virtual Assembly[] GetAdditionalAssemblies()
        {
            return new Assembly[0];
        }

        /// <summary>
        /// 检查是否WpModule
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsWpModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass &&
                   !typeInfo.IsAbstract &&
                   !typeInfo.IsGenericType &&
                   typeof(WpModule).IsAssignableFrom(type);
        }

        /// <summary>
        /// 查找模块的依赖模块（不包括当前模块）
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            if (!IsWpModule(moduleType))
            {
                //TODO:convert to AbpInitializationException
                throw new Exception("This type is not an WP module: " + moduleType.AssemblyQualifiedName);
            }

            var list = new List<Type>();

            if (moduleType.GetTypeInfo().IsDefined(typeof(DependsOnAttribute), true))
            {
                var dependsOnAttributes = moduleType.GetTypeInfo().GetCustomAttributes(typeof(DependsOnAttribute), true)
                    .Cast<DependsOnAttribute>();
                foreach (var dependsOnAttribute in dependsOnAttributes)
                {
                    foreach (var dependedModuleType in dependsOnAttribute.DependedModuleTypes)
                    {
                        list.Add(dependedModuleType);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 递归查找模块的依赖模块
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public static List<Type> FindDependedModuleTypesRecursivelyIncludingGiveModule(Type moduleType)
        {
            var list = new List<Type>();
            AddModuleAndDependenciesRecursively(list, moduleType);
            list.AddIfNotContains(typeof(WpKernelModule));
            return list;
        }

        /// <summary>
        /// 递归添加依赖模块
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="module"></param>
        private static void AddModuleAndDependenciesRecursively(List<Type> modules, Type module)
        {
            if (!IsWpModule(module))
            {
                ////TODO:convert to AbpInitializationException
                throw new Exception("This type is not an WP module: " + module.AssemblyQualifiedName);
            }

            if (modules.Contains(module))
            {
                return;
            }

            modules.Add(module);

            var dependedModules = FindDependedModuleTypes(module);
            foreach (var dependedModule in dependedModules)
            {
                AddModuleAndDependenciesRecursively(modules, dependedModule);
            }
        }
    }
}