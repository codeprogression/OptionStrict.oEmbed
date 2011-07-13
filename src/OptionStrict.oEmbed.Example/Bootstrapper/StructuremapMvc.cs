using System.Web.Mvc;
using OptionStrict.oEmbed.Example.Bootstrapper;

[assembly: WebActivator.PreApplicationStartMethod(typeof(OptionStrict.oEmbed.Example.App_Start.StructuremapMvc), "Start")]

namespace OptionStrict.oEmbed.Example.App_Start {
    public static class StructuremapMvc {
        public static void Start() {
            var container = Container.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}