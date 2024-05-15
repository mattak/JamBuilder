using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace JamBuilder
{
    public static class DefaultBuilder
    {
        public static void BuildDebug()
        {
            Build(false);
        }

        private static void Build(bool isReleaseBuild)
        {
            var buildFolder = "Build/Debug";
            var scenes = GetScenes();
            var options = CreateBuildPlayerOptions(isReleaseBuild, buildFolder, scenes);

            Mkdirp(buildFolder);

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("INFO:\tBuild succeeded: " + summary.totalSize + " bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError("INFO:\tBuild failed");
            }
        }

        private static void Mkdirp(string name)
        {
            if (!Directory.Exists(name))
            {
                Directory.CreateDirectory(name);
            }
        }

        private static string[] GetScenes()
        {
            var sceneList = new List<string>();
            var scenes = EditorBuildSettings.scenes;

            foreach (EditorBuildSettingsScene scene in scenes)
            {
                sceneList.Add(scene.path);
                Debug.Log(scene.path);
            }

            return sceneList.ToArray();
        }

        private static BuildPlayerOptions CreateBuildPlayerOptions(
            bool isRelease,
            string locationPathName,
            string[] scenes
        )
        {
            var buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes;

            buildPlayerOptions.locationPathName = locationPathName;

            if (isRelease)
            {
                buildPlayerOptions.options = BuildOptions.Development;
            }

            buildPlayerOptions.target = BuildTarget.WebGL;
            return buildPlayerOptions;
        }
    }
}