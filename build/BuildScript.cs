using System;
using FlubuCore.Context;
using FlubuCore.Context.Attributes.BuildProperties;
using FlubuCore.IO;
using FlubuCore.Scripting;

/* 
 * 此範例展示 FlubuCore 搭配 MinVer 來指定應用程式的版本編號。已使用 FlubuCore v6.3.2 測試過。
 * 做法很簡單，只要把 MinVer 套件加入應用程式專案就行了，然後用 git tag 命令來指定應用程式的版本。
 * 除此之外，不用加入其他任何設定，建置腳本裡面也不用寫任何與版本編號有關的程式碼。
 */
namespace Build
{
    public class BuildScript : DefaultBuildScript
    {
        // 指定建置結果的輸出目錄。
        public FullPath OutputDirectory => RootDirectory.CombineWith("output");

        [ProductId]
        public string ProductId { get; set; } = "HuanlinLib";

        // 指定 .sln 檔案。這裡加上了 "source/"，是因為我把建置專案放在 repository 的跟目錄。
        [SolutionFileName]
        public string SolutionFileName => RootDirectory.CombineWith("src/HuanlinLib.sln");

        [BuildConfiguration]
        public string BuildConfiguration { get; set; } = "Release"; // Debug or Release        

        protected override void ConfigureTargets(ITaskContext session)
        {
            var clean = session.CreateTarget("clean")
                .SetDescription("Cleaning solution output folder.")
                .AddCoreTask(x => x.Clean()
                    .CleanOutputDir());

            var compile = session.CreateTarget("compile")
                .SetDescription("編譯 solution。")
                .AddCoreTask(x => x.Build());

            var publish = session.CreateTarget("pack")
                .SetDescription("建立 NuGet 套件。")
                .DependsOn(compile)
                .AddCoreTask(x => x.Publish()
                    .OutputDirectory(OutputDirectory));
        }
    }
}