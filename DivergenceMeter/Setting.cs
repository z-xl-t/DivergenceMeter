using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace DivergenceMeter
{
    public class Setting
    {
        public string READMEMD_1 { get; set; }
        public double BaseWidth { get; set; } = 288;
        public double BaseHeight { get; set; } = 92;
        public double Scale { get; set; }
        public string READMEMD_2 { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Opacity { get; set; }
        public bool AlwaysInTopStatus { get; set; }
        public string READMEMD_3 { get; set; }
        public bool ClickThroughStatus { get; set; }
        public bool DragMoveStatus { get; set; }
        public bool EdgeAttachStatus { get; set; }
        public string READMEMD_4 { get; set; }
        public bool StartUpStatus { get; set; }

        public Setting()
        {
            READMEMD_1 = "基本宽度*缩放比例应该等于宽度";
            READMEMD_2 = "宽度宽度和高度的比例为 3：1， 并且窗体的实际宽高就是由此属性决定";
            READMEMD_3 = "点击穿透功能高于允许拖动功能，即能拖动时必不穿透";
            READMEMD_4 = "开机自启功能需要往注册表里面写程序的启动路径";
            Scale = 1.0;
            Width = BaseWidth * Scale;
            Height = BaseHeight * Scale;
            Left = 0;
            Top = 0;
            Opacity = 1.0;
            StartUpStatus = false;
            ClickThroughStatus = false;
            EdgeAttachStatus = false;
            DragMoveStatus = true;
            AlwaysInTopStatus = true;
        }
        public static bool SaveSetting(String path,Setting setting)
        {
            string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            File.WriteAllText(path, json, Encoding.UTF8);
            return true;
        }
        public static bool LoadSetting(String path, out Setting Setting)
        {
            string settingJson = File.ReadAllText(path, Encoding.UTF8);
            Setting = JsonConvert.DeserializeObject<Setting>(settingJson);
            return true;
        }
    }
}
