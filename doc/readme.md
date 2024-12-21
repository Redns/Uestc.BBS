# 版本号

应用版本号格式为 `Major.Minor.Build.Release<-beta>`

- `Major`：主版本号，表示 **重大更新或不兼容的 API 改动**。当软件进行了重大的架构改变时递增，同时 Minor 和 Build 清零
- `Minor`：次版本号，表示 **向后兼容的重要功能更新**。当软件添加了新功能或进行了一些较大的改进时递增，同时 Build 和 Revision 清零
- `Build`：构建号，用于标识在同一次版本中的不同构建，通常用于修复 bug 或进行小的改进
- `Release`：发布日期与应用起始创建时间（2024.7.16）的间隔（天）

版本号后缀 `beta` 表示该版本属于预览版，可能存在功能性问题

## 桌面版

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

`Appmanifest` 已进行依赖注入，使用时直接获取即可。如果需要自行加载，示例代码如下

```csharp
var appmanifest = JsonSerializer.Deserialize<Appmanifest>(
    ResourceHelper.Load("/Assets/appmanifest.json"),
    Appmanifest.SerializerOptions
);
```

新版本发布需遵循如下修改

1. 修改 `appmanifest.json` 中的 `Version` 字段，请务必遵循上述版本约定
2. 修改 `Contributors` 字段，贡献者列表包括代码贡献、设计原型、建议、问题反馈等等