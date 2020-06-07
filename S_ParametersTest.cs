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
    public enum S_Parameters
    {
        S11,
        S12,
        S21,
        S22
    }

    [Display("S_ParametersTest", Group: "ENA_Demo", Description: "Insert a description here")]
    public class S_ParametersTest : TestStep
    {
        #region Settings
        private OpenTap.TraceSource LogS_Parameter = OpenTap.Log.CreateSource("S-Parameters Log");
        [Display("Instrument", Group: "Instrument Setting", Description: "Step for S-Parameter Measurement", Order: 1.1)]
        public E5080B MyENA_Instrument { get; set; }

        [Display("S Parameters", Group: "Measurement Selection", Description: "Select the S Parameters that you want to test", Order: 5.1)]
        public S_Parameters _s_parameters { get; set; }        
        #endregion

        public S_ParametersTest()
        {
            _s_parameters = S_Parameters.S21;
        }
        public override void Run()
        {
            LogS_Parameter.Info("Selected Parameter :" + _s_parameters.ToString());
            MyENA_Instrument.SelectS_Parameter(_s_parameters);

            List<string> allParameters = MyENA_Instrument.SParameter();
            Results.Publish( _s_parameters.ToString() + " Plot", allParameters);

        }
    }
}
