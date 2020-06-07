// Author: Pawan Narwal
// Copyright:   Copyright 2020 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files..

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenTap;

namespace ENA_Demo
{
    [Display("Diplexer", Group: "ENA_Demo", Description: "Add a description here")]
    public class Diplexer : Dut
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change.
        #endregion

        /// <summary>
        /// Initializes a new instance of this DUT class.
        /// </summary>
        public Diplexer()
        {
            Name = "MyDUT";
            // ToDo: Set default values for properties / settings.
        }

        /// <summary>
        /// Opens a connection to the DUT represented by this class
        /// </summary>
        public override void Open()
        {
            base.Open();
            // TODO: establish connection to DUT here
        }

        /// <summary>
        /// Closes the connection made to the DUT represented by this class
        /// </summary>
        public override void Close()
        {
            // TODO: close connection to DUT
            base.Close();
        }
    }
}
