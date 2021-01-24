using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

internal class Build : NukeBuild
{
    // Console application entry. Also defines the default target.
    public static int Main() => Execute<Build>(x => x.Compile);

    // Auto-injection fields:

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly string Configuration = IsLocalBuild ? "Debug" : "Release";

    [Solution("src/HuanlinLib.sln")] private readonly Solution TheSolution;
    [GitRepository] private readonly GitRepository GitRepository;
    [GitVersion] private readonly GitVersion GitVersion;
    // Semantic versioning. Must have 'GitVersion.CommandLine' referenced.

    // Parses origin, branch name and head from git config.

    // [Parameter] readonly string MyGetApiKey;
    // Returns command-line arguments and environment variables.

    private AbsolutePath SourceDirectory => RootDirectory / "src";
    private AbsolutePath TestsDirectory => RootDirectory / "tests";
    private AbsolutePath OutputDirectory => RootDirectory / "output";

    private Target Clean => _ => _             
             .Executes(() =>
             {
                 GlobDirectories(SourceDirectory, "**/bin", "**/obj").ForEach(DeleteDirectory);
                 GlobDirectories(TestsDirectory, "**/bin", "**/obj").ForEach(DeleteDirectory);
                 EnsureCleanDirectory(OutputDirectory);
             });

    private Target Restore => _ => _
             .DependsOn(Clean)
             .Executes(() =>
             {
                 DotNetRestore(s => s.SetProjectFile(TheSolution));
             });

    private Target Compile => _ => _
             .DependsOn(Restore)
             .Executes(() =>
             {
                 DotNetBuild(s => s
                     .SetProjectFile(TheSolution)
                     .EnableNoRestore()
                     .SetConfiguration(Configuration)
                     .SetAssemblyVersion(GitVersion.AssemblySemVer)
                     .SetFileVersion(GitVersion.AssemblySemVer)
                     .SetInformationalVersion(GitVersion.InformationalVersion));
             });

    private string ChangelogFile => RootDirectory / "CHANGELOG.md";

    private IEnumerable<string> ChangelogSectionNotes => ExtractChangelogSectionNotes(ChangelogFile);

    private Target Pack => _ => _
             .DependsOn(Compile)
             .Executes(() =>
             {

                 DotNetPack(s => s
                     .SetProject(TheSolution)
                     .EnableNoBuild()
                     .SetConfiguration(Configuration)
                     .EnableIncludeSymbols()
                     .SetOutputDirectory(OutputDirectory)
                     .SetVersion(GitVersion.NuGetVersionV2)
                     .SetPackageReleaseNotes(GetNuGetReleaseNotes(ChangelogFile, GitRepository)));
             });
}