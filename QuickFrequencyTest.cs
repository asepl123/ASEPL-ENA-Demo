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
    [Display("QuickFrequencyTest", Group: "ENA_Demo", Description: "Insert a description here")]
    public class QuickFrequencyTest : TestStep
    {
        #region Settings
        private OpenTap.TraceSource LogS_Parameter = OpenTap.Log.CreateSource("S-Parameters Log");
        [Display("Instrument", Group: "Instrument Setting", Description: "Get S parameter fro specified Frequencies points.", Order: 1.1)]
        public E5080B MyENA_Instrument { get; set; }

        [Display("S Parameters", Group: "Measurement Selection", Description: "Select the S Parameters that you want to test", Order: 2.1)]
        public S_Parameters _s_parameters { get; set; }

        [Display("Marker 1 Frequency", Group: "Marker Settings", Description: "Enter the Marker 1 Frequency", Order: 3.1)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double Marker_1_Frequency { get; set; }

        [Display("Marker 2 Frequency", Group: "Marker Settings", Description: "Enter the Marker 2 Frequency", Order: 3.2)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double Marker_2_Frequency { get; set; }

        [Display("Marker 3 Frequency", Group: "Marker Settings", Description: "Enter the Marker 3 Frequency", Order: 3.3)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double Marker_3_Frequency { get; set; }
        #endregion

        public QuickFrequencyTest()
        {
            Marker_1_Frequency = 1.9e9;
            Marker_2_Frequency = 2.2e9;
            Marker_3_Frequency = 2.5e9;
            _s_parameters = S_Parameters.S11;
        }
        public override void Run()
        {
            MyENA_Instrument.ReadMarkerVaules(_s_parameters, Marker_1_Frequency, Marker_2_Frequency, Marker_3_Frequency);
            string[] MarkerFrequencies = new string[] { Marker_1_Frequency.ToString(), Marker_2_Frequency.ToString(), Marker_3_Frequency.ToString() };
            string[] result_of_execution;
            result_of_execution = MyENA_Instrument.GetS_ParameterMarkerValues();

            string Marker1 = result_of_execution[1];
            string Marker2 = result_of_execution[2];
            string Marker3 = result_of_execution[3];

            LogS_Parameter.Info("*********************** Marker1 Value Parameters ******************");
            LogS_Parameter.Info("************************* " + Marker1 + " dB *************************");
            LogS_Parameter.Info("********************************************************");
            LogS_Parameter.Info("");
            LogS_Parameter.Info("");
            LogS_Parameter.Info("*********************** Marker2 Value Parameters ******************");
            LogS_Parameter.Info("************************* " + Marker2 + " dB *************************");
            LogS_Parameter.Info("********************************************************");
            LogS_Parameter.Info("");
            LogS_Parameter.Info("");
            LogS_Parameter.Info("*********************** Marker3 Value Parameters ******************");
            LogS_Parameter.Info("************************* " + Marker3 + " dB *************************");
            LogS_Parameter.Info("********************************************************");
            LogS_Parameter.Info("");
            LogS_Parameter.Info("");
            //Results.PublishTable("S-Parameter", new List<string> { "Result", "Mean Value", "Screenshot Path" }, result_of_execution);
            //Results.PublishTable(_s_parameters.ToString(), new List<string> { Marker_1_Frequency.ToString(), Marker_2_Frequency.ToString(), Marker_3_Frequency.ToString() }, result_of_execution);

            Results.Publish(_s_parameters.ToString() + "-Parameter", new List<string> { "DUT-Barcode", "Freq1: " + Marker_1_Frequency.ToString(), "Freq2: " + Marker_2_Frequency.ToString(), "Freq3: " + Marker_3_Frequency.ToString(), "Verdict" }, result_of_execution);

            //UpgradeVerdict(Verdict.Pass);
        }
    }
}
