using System;
using System.IO;
using System.Collections.Generic;
using XTAPI;

namespace XT
{
	class LastPrice
	{
		// Define Gate and notify objects
		public static TTGate myGate = new TTGateClass();
		public static TTInstrNotify myNotify = new TTInstrNotifyClass();
		
		public static void SetNotifyHandlers()
		{
			myNotify.OnNotifyFound    += new _ITTInstrNotifyEvents_OnNotifyFoundEventHandler(NotifyFound);
			myNotify.OnNotifyNotFound += new _ITTInstrNotifyEvents_OnNotifyNotFoundEventHandler(NotifyNotFound);
			myNotify.OnNotifyUpdate   += new _ITTInstrNotifyEvents_OnNotifyUpdateEventHandler(NotifyUpdate);
		}

		public static void SetParameters(TTInstrObj pInstr, string exch, string prod, string type, string cont)
		{
			pInstr.Exchange = exch;
			pInstr.Product = prod;
			pInstr.ProdType = type;
			pInstr.Contract = cont;
		}
		
		public static void GetLastPrice(string exch, string prod, string type, string cont)
		{
			TTInstrObjClass myInstr = new TTInstrObjClass();
			SetParameters(myInstr, exch, prod, type, cont);
			myInstr.Open(0);
			myNotify.AttachInstrument(myInstr);
		}

		public static string[] ParseInstrumentList()
		{
			var reader = new StreamReader(File.OpenRead(@"instrument_list.csv"));
			List<string> instrList = new List<string>();
			while (!reader.EndOfStream)
			{
				string   line = reader.ReadLine();
				instrList.Add(line);
			}
			string[] instrArray = instrList.ToArray();
			return instrArray;
		}
		
		public static void Main()
		{
			myGate.OpenExchangePrices("*");
			
			// Get list of instruments from csv file:
			string[] instrList = ParseInstrumentList();
			// Set up event handlers:
			SetNotifyHandlers();
			// Get last price for each instrument:
			foreach (string list in instrList)
			{
				string[] elem = list.Split(',');
				GetLastPrice(elem[0],elem[1],elem[2],elem[3]);
			}
			// Wait for any user input to stop the process:
			string str = Console.ReadLine();
			
			// You can also get exchange rates:
			Console.WriteLine(" EURUSD = " + myGate.GetExchangeRate("EUR", "USD"));
			
			// Shut down XTAPI:
			myGate.XTAPITerminate();

		}
		
		// Notification event handlers:
		
		// Instrument FOUND event handler....
		public static void NotifyFound(TTInstrNotify pNotify, TTInstrObj pInstr)
		{
			Console.WriteLine("Instrument found: " + pInstr.Product + " " + pInstr.Contract );
		}
		// Instrument NOT FOUND event handler....
		public static void NotifyNotFound(TTInstrNotify pNotify, TTInstrObj pInstr)
        {
            Console.WriteLine("The Contract: " + pInstr.Contract + " was not found", "Error");
		}
		// Instrument UPDATED event handler....
		public static void NotifyUpdate(TTInstrNotify pNotify, TTInstrObj pInstr)
		{
			// All instrument properties can be found in the XT API Class Reference (Property: Get)
			// XTAPI_ClassReference_7.17_DG.pdf -- Pages 236 - 254

			Array data = (Array) pInstr.get_Get("Exchange,Product,ProdType,Contract,Last$");
			string txtExchange = (string)data.GetValue(0);
			string txtProduct = (string)data.GetValue(1);
			string txtProductType = (string)data.GetValue(2);
			string txtContract = (string)data.GetValue(3);
			string txtLastPrice = (string)data.GetValue(4);

			string now = DateTime.Now.ToString(" yyyy-MM-dd HH:mm:ss  ");

			// Do **** with LastPrice here...
			// For now, just print to console...
			Console.WriteLine(now + txtContract + "  =  " + txtLastPrice);
		}

	}

}

