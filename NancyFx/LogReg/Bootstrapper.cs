using Nancy;
using Nancy.Session.Persistable;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.Session.InMemory;

namespace LogReg
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            PersistableSessions.Enable(pipelines, new InMemorySessionConfiguration());
        }
    }
}