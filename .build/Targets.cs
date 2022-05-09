using System;
using System.IO;
using System.Linq;
using K4os.FakeNukeBridge;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.Xunit;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Targets: NukeBuild
{
	/// Support plugins are available for:
	///   - JetBrains ReSharper        https://nuke.build/resharper
	///   - JetBrains Rider            https://nuke.build/rider
	///   - Microsoft VisualStudio     https://nuke.build/visualstudio
	///   - Microsoft VSCode           https://nuke.build/vscode
	public static int Main() => Execute<Targets>(x => x.Build);

	[Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
	readonly Configuration Configuration =
		IsLocalBuild ? Configuration.Debug : Configuration.Release;

	[Solution] readonly Solution Solution;
	[GitRepository] readonly GitRepository GitRepository;

	AbsolutePath SourceDirectory => RootDirectory / "src";
	AbsolutePath OutputDirectory => RootDirectory / ".output";

	SettingsFile Secrets =
		SettingsFile.TryParseFile(RootDirectory / ".secrets.cfg") ?? 
		SettingsFile.Parse(string.Empty);
	
	ReleaseNotes ReleaseNotes = 
		ReleaseNotes.ParseFile(RootDirectory / "CHANGES.md");

	Target Clean => _ => _
		.Before(Restore)
		.Executes(() =>
		{
			Log.Information("Git: {Repository}", GitRepository);
			SourceDirectory
				.GlobDirectories("**/bin", "**/obj")
				.ForEach(DeleteDirectory);
			RootDirectory
				.GlobDirectories("**/BenchmarkDotNet.Artifacts")
				.ForEach(DeleteDirectory);
			EnsureCleanDirectory(OutputDirectory);
		});

	Target Restore => _ => _
		.After(Clean)
		.Executes(() =>
		{
			DotNetRestore(s => s
				.SetProjectFile(Solution));
		});

	Target Build => _ => _
		.DependsOn(Restore)
		.Executes(() =>
		{
			DotNetBuild(s => s
				.SetProjectFile(Solution)
				.SetConfiguration(Configuration)
				.EnableNoRestore());
		});
	
	Target Test => _ => _
		.DependsOn(Build)
		.Executes(() =>
		{
			DotNetTest(s => s
				.SetProjectFile(Solution)
				.SetConfiguration(Configuration)
				.EnableNoRestore()
				.EnableNoBuild()
				.SetVerbosity(DotNetVerbosity.Normal));
		});

	Target Release => _ => _
		.DependsOn(Clean)
		.DependsOn(Restore)
		.Executes(() =>
		{
			var store = new ValueStore(OutputDirectory / ".release");
			
			DotNetPack(s => s
				.SetProject(Solution)
				.SetConfiguration(Configuration.Release)
				.SetOutputDirectory(OutputDirectory)
				.SetVersion(ReleaseNotes.NugetVersion)
				.SetAssemblyVersion(ReleaseNotes.FileVersion)
				.SetFileVersion(ReleaseNotes.FileVersion)
				.EnableNoRestore());

			store["commit"] = GitRepository.Commit;
			store["version"] = ReleaseNotes.NugetVersion;
			store.Commit();
		});

	Target Publish => _ => _
		.Executes(() =>
		{
			var store = new ValueStore(OutputDirectory / ".release");

			if (!GitTasks.GitHasCleanWorkingCopy())
				throw new Exception("Git working copy is not clean");
			
			if (!GitRepository.IsOnMainOrMasterBranch())
				throw new Exception("Releases should be done from the master branch");

			var commit = store["commit"];
			if (GitRepository.Commit != commit)
				throw new Exception("Release was done with another commit");

			GitTasks.Git("push");

			Console.WriteLine("!!!");
			var version = store["version"];
			Console.WriteLine($"!!! {version}");
			foreach (var package in GlobFiles(OutputDirectory, $"*.{version}.nupkg"))
			{
				Console.WriteLine($"!!! {package}");
				DotNetNuGetPush(s => s.SetTargetPath(package));
			}
		});
}
