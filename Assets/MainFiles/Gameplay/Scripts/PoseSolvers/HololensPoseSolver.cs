using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using AuxiliarContent;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "HololensPS", menuName = "Pose Solver/Hololens", order = 0)]
public class HololensPoseSolver : PoseSolver {
 

    Thread thread;
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    private volatile bool running = true;
    private volatile bool clientAccepted = false;

    public Vector3[] position;
    public static Pose hololensPlayerPose;
	public static Regex _poseRegex = new("Position=\\[\\s(?<x>-?\\d+(?:\\.\\d+)?),\\s(?<y>-?\\d+(?:\\.\\d+)?),\\s\\s(?<z>-?\\d+(?:\\.\\d+)?)\\]");
    void GetData()
    {
        // Create the server
        server = new TcpListener(IPAddress.Any, connectionPort);
        server.Start();

        // Create a client to get the data stream
        
        // Start listening
        while (running)
        {
            if (clientAccepted == false)
            {
                AcceptClient();
            }

            // Check if the client is still connected
            if (clientAccepted && !IsClientConnected(client))
            {
                Debug.LogWarning("Client connection lost. Reconnecting...");
                clientAccepted = false;

            }
            Connection();
            Thread.Sleep(100);
        }
        server.Stop();
        if (client != null)
            client.Close();
    }

    void AcceptClient()
    {
        client = server.AcceptTcpClient();
        clientAccepted = true;
        Debug.Log("Client connected.");
    }

    bool IsClientConnected(TcpClient tcpClient)
    {
        try
        {
            if (tcpClient != null && tcpClient.Client != null && tcpClient.Client.Connected)
            {
                if (tcpClient.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(tcpClient.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    void Connection()
    {
        // Read data from the network stream
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

        // Decode the bytes into a string
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        // Make sure we're not getting an empty string
        //dataReceived.Trim();
        if (dataReceived != null && dataReceived != "")
        {
            // Convert the received string of data to the format we are using
            //position = ParseData(dataReceived);
            //CustomDebug.LogAlex("NEW PARSED POSITION = " + string.Join(", ", position));
            hololensPlayerPose = Pose.GetPoseFromString(dataReceived, _poseRegex);
            nwStream.Write(buffer, 0, bytesRead);
        }
        else
        {
            Debug.Log("No data was received by TCP.");
        }
    }

    // Use-case specific function, need to re-write this to interpret whatever data is being sent
    public static Vector3[] ParseData(string dataString)
    {
        Vector3[] parsedData = Pose.GetPoseVectorFromString(dataString, _poseRegex);
        CustomDebug.LogAlex("ParsedData: " + string.Join(", ", parsedData));
        return parsedData;
    }

    public override void Init()
    {
        ThreadStart ts = new ThreadStart(GetData);
        thread = new Thread(ts);
        thread.Start();
        position = new Vector3[Pose.LandmarkIds.Count];
    }

    public override void Process()
    {
        // Set this object's position in the scene according to the position received
        //transform.position = position;
    }

    public override void Close()
    {
        running = false;
        //Debug.Log("quitting");
        if (clientAccepted)
        {
            Debug.Log("shutting down TCP thread");
            // Set the flag to stop the thread

            // Wait for the thread to finish its execution
            thread.Join();
        }
    }

    public override Pose GetPose()
    {
        return hololensPlayerPose;
    }
}