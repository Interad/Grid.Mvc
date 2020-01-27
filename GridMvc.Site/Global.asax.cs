using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GridMvc.Site.Models;

namespace GridMvc.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var context = new NorthwindDbContext())
            {
                if (System.Diagnostics.Debugger.IsAttached
                    && context.Database.Exists()
                    && !context.Database.CompatibleWithModel(false)
                    && context.Database.Connection.ConnectionString.Contains("(LocalDb)"))
                {
                    context.Database.Delete();
                }

                if (context.Database.Exists())
                {
                    return;
                }

                context.Database.Create();
                ExecuteSql(context, @"\App_Data\instnwnd.sql");
            }
        }

        private static void ExecuteSql(NorthwindDbContext context, string filePath)
        {
            var code = File.ReadAllText(AssemblyDirectory + filePath);
            var commands = SplitCommands(code);
            foreach (var command in commands)
            {
                context.Database.ExecuteSqlCommand(command);
            }
        }

        private static IEnumerable<string> SplitCommands(string sqlCode)
        {
            return Regex.Split(sqlCode, @"GO(\r\n|\n|\r|$)", RegexOptions.IgnoreCase)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim());
        }

        private static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}