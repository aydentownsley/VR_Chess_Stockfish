using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;

public class runStockfish : MonoBehaviour
{
    public GameObject startButton;

    public void getMove(string fen)
    {
        string result = "";
        Process stockfish = new Process();

        stockfish.StartInfo.FileName = "C:\\Users\\ayden\\Documents\\Unity\\Stockfish_Test\\Assets\\Stockfish\\stockfish.exe";
        stockfish.StartInfo.RedirectStandardOutput = true;
        stockfish.StartInfo.RedirectStandardInput = true;
        stockfish.StartInfo.CreateNoWindow = true;
        stockfish.StartInfo.UseShellExecute = false;
        stockfish.Start();

        StreamWriter sw = stockfish.StandardInput;

        sw.WriteLine("go movetime 1000");
        while (stockfish.StandardOutput.EndOfStream == false)
        {
            result = stockfish.StandardOutput.ReadLine();
            if (result.Contains("bestmove"))
            {
                break;
            }
        }
        stockfish.Close();
        UnityEngine.Debug.Log(result);
    }
}
