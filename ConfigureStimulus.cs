// Author: Pawan Narwal
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

namespace ENA_Demo
{
    public enum Average_Mode
    {
        SWEEP,
        POINT
    }
    [Display("Configure Stimulus", Group: "ENA_Demo", Description: "Insert a description here")]
    public class ConfigureStimulus : TestStep
    {
        #region Settings
        private OpenTap.TraceSource LogS_Configure = OpenTap.Log.CreateSource("Configure Log");

        [Display("Instrument", Group: "Instrument Setting", Description: "Configure Network Analyzer", Order: 1.1)]
        public E5080B MyENA_Instrument { get; set; }

        [Display("Start Frequency", Group: "Test Case Settings", Description: "Enter Start Frequency", Order: 2.1)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double StartFrequency { get; set; }

        [Display("Stop Frequency", Group: "Test Case Settings", Description: "Enter Stop Frequency", Order: 2.2)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double StopFrequency { get; set; }

        [Display("IFBandwidth", Group: "Test Case Settings", Description: "Enter IF Bandwidth", Order: 2.3)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFBandwidth { get; set; }

        [Display("Sweep Points", Group: "Test Case Settings", Description: "Enter Sweep Points", Order: 2.4)]
        public int Sweep_Points { get; set; }

        [Display("Power Level", Group: "Test Case Settings", Description: "Enter the value of the power level in dBm", Order: 2.5)]
        [Unit("dBm")]
        public double Power_Level { get; set; }


        [Display("Averaging", Group: "Advanced Test Case Settings", Description: "Enable/Disable Averaging", Order: 3.1)]
        public bool Averaging { get; set; }

        [Display("Averaging Factor", Group: "Advanced Test Case Settings", Description: "Enter the Averaging Factor", Order: 3.2)]
        [EnabledIf("Averaging", true, HideIfDisabled = true)]
        public int Averaging_Factor { get; set; }

        [Display("Average Mode", Group: "Advanced Test Case Settings", Description: "Select the Average Mode", Order: 3.3)]
        [EnabledIf("Averaging", true, HideIfDisabled = true)]
        public Average_Mode _Averaging_Mode { get; set; }

        [Display("S Parameters", Group: "Measurement Selection", Description: "Select the S Parameters that you want to test", Order: 4.1)]
        public S_Parameters _s_parameters { get; set; }
        #endregion

        public ConfigureStimulus()
        {
            StartFrequency = 100e3;
            StopFrequency = 4.5e9;
            IFBandwidth = 10e3;
            Sweep_Points = 51;
            Power_Level = -10;
            Averaging_Factor = 10;
            _s_parameters = S_Parameters.S11;
        }

        public override void Run()
        {
            LogS_Configure.Info("Selected Start Frequency :" + StartFrequency.ToString());
            LogS_Configure.Info("Selected Stop Frequency :" + StopFrequency.ToString());
            LogS_Configure.Info("Selected IF Bandwidth :" + IFBandwidth.ToString());
            LogS_Configure.Info("Selected Power Level :" + Power_Level.ToString());

            MyENA_Instrument.ENAConfigure(StartFrequency, StopFrequency, IFBandwidth, Sweep_Points, Power_Level, _s_parameters);
        }
    }
}
