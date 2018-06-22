using wp.Modules;

namespace wp
{
    [DependsOn(typeof(WpKernelModule))]
    public class WpTestBaseModule : WpModule
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