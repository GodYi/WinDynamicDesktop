﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using RestSharp;

namespace WinDynamicDesktop
{
    public class GitHubApiData
    {
        public string url { get; set; }
        public string html_url { get; set; }
        public string assets_url { get; set; }
        public string upload_url { get; set; }
        public string tarball_url { get; set; }
        public string zipball_url { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string tag_name { get; set; }
        public string target_commitish { get; set; }
        public string name { get; set; }
        public string body { get; set; }
        public bool draft { get; set; }
        public bool prerelease { get; set; }
        public DateTime created_at { get; set; }
        public DateTime published_at { get; set; }
    }

    class UpdateChecker
    {
        private static string updateLink = "https://github.com/t1m0thyj/WinDynamicDesktop/releases";

        private static string GetLatestVersion()
        {
            var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/repos/t1m0thyj/WinDynamicDesktop/releases/latest", Method.GET);

            var response = client.Execute<GitHubApiData>(request);
            if (!response.IsSuccessful)
            {
                return null;
            }

            return response.Data.tag_name.Substring(1);
        }

        private static string GetCurrentVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private static bool IsUpdateAvailable(string currentVersion, string latestVersion)
        {
            Version current = new Version(currentVersion);
            Version latest = new Version(latestVersion);

            return (latest > current);
        }

        public static void CheckManual()
        {
            string currentVersion = GetCurrentVersion();
            string latestVersion = GetLatestVersion();

            if (IsUpdateAvailable(currentVersion, latestVersion))
            {
                DialogResult result = MessageBox.Show("There is a newer version of WinDynamicDesktop " +
                    "available. Would you like to visit the download page?" + Environment.NewLine +
                    Environment.NewLine + "Current Version: " + currentVersion + Environment.NewLine +
                    "Latest Version: " + latestVersion, "Update Available", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(updateLink);
                }
            }
            else
            {
                MessageBox.Show("You already have the latest version of WinDynamicDesktop installed.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
