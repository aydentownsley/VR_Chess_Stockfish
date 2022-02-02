using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System;
using TMPro;

public class FEN : MonoBehaviour
{
    public GameObject TurnIndicator;
    public List<GameObject> spaces = new List<GameObject>();
    private string turn = " w";
    private string FEN_string = "";
    private string move_string = "";
    public TextMeshProUGUI Dialogue;
    private string path;

    /// <summary>
    /// Initializes the board as 1D array of spaces.
    /// </summary>
    void Start()
    {
        foreach (Transform t in this.transform)
            spaces.Add(t.gameObject);
    }


    /// <summary>
    /// Gets path to engine executable
    /// Gets FEN string of current board state
    /// Creates a new thread for stockfish to run on
    /// Calls for thread start
    /// </summary>
    public void stockfish_process()
    {
        this.path = Application.dataPath;
        getFEN();
        Thread stockfish_thread = new Thread(stockfish);
        StartCoroutine(wait_for_stockfish(stockfish_thread));
    }

    /// <summary>
    /// Starts new thread and stockfish process
    /// waits until thread has completed running
    /// Joins thread to get move from engine
    /// </summary>
    IEnumerator wait_for_stockfish(Thread stockfish_thread)
    {
        Dialogue.text = "Thinking...";
        stockfish_thread.Start();
        while (stockfish_thread.IsAlive)
        {
            yield return null;
        }
        stockfish_thread.Join();
        move();

        // stockfish finished and has moved when this is printed
        Dialogue.text = "move from stockfish: " + this.move_string.Split(' ')[1];

        // FOR white suggestion repeat process but somehow highlight suggested move
        // stockfish_process();
        // dont call move, make a suggestion function
    }

    /// <summary>
    /// Runs stockfish process on its seperate thread
    /// Decides which environment the program is running in
    /// Handles all neccessary calls and closes stockfish process
    /// </summary>
    public void stockfish()
    {
        this.move_string = "";
#if UNITY_EDITOR
        Process stockfish = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = this.path + "/Stockfish/stockfish.exe",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = true
        };
        // This code is just incase stockfish library moves and needs to
        // be found on the headset
        // if (!Directory.Exists(filepath))
        // {
        //     Dialogue.text = "dir not found";
        // }else{
        //     Dialogue.text = "dir found";
        //     string[] dir = Directory.GetFiles(filepath);
        //     foreach (string file in dir)
        //         Dialogue.text += '\n' + file;
        // }
#elif UNITY_ANDROID
        string filepath = "/data/data/com.whozi.stockfish/lib";
        Process stockfish = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = filepath + "/libstockfish.so",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = true
        };
#endif
        stockfish.StartInfo = startInfo;
        stockfish.Start();
        StreamWriter sw = stockfish.StandardInput;

        sw.WriteLine("uci");
        sw.WriteLine("ucinewgame");
        sw.WriteLine("position fen " + this.FEN_string + " b");
        sw.WriteLine("go movetime 3000");
        while (stockfish.StandardOutput.EndOfStream == false)
        {
            this.move_string = stockfish.StandardOutput.ReadLine();
            // Dialogue.text += this.move_string;
            if (this.move_string.Contains("bestmove"))
            {
                UnityEngine.Debug.Log("Stockfish output: " + this.move_string);
                break;
            }
        }
        stockfish.Close();
    }

    /// <summary>
    /// Takes output from stockfish, parses it, and convertes
    /// value to actions for gameobjects.
    /// </summary>
    public void move()
    {
        Dialogue.text += "Move has been called\n";
        GameObject piece = null;
        string[] parse;
        string move = "";
        parse = this.move_string.Split(' ');
        Dialogue.text += "\n" + this.move_string;
        foreach (string s in parse)
            UnityEngine.Debug.Log(s);
        UnityEngine.Debug.Log(parse.Length);
        move = parse[1];
        UnityEngine.Debug.Log("move: " + move);

        // /// check destination for game piece and capture if neccessary
        // if (GameObject.Find(move.Substring(2,2)).transform.childCount == 3)
        //     Destroy(GameObject.Find(move.Substring(2,2)).transform.GetChild(2).gameObject);

        piece = GameObject.Find(move.Substring(0,2)).transform.GetChild(2).gameObject;
        GameObject.Find(move.Substring(0,2)).GetComponent<XRSocketInteractor>().enabled = false;
        UnityEngine.Debug.Log(GameObject.Find(move.Substring(0,2)).GetComponent<XRSocketInteractor>().enabled);
        piece.transform.parent = null;
        piece.transform.position = GameObject.Find(move.Substring(2,2)).transform.position;
        UnityEngine.Debug.Log(GameObject.Find(move.Substring(2,2)));
        StartCoroutine(EnableSockets());

        var turnRenderer = TurnIndicator.GetComponent<Renderer>();
        if (this.turn == " w")
        {
            this.turn = " b";
            turnRenderer.material.color = Color.black;
        }
        else if (this.turn == " b")
        {
            this.turn = " w";
            turnRenderer.material.color = Color.white;
        }
    }

    /// <summary>
    /// Sockets are quite possesive and have to be turned off to
    /// move gameobjects, they need to be renabled after move is made
    /// </summary>
    IEnumerator EnableSockets()
    {
        yield return new WaitForSeconds(3);
        foreach (Transform t in this.transform)
        {
            t.GetComponent<XRSocketInteractor>().enabled = true;
        }
    }

    /// <summary>
    /// loops through the spaces
    /// on the board and creates
    /// a FEN string for Stockfish
    /// </summary>
    public void getFEN()
    {
        this.FEN_string = "";
        string temp = "";
        int count = 0;
        int empty = 0;
        foreach (GameObject space in spaces)
        {
            if (count % 8 == 0 && count != 0)
            {
                if (empty == 0)
                    this.FEN_string += temp + "/";
                else
                {
                    this.FEN_string += temp + empty.ToString() + "/";
                    empty = 0;
                }
                temp = "";
            }
            else if (count == 63)
            {
                if (empty == 0)
                    this.FEN_string += temp;
                else
                {
                    this.FEN_string += temp + empty.ToString();
                    empty = 0;
                }
                temp = "";
            }

            if (space.transform.childCount == 3)
            {
                if (empty == 0)
                    temp += space.transform.GetChild(2).name;
                else
                {
                    temp += empty.ToString() + space.transform.GetChild(2).name;
                    empty = 0;
                }
            }
            else
                empty += 1;

            count += 1;
        }
        UnityEngine.Debug.Log(this.FEN_string);
    }
}
