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
    [Display("LimitLineTest", Group: "ENA_Demo", Description: "Insert a description here")]
    public class LimitLineTest : TestStep
    {
        #region Settings
        [Display("Instrument", Group: "Instrument Setting", Description: "Limit Line test for Network Analyzer", Order: 1.1)]
        public E5080B MyENA_Instrument { get; set; }

        [Display("MAX Begin Stimulus", Group: "Limit Settings", Description: "Enter the MAX Begin Stimulus value in Hz", Order: 7.5)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double Max_Begin_Stimulus { get; set; }

        [Display("MAX End Stimulus", Group: "Limit Settings", Description: "Enter the MAX End Stimulus value in Hz", Order: 7.6)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double Max_End_Stimulus { get; set; }

        [Display("MAX Begin Response", Group: "Limit Settings", Description: "Enter the MAX Begin Response value", Order: 7.7)]
        [Unit("", true)]
        //  [Unit("Hz", UseEngineeringPrefix: true)]
        public double Max_Begin_Response { get; set; }

        [Display("MAX End Response", Group: "Limit Settings", Description: "Enter the MAX End Response value", Order: 7.8)]
        [Unit("", true)]
        // [Unit("Hz", UseEngineeringPrefix: true)]
        public double Max_End_Response { get; set; }

        [Display("MIN Begin Stimulus", Group: "Limit Settings", Description: "Enter the MIN Begin Stimulus value in Hz", Order: 7.1)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double Min_Begin_Stimulus { get; set; }

        [Display("MIN End Stimulus", Group: "Limit Settings", Description: "Enter the MIN End Stimulus value in Hz", Order: 7.2)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double Min_End_Stimulus { get; set; }

        [Display("MIN Begin Response", Group: "Limit Settings", Description: "Enter the MIN Begin Response value", Order: 7.3)]
        [Unit("", true)]
        // [Unit("Hz", UseEngineeringPrefix: true)]
        public double Min_Begin_Response { get; set; }

        [Display("MIN End Response", Group: "Limit Settings", Description: "Enter the MIN End Response value", Order: 7.4)]
        [Unit("", true)]
        // [Unit("Hz", UseEngineeringPrefix: true)]
        public double Min_End_Response { get; set; }
        #endregion

        public LimitLineTest()
        {
            Max_Begin_Stimulus = 1.45e9;
            Max_End_Stimulus = 2.7e9;
            Max_Begin_Response = -5;
            Max_End_Response = -5;

            Min_Begin_Stimulus = 1.55e9;
            Min_End_Stimulus = 2.45e9;
            Min_Begin_Response = -11;
            Min_End_Response = -11;

            // The following writes three max limit segments for a bandpass filter.
            // CALC:LIM:DATA 1,3e5,4e9,-60,0,1,4e9,7.5e9,0,0,1,7.5e9,9e9,0,-30

            //BegResp, EndResp
            string minLimit = "-11,-4";
            //CALC:LIM:DATA 1,100e3,1.4e9,-40,-40,1,1.4e9,1.45e9,-40,-5,1,1.45e9,2.55e9,-5,-5,1,2.55e9,2.7e9,-5,-40,1,2.7e9,4.5e9,-40,-40,2,1.55e9,2.45e9,-11,-11
            //CALC:MEAS:LIM:STAT ON
            //CALC:MEAS:LIM:FAIL? //0-->Pass, 1-->Fail
        }

        Random LimitLineDutCode = new Random();
        string LimitLineVerdict = "";

        public override void Run()
        {
            MyENA_Instrument.ApplyLimits(Max_Begin_Stimulus, Max_End_Stimulus, Max_Begin_Response, Max_End_Response, Min_Begin_Stimulus, Min_End_Stimulus, Min_Begin_Response, Min_End_Response);
            bool GetPassFail = MyENA_Instrument.GetPassFailResult();
            //CALC:MEAS:LIM:FAIL? //0-->Pass, 1-->Fail

            int LimitLineDutNumber = LimitLineDutCode.Next();

            if (GetPassFail)
            {
                UpgradeVerdict(Verdict.Fail);
                LimitLineVerdict = "Fail";
            }
            else
            {
                UpgradeVerdict(Verdict.Pass);
                LimitLineVerdict = "Pass";
            }
            string[] LimitLine_Results = new string[] { LimitLineDutNumber.ToString(), LimitLineVerdict };

            Results.Publish("LimitLineTest", new List<string> { "DUT-Barcode", "Verdict" }, LimitLine_Results);

        }
    }
}
