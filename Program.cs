using NetDimension.NanUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3dTilesExplorer
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // ...
            WinFormium.CreateRuntimeBuilder(env =>
            {

                env.CustomCefSettings(settings =>
                {
                    // 在此处设置 CEF 的相关参数
                });

                env.CustomCefCommandLineArguments(commandLine =>
                {
                    // 在此处指定 CEF 命令行参数
                });

            }, app =>
            {
                app.UseLocalFileResource("http", "web.app.local", @"WebRoot");
                app.UseLocalFileResource("http", "model.app.local", @"ModelRoot");
                // 指定启动窗体
                app.UseMainWindow(context => new MainWindow());
            })
            .Build()
            .Run();
        }
    }
}

