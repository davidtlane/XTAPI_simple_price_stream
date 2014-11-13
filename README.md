XT-API-DATA-FEED
================

Simple price data request application in C# using X Trader's API

*****************************************************************************
This application requires you to have X Trader Pro installed on your machine.
Utilizes XTAPI library, found here: tt\x_trader\xtapi\bin\xtapi.interop.dll
*****************************************************************************

In command window: csc /platform:x86 /r:xtapi.interop.dll xtpricefeed.cs
to compile and generate xtpricefeed.exe

Add desired instruments to instrument_list.csv

Open X TRADER (Pro licence)

Run xtpricefeed.exe

Hit any key to stop the price stream
