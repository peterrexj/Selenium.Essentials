using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.Essentials.Model
{
    public class BrowserInformationModel
    {
        public string Name { get; set; }
        public string InstallationPath { get; set; }
        public string IconPath { get; set; }
        public string BrowserVersion { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} {Environment.NewLine}Browser Version: {BrowserVersion} {Environment.NewLine}Installation Path: {InstallationPath}{Environment.NewLine}IconPath: {IconPath}";
        }
    }
}
