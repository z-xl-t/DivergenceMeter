## 命运石之门时钟挂件

### 开发相关

开发工具：`vistual studio 2019 Community `

开发语言：`C#`

开发环境：`Window 10 2004 && .net core 3.1`

开发技术: `Windows Presentation Foundation (WPF)`

开发依赖：`Hardcodet.NotifyIcon.Wpf   Newtonsoft.Json`

### 实现功能

- [x] 开机自启
- [x] 点击穿透
- [x] 拖动允许
- [x] 边缘吸附
- [x] 窗口置顶
- [x] 透明度设置
- [x] 世界线变动效果（鼠标右击）

### 使用说明

0.    直接从 exe 文件夹中下载安装，即可食用。如果想查看并修改源代码，执行下一步。

1. 从 [Github.com](https://github.com/sanshiliuxiao/DivergenceMeter) 或者 [Gitee.com](https://gitee.com/sanshiliuxiao/DivergenceMeter) 下载源代码

2. 如果你使用 `dotnet cli` ，请在 `shell` 下 输入如下命令:

   ```powershell
   // cmd or powershell or git bash etc
   // path .../DivergenceMeter/
   
   dotnet restore
   dotnet run --project DivergenceMeter
   ```

3. 如果你使用 `vistual studio 2019`，点击 `DivergenceMeter.sln` 解决方案，等待自动恢复项目的依赖项和工具 ，最后调试项目或者发布应用。


### 项目说明 

项目参考于 [Rainmeter](https://www.rainmeter.net/) 的 [DIvergence Meter](https://drive.google.com/file/d/1pM1XvJ_R0ege6ou9GwL-YDuK5GwXn3JR/view?usp=sharing) 皮肤插件，皮肤作者：[waicool20](https://github.com/waicool20)

大约一个月学习 C# 和 WPF 的成果，但这个挂件应用没有用到 WPF 的 MVVM 机制，因为在 WPF 中对象和 UI 之间的绑定（太难理解了），这跟前端框架 VUE 不太一样。

因此，这些效果的出现全都是直接使用事件驱动的方式，当事件触发后给 UI 设置对应的 属性。

### 补充说明

欢迎 PR，欢迎 Issues。

