using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetDimension.NanUI;
using NetDimension.NanUI.HostWindow;
using NetDimension.NanUI.JavaScript;

namespace _3dTilesExplorer
{
    class MainWindow : Formium
    {
        // 设置窗体样式类型
        public override HostWindowType WindowType => HostWindowType.System;
        // 指定启动 Url
        public override string StartUrl => "http://web.app.local/index.html";


        List<string> addNodellist = new List<string>();

        public MainWindow()
        {
            // 在此处设置窗口样式
            Size = new System.Drawing.Size(1024, 768);
            this.Title = "3D Tiles 查看器";
            //this.Mask.BackColor = Color.White;
            SplashScreen.Image = null;

            this.Icon =_3DTilesExplorer.Properties.Resources._3dtiles;

            this.StartPosition = FormStartPosition.CenterScreen;
            //退出的时候删除临时文件

            this.BeforeClose += (sender, e) => 
            {
                foreach (var item in addNodellist)
                {
                    if (System.IO.Directory.Exists(item))
                    {
                        System.IO.Directory.Delete(item);
                    }
                }
            };
        }

        [DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        private enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        protected override void OnReady()
        {
            // 在此处进行浏览器相关操作
            //ShowDevTools();
            var obj = new JavaScriptObject();
            //注册同步方法

            obj.Add("OpenModelSelect", (args =>
            {
                string text = "";
                InvokeIfRequired(() =>
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Multiselect = true;//该值确定是否可以选择多个文件
                    dialog.Title = "请选择模型文件";
                    dialog.Filter = "3D TilesJson(*.json)|*.json";
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string jsonfilepath = dialog.FileName;

                        //创建链接
                        var basepath = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

                        var newdirname = System.Environment.CurrentDirectory + @"\ModelRoot\" + basepath;
                        addNodellist.Add(newdirname);
                        //创建目录
                        if (System.IO.Directory.Exists(newdirname))
                        {
                            System.IO.Directory.Delete(newdirname);
                        }
                        var newfilepath = System.IO.Path.Combine(newdirname, "tileset.json");
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(jsonfilepath);

                        var sourcesdir = fileInfo.Directory.FullName;
                        var filename = fileInfo.Directory.FullName.Replace(@"\",@"\\");
                        //执行链接
                        bool succ = CreateSymbolicLink(newdirname, sourcesdir, SymbolicLink.Directory);
                        if (succ)
                        {
                            ExecuteJavaScript("loadmodel('" + basepath + "')");
                            ExecuteJavaScript("setModelPath('" + filename + "')");
                        }
                    }
                });
                return new JavaScriptValue(text);
            }));
            RegisterJavaScriptObject("localrun", obj);
        }
    }
}


