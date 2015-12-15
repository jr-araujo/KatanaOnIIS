using Owin;
using System.IO;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

/*TODO: Você pode executar o projeto com esta linha comentada/descomentada.
        Não vai surtir nenhum efeito no resultado final, a não ser que você
        Mude o nome da classe Startup.
*/
//[assembly: OwinStartup(typeof(KatanaOnIIS.Startup))]

namespace KatanaOnIIS
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            InicializacaoUsandoHelpers(app);
            //InicializacaoUsandoOwinNaIntegra(app);            
        }

        private void InicializacaoUsandoOwinNaIntegra(IAppBuilder app)
        {
            app.Use(new Func<AppFunc, AppFunc>(next => (env =>
            {
                var response = env["owin.ResponseBody"] as Stream;
                var headers = env["owin.ResponseHeaders"] as IDictionary<string, string[]>;

                headers["Content-Type"] = new[] { "text/html" };

                var htmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/html/formulario.html");

                var html = File.ReadAllText(htmlFilePath);

                return response.WriteAsync(Encoding.UTF8.GetBytes(html), 0, html.Length);
            })));
        }

        public void InicializacaoUsandoHelpers(IAppBuilder app)
        {
            var htmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/html/formulario.html");

            var html = File.ReadAllText(htmlFilePath);

            app.Run(context =>
            {
                context.Response.ContentType = "text/html";

                return context.Response.WriteAsync(html);
            });
        }
    }
}