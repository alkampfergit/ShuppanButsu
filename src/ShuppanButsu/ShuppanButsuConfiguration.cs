using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuppanButsu
{
    /// <summary>
    /// Main configuration file, basically it gets populated by the app.config .
    /// </summary>
    public class ShuppanButsuConfiguration
    {
        public String TemplateDirectory { get; private set; }

        public Int32 NumberOfPostsInHomePage { get; private set; }

        //public ShuppanButsuConfiguration() 
        //{ 
        //    var baseTemplateDirectory = ConfigurationManager.AppSettings["TemplateDirectory"] ?? "Templates";
        //    String basePath = AppDomain.CurrentDomain.BaseDirectory;
        //    TemplateDirectory = Path.Combine(basePath, baseTemplateDirectory);
        //}

        public ShuppanButsuConfiguration(
            String templateDirectory, 
            Int32 numberOfPostsInHomePage) 
        {
            TemplateDirectory = templateDirectory;
            NumberOfPostsInHomePage = numberOfPostsInHomePage;
        }
    }
}
