[![Build status](https://ci.appveyor.com/api/projects/status/5qv789s4aar9cpt7?svg=true)](https://ci.appveyor.com/project/huanlin/huanlinlib)

# HuanlinLib

包含三個套件：

- [Huanlin.Common](https://www.nuget.org/packages/Huanlin.Common/)
- [Huanlin.Windows](https://www.nuget.org/packages/Huanlin.Windows/)
- [Huanlin.AppBlock](https://www.nuget.org/packages/Huanlin.AppBlock/)

最近一次的主要更新：

- 升級至 .NET 6。
- 解決過時的 API calls，例如 WebClient 改用 HttpClient。
- 加入 MinVer 套件來自動處理應用程式的版本編號。
- 加入 AppVeyor.yml 來自動建置和發布 NuGet 套件至 nuget.org。

欲查看詳細修改紀錄，請參考 [CHANGELOG.md](CHANGELOG.md)。