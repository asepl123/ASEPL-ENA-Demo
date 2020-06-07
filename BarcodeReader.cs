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
using Microsoft.VisualBasic;

namespace ENA_Demo
{
    [Display("BarcodeReader", Group: "ENA_Demo", Description: "Insert a description here")]
    public class BarcodeReader : TestStep
    {
        #region Settings
        private OpenTap.TraceSource Log_BarcodeReader = OpenTap.Log.CreateSource("Barcode Reader Logs");
        #endregion

        public BarcodeReader()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            string BarcodeInput = Interaction.InputBox("Scan Barcode here", "Barcode Scanner", "", -1, -1);
            Log_BarcodeReader.Info("*********************** Barcode Reading **************");
            Log_BarcodeReader.Info("*************************>>  " + BarcodeInput + "  <<*****************************");
        }
    }
}
