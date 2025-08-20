# Lottie 动画生成流程

## 环境安装

命令行安装 code-gen 工具

```bash
    dotnet tool install -g LottieGen
```
    
1. 获取 Lottie json 文件（https://app.lottiefiles.com/animation）

2. 使用命令行工具生成代码
    
```bash
    lottiegen -InputFile {json_file_path} -Language cs -WinUIVersion 3.0
```

3. 复制生成的代码到项目中，将 sealed 修改为 public partial 以兼容 AOT

开头一处中间一处，少修改会导致 AOT 编译后无法运行

4. 安装 Microsoft.Graphics.Win2D Nuget 包

5. 在 xaml 中引用动画资源

```xaml
    ...
    xmlns:animatedvisuals="using:AnimatedVisuals"
    ...
    <AnimatedVisualPlayer>
        <animatedvisuals:MyAnimation/>
    </AnimatedVisualPlayer>
```