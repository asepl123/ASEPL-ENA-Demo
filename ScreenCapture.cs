// Author: MyName
// Copyright:   Copyright 2020 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenTap;
using System.Threading;

namespace ENA_Demo
{
    [Display("ScreenCapture", Group: "ENA_Demo", Description: "Insert a description here")]
    public class ScreenCapture : TestStep
    {
        #region Settings
        [Display("Instrument", Group: "Instrument Setting", Description: "Step to Capture NA Screen", Order: 1.1)]
        public E5080B MyENA_Instrument { get; set; }

        private string folderLocation = @"C:\Temp\";
        [DisplayAttribute("Folder path", "", "Enter image store folder path", 2)]
        public string FolderLocation { get => folderLocation; set => folderLocation = value; }
        #endregion

        public ScreenCapture()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            MyENA_Instrument.IoTimeout = 5000;
            Thread.Sleep(2000);
            MyENA_Instrument.ScreenCapture(FolderLocation);
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
