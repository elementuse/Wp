using wp.Modules;

namespace wp.Web
{
    [DependsOn(typeof(WpKernelModule))]
    public class WpWebCommonModule : WpModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}