using System;
using FlubuCore.Context;
using FlubuCore.Context.Attributes.BuildProperties;
using FlubuCore.IO;
using FlubuCore.Scripting;

namespace Build
{
    public class BuildScript : DefaultBuildScript
    {
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
                .AddCoreTask(x => x.Pack());
        }
    }
}