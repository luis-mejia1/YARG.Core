﻿using ReplayAnalyzer;
using YARG.Core;
using YARG.Core.Chart;
using YARG.Core.Replays.IO;

YargTrace.AddListener(new YargDebugTraceListener());

void ClearAndPrintHeader()
{
    const string HEADER = "Welcome to the YARG.Core Replay Analyzer";

    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.SetCursorPosition((Console.WindowWidth - HEADER.Length) / 2, Console.CursorTop + 1);
    Console.WriteLine(HEADER);

    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
    Console.ForegroundColor = ConsoleColor.White;
}

while (true)
{
    ClearAndPrintHeader();

    // Show the options
    Console.WriteLine("Choose on of the options below:");
    Console.WriteLine("1. Verify replay");
    // Console.WriteLine("2. Look for engine discrepancies");

    // Prompt the user to select an option
    Console.Write("Enter option: ");
    string input = Console.ReadLine();
    int selectedOption;
    if (!int.TryParse(input, out selectedOption))
    {
        continue;
    }
    if (selectedOption is < 0 or > 1)
    {
        continue;
    }

    // Prompt for a replay path (regardless of option)
    ClearAndPrintHeader();
    Console.Write("Enter a valid replay path: ");
    string replayPath = Console.ReadLine();
    if (!File.Exists(replayPath))
    {
        Console.WriteLine("ERROR: Replay does not exist. Press any key to continue.");
        Console.ReadKey(true);
        continue;
    }

    // Prompt for a song folder (regardless of option)
    Console.Write("Enter a valid song folder: ");
    string songFolder = Console.ReadLine();
    if (!Directory.Exists(songFolder))
    {
        Console.WriteLine("ERROR: Song folder does not exist. Press any key to continue.");
        Console.ReadKey(true);
        continue;
    }

    // Look for song.ini and notes file
    string songIni = Path.Combine(songFolder, "song.ini");
    string notesFile = Path.Combine(songFolder, "notes.mid");
    if (!File.Exists(songIni) || !File.Exists(notesFile)) {
        Console.WriteLine("ERROR: Song folder does not have to proper files inside! Press any key to continue.");
        Console.ReadKey(true);
        continue;
    }

    // Load song
    ClearAndPrintHeader();
    Console.WriteLine("Loading song...");
    SongChart chart;
    try
    {
        chart = SongChart.FromFile(new SongMetadata(), notesFile);
    }
    catch (Exception e)
    {
        Console.WriteLine($"ERROR: {e}. Press any key to continue.");
        Console.ReadKey(true);
        continue;
    }
    Console.WriteLine("Done!");

    // Load replay
    Console.WriteLine("Loading replay...");
    var result = ReplayIO.ReadReplay(replayPath, out var replay);
    if (result != ReplayReadResult.Valid)
    {
        Console.WriteLine($"ERROR: Replay result is {result}! Press any key to continue.");
        Console.ReadKey(true);
        continue;
    }
    Console.WriteLine("Done!\n");

    // Load players
    Console.WriteLine($"Players ({replay.PlayerCount}):");
    foreach (var frame in replay.Frames)
    {
        Console.WriteLine($" - {frame.PlayerName}, {frame.Instrument} ({frame.Difficulty})");
    }
    Console.WriteLine($"Band score: {replay.BandScore} (as per metadata)\n");

    // Analyze replay
    Console.WriteLine("Analyzing replay...");
    var analyzer = new Analyzer(chart, replay);
    analyzer.Run();
    Console.WriteLine("Done!\n");

    // Results
    if (analyzer.BandScore != replay.BandScore)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("VERIFICATION FAILED!");
        Console.WriteLine($"Metadata score: {replay.BandScore}");
        Console.WriteLine($"Real score    : {analyzer.BandScore}");
        Console.WriteLine($"Difference    : {Math.Abs(analyzer.BandScore - replay.BandScore)}\n");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("VERIFICATION SUCCESS!");
        Console.WriteLine($"Metadata score: {replay.BandScore}");
        Console.WriteLine($"Real score    : {analyzer.BandScore}");
        Console.WriteLine($"Difference    : {Math.Abs(analyzer.BandScore - replay.BandScore)}\n");
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
}