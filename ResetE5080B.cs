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
    [Display("ResetE5080B", Group: "ENA_Demo", Description: "Insert a description here")]
    public class ResetE5080B : TestStep
    {
        #region Settings
        [Display("Instrument", Group: "Instrument Setting", Description: "Reset Network Analyzer", Order: 1.1)]
        public E5080B MyENA_Instrument { get; set; }
        #endregion

        public ResetE5080B()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            MyENA_Instrument.ResetInstrument();
        }
    }
}
