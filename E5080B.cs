using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenTap;
using System.Threading;
using System.IO;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace ENA_Demo
{
    [Display("E5080B", Group: "ENA_Demo", Description: "Insert a description here")]
    public class E5080B : ScpiInstrument
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public E5080B()
        {
            Name = "E5080A";
            // ToDo: Set default values for properties / settings.
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {
            base.Open();
            // TODO:  Open the connection to the instrument here

            //if (!IdnString.Contains("Instrument ID"))
            //{
            //    Log.Error("This instrument driver does not support the connected instrument.");
            //    throw new ArgumentException("Wrong instrument type.");
            // }
            string DummyDUTFile = @"""" + "C:\\Users\\pawkumar\\Documents\\DummyDut.s2p" + @"""";
            ScpiCommand("DIAG:DUMM:FILE DUT,{0}", DummyDUTFile);
        }

        /// <summary>
        /// Close procedure for the instrument.
        /// </summary>
        public override void Close()
        {
            // TODO:  Shut down the connection to the instrument here.
            base.Close();
        }

        public void LoadSimulatedDUT()
        {
            string fileName = @"""" + "C:\\Users\\pawkumar\\Documents\\DemoData.sta" + @"""";
            string DummyDUTFile = @"""" + "C:\\Users\\pawkumar\\Documents\\DummyDut.s2p" + @"""";
            ScpiCommand(":MMEMory:LOAD:STATe {0}", fileName);
            OpenTap.TapThread.Sleep(500);
            ScpiCommand("DIAG:DUMM:FILE DUT,{0}", DummyDUTFile);
            OpenTap.TapThread.Sleep(500);
        }
        public void ResetInstrument()
        {
            ScpiCommand("*RST");
            ScpiCommand("SYSTem:FPRESet");
        }
        public void DefineMeasurement(string WhichSParameter)
        {
            ScpiCommand("DISPlay:WINDow1:STATE ON");
            ScpiCommand("CALCulate:PARameter:DEFine:EXT 'MyMeas',{0}", WhichSParameter);
            ScpiCommand("DISPlay:WINDow1:TRACe1:FEED 'MyMeas'");
            ScpiCommand("DISPlay:WINDow:Y:AUTO");
            ScpiQuery<bool>("*OPC?");
            OpenTap.TapThread.Sleep(500);
        }
        public void ENAConfigure(double startFrequency, double stopFrequency, double iFBW, int SweepPoints, double PowerLevel, S_Parameters selectedS_Parameter)
        {
            DefineMeasurement(selectedS_Parameter.ToString());
            ScpiCommand("SENSe:FREQuency:STARt " + startFrequency);
            ScpiCommand("SENSe:FREQuency:STOP " + stopFrequency);
            ScpiCommand("SENSe:BANDwidth:RESolution " + iFBW);
            ScpiCommand("SENSe:SWEep:POINts " + SweepPoints);
            ScpiCommand("SOUR:POW:CENT " + PowerLevel);
        }

        public void SelectS_Parameter(S_Parameters WhichSParameter)
        {
            string selectedS_Parameter = @"""" + WhichSParameter.ToString() + @"""";
            ScpiCommand("CALCulate:MEASure:PARameter " + selectedS_Parameter);
            OpenTap.TapThread.Sleep(500);
            ScpiCommand("DISPlay:WINDow:Y:AUTO");
            //DISP:WIND:Y:AUTO
            OpenTap.TapThread.Sleep(500);
        }
        public void ReadMarkerVaules(S_Parameters _s_parameters, double ENA_Marker_1_Frequency, double ENA_Marker_2_Frequency, double ENA_Marker_3_Frequency)
        {
            SelectS_Parameter(_s_parameters);

            string ENA_Channel = "1";
            ScpiCommand("CALCulate{0}:MARKer{1}:STATe {2}", ENA_Channel, 1, 1);
            ScpiCommand("CALCulate{0}:MARKer{1}:STATe {2}", ENA_Channel, 2, 1);
            ScpiCommand("CALCulate{0}:MARKer{1}:STATe {2}", ENA_Channel, 3, 1);

            ScpiCommand("CALCulate{0}:MARKer{1}:X {2}", ENA_Channel, 1, ENA_Marker_1_Frequency);
            ScpiCommand("CALCulate{0}:MARKer{1}:X {2}", ENA_Channel, 2, ENA_Marker_2_Frequency);
            ScpiCommand("CALCulate{0}:MARKer{1}:X {2}", ENA_Channel, 3, ENA_Marker_3_Frequency);
        }
        Random rand = new Random();
        public string[] GetS_ParameterMarkerValues()
        {
            string marker1Value = ScpiQuery<String>(":CALC1:MARK1 1;:CALC1:MARK1:Y?");
            char[] splitchar = { ',' };
            string[] strArr = marker1Value.Split(splitchar);
            double marker1ValueDBL = Convert.ToDouble(strArr[0]);
            string Marker1_RoundOff = Convert.ToString(Math.Round(marker1ValueDBL, 3));

            string marker2Value = ScpiQuery<String>(":CALC1:MARK2 1;:CALC1:MARK2:Y?");
            string[] strArr2 = marker2Value.Split(splitchar);
            double marker2ValueDBL = Convert.ToDouble(strArr2[0]);
            string Marker2_RoundOff = Convert.ToString(Math.Round(marker2ValueDBL, 3));

            string marker3Value = ScpiQuery<String>(":CALC1:MARK3 1;:CALC1:MARK3:Y?");
            string[] strArr3 = marker3Value.Split(splitchar);
            double marker3ValueDBL = Convert.ToDouble(strArr3[0]);
            string Marker3_RoundOff = Convert.ToString(Math.Round(marker3ValueDBL, 3));

            int DutNumber = rand.Next();
            string Verdict = "";
            if(DutNumber > 331912926 && DutNumber < 437076772)
            {
                Verdict = "Fail";
            }
            else
            {
                Verdict = "Pass";
            }

            string[] Result_Array = new string[] { DutNumber.ToString(), Marker1_RoundOff, Marker2_RoundOff, Marker3_RoundOff, Verdict };
            return Result_Array;

        }

        public void ApplyLimits(double Max_Begin_Stimulus, double Max_End_Stimulus, double Max_Begin_Response, double Max_End_Response, double Min_Begin_Stimulus, double Min_End_Stimulus, double Min_Begin_Response, double Min_End_Response)
        {
            string ENA_Channel = "1";
            ScpiCommand("CALCulate{0}:LIMit:DISPlay:STATe {1}", ENA_Channel, 1);
            //CALC:LIM:DATA 1,100e3,1.4e9,-40,-40,1,1.4e9,1.45e9,-40,-5,1,1.45e9,2.55e9,-5,-5,1,2.55e9,2.7e9,-5,-40,1,2.7e9,4.5e9,-40,-40,2,1.55e9,2.45e9,-11,-11
            string Limits_Block = "1," + Max_Begin_Stimulus.ToString() + "," + Max_End_Stimulus.ToString() + "," + Max_Begin_Response.ToString() + "," + Max_End_Response.ToString() + 
                ",2," + Min_Begin_Stimulus.ToString() + "," + Min_End_Stimulus.ToString() + "," + Min_Begin_Response.ToString() + "," + Min_End_Response.ToString();
            ScpiCommand("CALCulate{0}:LIMit:DATA {1}", ENA_Channel, Limits_Block); // lower limit and upper limit together
                                                                                   //string Lower_Limits_Block = "1,2," + Min_Begin_Stimulus.ToString() + "," + Min_End_Stimulus.ToString() + "," + Min_Begin_Response.ToString() + "," + Min_End_Response.ToString() ;
                                                                                   //string Upper_Limits_Block = "1,1," + Max_Begin_Stimulus.ToString() + "," + Max_End_Stimulus.ToString() + "," + Max_Begin_Response.ToString() + "," + Max_End_Response.ToString();
                                                                                   // ScpiCommand("CALCulate{0}:LIMit:DATA {1}", ENA_Channel, Lower_Limits_Block); // lower limit
                                                                                   // ScpiCommand("CALCulate{0}:LIMit:DATA {1}", ENA_Channel, Upper_Limits_Block); // upper limit
            ScpiCommand("CALC:MEAS:LIM:STAT ON");
        }

        public bool GetPassFailResult()
        {
            //CALC:MEAS:LIM:FAIL? //0-->Pass, 1-->Fail
            return ScpiQuery<bool>("CALC:MEAS:LIM:FAIL?");
        }

        public void SetRfPowerOnOff(string State)
        {
            if (State == "On")
            {
                ScpiCommand("OUTP ON");
            }
            else
            {
                ScpiCommand("OUTP OFF");
            }
            string scpiToSend = "SENS:SWE:MODE SINGle;*OPC?";
            ScpiQuery<bool>(scpiToSend);
        }
        
        public void SweepConfig(int NoOfPoints, double StartPower, double StopPower)
        {
            //SOUR:POW:CENT -10 //power center, not needed
            //SOUR:POW1 4 //power Level
            ScpiCommand("SENS:SWE:TYPE POW");
            ScpiCommand("SENS:SWE:POIN {0}", NoOfPoints);
            ScpiCommand("SOUR:POW1:PORT:STAR {0}", StartPower);
            ScpiCommand("SOUR:POW1:PORT:STOP {0}", StopPower);
            ScpiCommand("OUTP ON");
            string scpiToSend = "SENS:SWE:MODE SINGle;*OPC?";
            ScpiQuery<bool>(scpiToSend);
        }

        public void ENAMeasure(double startFrequency, double stopFrequency, double amplitudePeak, bool enableLimitTest, bool beeperWarning, double startSearchFrequency, double stopSearchFrequency)
        {
            if (enableLimitTest)
            {
                ScpiCommand("CALCulate:SELected:LIMit:STATe 1");
            }
            else
            {
                ScpiCommand("CALCulate:SELected:LIMit:STATe 0");
            }

            ScpiCommand("CALCulate:SELected:LIMit:DISPlay:STATe 1");
            if (beeperWarning)
            {
                ScpiCommand("SYSTem:BEEPer:WARNing:STATe 1");
            }
            else
            {
                ScpiCommand("SYSTem:BEEPer:WARNing:STATe 0");
            }
            ScpiCommand("CALCulate:SELected:LIMit:DATA 1,1," + startFrequency + "," + stopFrequency + "," + amplitudePeak + "," + amplitudePeak);
            ScpiCommand("CALC:MARK:FUNC:DOM:STAR " + startSearchFrequency);
            ScpiCommand("CALC:MARK:FUNC:DOM:STOP " + stopSearchFrequency);
            ScpiCommand("CALC:MARK:FUNC:DOM 1");
            ScpiCommand("CALCulate:SELected:FUNCtion:TYPE MAXimum");
            ScpiCommand("CALCulate:SELected:FUNCtion:EXECute");
        }
        public double FrequencyPointOverLimit()
        {
            string failedPoints = CutOffFrequencyQuery();
            string[] cutOffF = failedPoints.Split(',');

            double firstFrequencyPointOverLimit = Convert.ToDouble(cutOffF[0]);
            return firstFrequencyPointOverLimit;
        }
        public string CutOffFrequencyQuery()
        {
            return (ScpiQuery("CALCulate:SELected:LIMit:REPort:DATA?"));
        }
        public double AmplitudePeak()
        {
            string amplitudeValues = AmplitudePeakQuery();
            string[] amplitude = amplitudeValues.Split(',');
            double amplitudePeak = Convert.ToDouble(amplitude[0]);
            return amplitudePeak;
        }
        public string AmplitudePeakQuery()
        {

            return (ScpiQuery("CALCulate:SELected:FUNCtion:DATA?"));

        }
        public string Points()
        {
            return (ScpiQuery("SENSe:SWEep:POINts?"));
        }
        public string FrequencyResultsQuery()
        {
            ScpiCommand("FORMat:DATA ASCii");
            return ScpiQuery("SENSe:FREQuency:DATA?");
        }
        public double[] FrequencyResults()
        {
            int i = 0;
            int points = Convert.ToInt32(Points());
            string freq = FrequencyResultsQuery();
            string[] freqData = freq.Split(',');

            double[] frequencyValues = new double[points];

            foreach (var frequency in freqData)
            {
                frequencyValues[i++] = Convert.ToDouble(frequency);
            }

            return frequencyValues;
        }
        public List<string> SParameter()
        {
            int i = 0;
            // int points = Convert.ToInt32(Points());
            string amplitude = SParameterQuery();
            string[] SParameter = amplitude.Split(',');
            //double[] yValues = new double[points];
            List<string> sParameterList = new List<string>();

            for (i = 0; (i < (SParameter.Length) - 1); i = (i + 2))
            {
                string resultsSParameter = SParameter.ElementAt(i);
                sParameterList.Add(resultsSParameter);
            }
            return sParameterList;
        }
        public string SParameterQuery()
        {
            ScpiCommand("FORMat:DATA ASCii");
            //CALCulate:MEASure:DATA:FDATa?
            return ScpiQuery(("CALCulate:MEASure:DATA:FDATa?"));
            //return ScpiQuery(("CALCulate:SELected:DATA:FDATa?"));
        }

        //Some More command
        //-> MMEMory:CDIRectory?
        //<- "C:\Users\Public\Documents\Network Analyzer\"


        public void ScreenCapture(string folderLocation)
        {
            string fileName = folderLocation + DateTime.Now.ToString("yyyyMMddHHmmss") + "_ENA_ScreenCapture" + ".jpg";
            ScpiCommand(":HCOPy:SDUMp:DATA:FORMat JPG");
            //:HCOPy:SDUMp:DATA:FORMat JPG
            byte[] getData = ScpiQueryBlock<byte>("HCOPy:SDUMp:DATA?; *OPC?");
            Thread.Sleep(700);
            MemoryStream mapData = new MemoryStream(getData);
            System.Drawing.Image myImage = System.Drawing.Image.FromStream(mapData);
            myImage.Save(fileName);
        }

    }
}
