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
                    // �ڴ˴����� CEF ����ز���
                });

                env.CustomCefCommandLineArguments(commandLine =>
                {
                    // �ڴ˴�ָ�� CEF �����в���
                });

            }, app =>
            {
                app.UseLocalFileResource("http", "web.app.local", @"WebRoot");
                app.UseLocalFileResource("http", "model.app.local", @"ModelRoot");
                // ָ����������
                app.UseMainWindow(context => new MainWindow());
            })
            .Build()
            .Run();
        }
    }
}

