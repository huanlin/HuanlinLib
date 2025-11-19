# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [5.5.1] - 2025-11-18

- Removed `JsonHelper.cs`.

## [5.5.0] - 2025-11-16

- 刪除 `HttpContentParser.cs` 和 `Base64.cs` 檔案，因其功能已被 .NET 內建函式庫取代或已過時。
- 修正 `HttpMultipartParser.cs` 中的編譯錯誤，並將 Regex 匹配範圍修正為針對當前區塊。
- 將所有 `Encoding.Default` 的使用替換為 `Encoding.UTF8`，以確保跨平台的編碼一致性。
- 將 `JsonHelper.cs` 升級為使用 `System.Text.Json` 進行 JSON 序列化和反序列化。
- 修正 `Base36.cs` 中 `Decode` 方法的效能問題，改用乘法累加取代 `Math.Pow`。
- 改善 `CharHelper.cs` 中字元判斷方法的可讀性，改用直接字元比較。
- 修正 `NetHelper.cs` 中 `IsServerConnectable` 方法的效能問題，改用 `IAsyncResult.AsyncWaitHandle.WaitOne` 取代忙碌等待。
- 改善 `FileHelper.cs` 中 `FindFiles` 方法的效能，改用 `HashSet<string>` 處理檔案列表。
- 將 `SmartDate` 類別標記為 `[Obsolete]`，建議改用 `Nullable<DateTime>` (即 `DateTime?`)。
- 在 `StrHelper.cs` 的 MD5 相關方法中加入註解，說明 MD5 演算法的不安全性。
- 將 `FileRunner.cs` 類別標記為 `sealed`，並將 `WaitTime` 屬性改為 `TimeSpan` 型別。
- 重構 `FileRunner.cs`，引入 `RunAsync` 非同步方法，移除 `Thread.Sleep`，並統一錯誤處理。
- 將 `MsIme.cs` 中的 `IFELanguage` 介面標記為 `[Obsolete]`，建議改用 Text Services Framework (TSF)。

## [5.4.2] - 2025-11-15

- Upgrade to .NET 10.0.
- Change Huanlin.Windows project's target OS version: Windows 10.
- Breaking change: HttpUpdate.GetUpdateListAsync 方法現在需要傳入更新清單的檔案名稱。

## [5.3.4] - 2025-06-29

- 轉換所有 C# 檔案為 file-scoped namespaces。(Gemini CLI)
- 改變發布時的版本編號方式：使用 MinVer 自動管理。 (Gemini CLI)
- Improve GitHub Actions workflow 中的打包指令。 (Gemini CLI)
- Upgrade PhoneticTools projects to .NET 9.
- Change all files' encding to UTF-8.
- Added more unit tests. (Gemini CLI)

## [5.3.3] - 2025-06-22

- Upgrade to .NET 9.0.
- No more Huanlin.AppBlock. Code are move to Huanlin.Common project.	 

## [5.2.0] - 2022-10-10

- Upgrade to .NET 7.


## [5.1.0] - 2022-02-05

- Upgrade to .NET 6.0.
- Change build system from Nuke to FlubuCore.
- Use MinVer package instead of GitVersion.
- Add AppVeyor for CI and auto publish NuGet packages to nuget.org.

## [5.0.0] - 2021-01-24

### Changed

- Upgraded to .NET 5.0. Some functions are removed due to outdated or rarely used.
- Upgraded references packages.

## [4.5.11] - 2018-12-13

### Added

- HttpUpdater now supports updating files in subfolders.
- Added CHANGELOG.md based on https://keepachangelog.com/en/1.0.0/

### Removed

- Removed RichTextBoxHL class. ScintillaNET is recommended if you need a better rich edit control.