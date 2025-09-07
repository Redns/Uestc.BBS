# 版本号

应用版本号格式为 `Major.Minor.Build.Release<-beta>`

- `Major`：主版本号，表示 **重大更新或不兼容的 API 改动**。当软件进行了重大的架构改变时递增，同时 Minor 和 Build 清零
- `Minor`：次版本号，表示 **向后兼容的重要功能更新**。当软件添加了新功能或进行了一些较大的改进时递增，同时 Build 清零
- `Build`：构建号，用于标识在同一次版本中的不同构建，通常用于修复 bug 或进行小的改进
- `Release`：发布日期与应用起始创建时间（2024.7.16）的间隔（天）

版本号后缀 `beta` 表示该版本属于预览版，可能存在功能性问题

# 桌面端

桌面版应用相关信息存储在 `Assets/appmanifest.json`，示例内容如下

```json
{
  "Version": "0.0.0.113-beta",
  "OriginalDate": "2024-07-16",
  "Contributors": [
    {
      "Name": "Redns",
      "Avatar": "/Assets/Contributors/Redns.jpg",
      "HomePage": "https://github.com/Redns",
      "Description": "Future is now"
    }
  ]
}
```

`Appmanifest` 已进行依赖注入，使用时直接获取即可

```csharp
var appmanifest = ServiceExtension.Services.GetRequiredService<Appmanifest>();
```

如果需要自行加载，示例代码如下（以 Avalonia 为例）

```csharp
var appmanifest = JsonSerializer.Deserialize<Appmanifest>(
    ResourceHelper.Load("/Assets/appmanifest.json"),
    AppmanifestContext.Default.Appmanifest
);
```

新版本发布需遵循如下修改

1. 修改 `appmanifest.json` 中的 `Version` 字段，请务必遵循上述版本约定
2. 修改 `Contributors` 字段，贡献者列表包括代码贡献、设计原型、建议、问题反馈等等

# 应用发布

应用发布流程如下

1. 将代码编译为可执行文件
2. 将可执行文件打包为平台特定安装包（如 Windows 下打包为 exe，Linux 下打包为 deb）

## 环境依赖

编译此项目前请参照 [官方文档](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=macOS%2Cnet8) 配置环境，对于 Ubuntu 平台需安装如下依赖

```sh
sudo apt-get install clang zlib1g-dev
```

## 编译与打包

桌面端启用 `Navite-AOT` 编译，需要在特定平台编译（例如 Linux 可执行文件只能在 Linux 平台编译）

### Windows

若使用 `Visual Studio` 开发则选中 Uestc.BBS.Desktop/Uestc.BBS.WinUI，右键点击发布

![image-20241223110112938](https://image.krins.cloud/f9ec8f991ad1d4d548aa2709c83fbb39.png)

配置内容如下

![image-20241223112455898](https://image.krins.cloud/24ab756a4dfdb3a4114587740b18fb97.png)

点击发布，等待应用编译完成

![image-20241223112659340](https://image.krins.cloud/c9a2d1a3507f2de6519f6aaedd6df1dd.png)

打开编译输出目录，其中 pdb 调试文件可直接删除

![image-20241223112902965](https://image.krins.cloud/9c304cee0b805ee84cfeee7902c94d93.png)

### Linux

命令行进入 Uestc.BBS.Desktop 目录，执行如下命令编译

```sh
dotnet piblish -r linux-x64 -c Release
```

## 分发