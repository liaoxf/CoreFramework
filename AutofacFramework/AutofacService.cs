using Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace AutofacFramework
{
    public class AutofacService
    {
        private static object lockHelper = new object();
        private static IContainer _instance;
        private static ContainerBuilder _builder;

        AutofacService(IContainer container)
        {
            _instance = container;
        }


        public static IContainer Register(params string[] assemblyNames)
        {
            var builder = CreateInstance(_builder);

            assemblyNames.ToList().ForEach(name =>
            {
                var interfaceFactory = Assembly.Load(name);
                builder.RegisterAssemblyTypes(interfaceFactory, interfaceFactory)
                  .AsImplementedInterfaces();
            });
            _instance = builder.Build();
            return _instance;
        }


        /// <summary>
        /// 注入接口
        /// </summary>
        /// <param name="assemblyNames"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterInterface(params string[] assemblyNames)
        {
            var builder = CreateInstance(_builder);

            var assembly = Assembly.GetExecutingAssembly();

            assemblyNames.ToList().ForEach(name =>
            {
                var interfaceFactory = Assembly.Load(name);
                builder.RegisterAssemblyTypes(interfaceFactory, interfaceFactory)
                  .AsImplementedInterfaces();
            });

            return builder;
        }

        public static T Resolve<T>()
        {
            return _instance.Resolve<T>();
        }

        public static IContainer Instance
        {
            //get { return _instance ?? Register(); }

            get { return _instance; }
        }

        public static T CreateInstance<T>(T _instance)
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = (T)Activator.CreateInstance(typeof(T));
                    }
                }
            }
            return _instance;
        }
    }

    public interface IRegister
    {
        void Install();
    }

    public class Register : IRegister
    {
        public Register()
        {
            Install();
        }
        //子类必须重写
        public virtual void Install()
        {

        }
    }
}
