#addin "Cake.Docker"
#tool "nuget:?package=GitVersion.CommandLine"
#addin "Cake.Json"

var target = Argument("target", "Default");
var tag = Argument("tag", "cake");
var projectJson = "../src/Auth0/project.json";

var imageName = "auth0";
GitVersion versionInfo = null;



Task("Version")
  .Does(() => {
  
    versionInfo = GitVersion(new GitVersionSettings{ OutputType = GitVersionOutput.Json });
    
    var config = ParseJsonFromFile(new FilePath(projectJson));
  
    config["version"] = versionInfo.SemVer;

    SerializeJsonToFile(new FilePath(projectJson), config); 

});

Task("Restore")
 .IsDependentOn("Version")
  .Does(() =>
{
  DotNetCoreRestore("../src");
});

Task("Build")
  .IsDependentOn("Restore")
  .Does(() =>
{
    DotNetCoreBuild("../src/**/project.json");
});


Task("Publish")
  .IsDependentOn("Build")
  .Does(() =>
{
    var settings = new DotNetCorePublishSettings
    {
        Framework = "netcoreapp1.0",
        Configuration = "Release",
        OutputDirectory = "./publish/"
    };
                
    DotNetCorePublish("../src/Auth0", settings);
});


Task("Dockerize")
  .IsDependentOn("Publish")
  .Does(() => 
  {
    var name = String.Format("{0}-{1}", imageName, versionInfo.SemVer); 
    
    var settings = new DockerBuildSettings
    {
      Tag = new string[] {name}
    };
    
    DockerBuild(settings, ".");
});


Task("Default")
    .IsDependentOn("Dockerize");

RunTarget(target);