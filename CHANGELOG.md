# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [5.4.0] - 2025-11-15
 - Upgrade to .NET 10.0.
 - Change Huanlin.Windows project's target OS version: Windows 10.

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