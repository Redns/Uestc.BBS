# Lottie 动画生成流程

## 环境安装

命令行安装 code-gen 工具

```bash
dotnet tool install -g LottieGen
```

## 代码生成

获取 Lottie json 文件（https://app.lottiefiles.com/animation）

![Clip_2025-09-07_23-08-10](https://image.krins.cloud/80d27851da4335318241145d4782a132.png)

使用命令行工具生成代码

```bash
lottiegen -InputFile {json_file_path} -Language cs -WinUIVersion 3.0
```

复制生成的代码到项目中，将 sealed 修改为 public partial 以兼容 AOT（开头一处中间一处，少修改会导致 AOT 编译后无法运行）

![Clip_2025-09-07_23-09-03](https://image.krins.cloud/d8065cfbbce219b157d998949507331f.png)

![Clip_2025-09-07_23-09-23](https://image.krins.cloud/ff90de9bfbc5e6fb8e8699d8319e6705.png)

安装 `Microsoft.Graphics.Win2D` 包

![Clip_2025-09-07_23-10-34](https://image.krins.cloud/52a84edc43ffa4d112b15fe879c70618.png)

在 xaml 中引用动画资源

```xaml
...
xmlns:animatedvisuals="using:AnimatedVisuals"
...
<AnimatedVisualPlayer>
    <animatedvisuals:MyAnimation/>
</AnimatedVisualPlayer>
```