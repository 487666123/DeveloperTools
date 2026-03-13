# Developer Tools

`Developer Tools` 是一个 tModLoader 辅助开发者开发模组的模组。

## 构建说明

`DeveloperTools.csproj` 依赖同级/上级目录中的以下内容：

- `../tModLoader.targets`
- `../SilkyUIFramework/SilkyUIFramework.csproj`
- `../SilkyUIAnalyzer/SilkyUIAnalyzer.csproj`

因此，这个项目应放在现有的 tModLoader 模组源码工作区中进行构建。

## 目录概览

- `Modules/`：功能模块
- `UserInterfaces/`：UI
- `Localization/`：本地化文本

本文档只描述当前仓库中可以直接从代码和配置确认的内容。
