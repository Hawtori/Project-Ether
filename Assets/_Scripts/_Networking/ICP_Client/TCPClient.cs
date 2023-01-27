// See https://aka.ms/new-console-template for more information

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

//
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TCPClient : MonoBehaviour{

public static string userDefinedIP = "127.0.0.1"; //localHost
public static string userDefinedPort = "7777"; //High port
public static bool gotMsg = false;

private static string msgRecvToPrint; 

public static void StartClient(){
    byte[] buffer = new byte[512];

    //Setup out end point (server)
    try{
        //Local Host   
        IPAddress ip = IPAddress.Parse(userDefinedIP); 

        //DNS translates an IP into readable adress
       // IPAddress ip = Dns.GetHostAddresses("mail.bigpond.com")[0];

        IPEndPoint serverEP = new IPEndPoint(ip, int.Parse(userDefinedPort));
        
        //Setup our client socket
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try{
            //Attempt a connection
            //Console.WriteLine("Connecting to server...");
            Debug.Log("Connecting to server...");
            client.Connect(serverEP);
            //Console.WriteLine("Connected to IP: {0}", client.RemoteEndPoint.ToString());
        


            int recv = client.Receive(buffer);
            msgRecvToPrint = "Received: {0}" + Encoding.ASCII.GetString(buffer, 0, recv);
            gotMsg = true;
          //  wowText.SetText(msgRecvToPrint);

            //Console.WriteLine("Received: {0}", Encoding.ASCII.GetString(buffer, 0, recv));

            //Release socket resources
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        } catch(ArgumentNullException nulExc){
            //Console.WriteLine("Argu,emt exceptopm", nulExc.ToString());

        } catch(SocketException se){
            //Console.WriteLine("Socket Exception", se.ToString());

        }catch (Exception e){
            //Console.WriteLine("Inner excpetion E", e.ToString());

        }


    } catch(Exception e){
           // Console.WriteLine("Outer Exception E", e.ToString());

    }


}
/*
public static int Main(String[] args){

    StartClient();
    //Console.ReadKey();
    return 0;
    
}*/

private void Start() {

    StartClient();

}

private void Update() {
    if(gotMsg){
        transform.GetComponentInChildren<TextMeshProUGUI>().text = msgRecvToPrint;
    }
}


}


