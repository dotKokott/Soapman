using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using Rug.Osc;

public class OscInterface : MonoBehaviour {

    public int Port = 12345;

    private OscReceiver receiver;
    private Thread listenThread;

    public int CAM_WIDTH = 640;
    public int CAM_HEIGHT = 480;

    public int HandCount = 0;
    public Vector3 LeftHandPosition = Vector3.zero;
    public Vector3 RightHandPosition = Vector3.zero;

    public Vector3 Object1 = Vector3.zero;
    public Vector3 Object2 = Vector3.zero;
    public Vector3 Object3 = Vector3.zero;

    void Start() {
        receiver = new OscReceiver( Port );
        listenThread = new Thread( new ThreadStart( listenLoop ) );

        receiver.Connect();
        listenThread.Start();

    }

    void listenLoop() {
        try {
            while ( receiver.State != OscSocketState.Closed ) {
                if ( receiver.State == OscSocketState.Connected ) {
                    OscPacket packet = receiver.Receive();
                    var packetString = packet.ToString();

                    var msg = OscMessage.Parse( packetString );
                    if(msg.Address == "/info") {
                        CAM_WIDTH = (int)msg[0];
                        CAM_HEIGHT = (int)msg[1];
                    }

                    if(msg.Address == "/!glove") {
                        
                    }

                    if ( msg.Address == "/glove" ) {
                        LeftHandPosition.Set( CAM_WIDTH - (float)msg[0], (float)msg[1], 0 );
                    }

                    if (msg.Address == "/object1") {
                        Object1.Set( CAM_WIDTH - (float)msg[0], (float)msg[1], 0 );
                    }

                    if ( msg.Address == "/object2" ) {
                        Object2.Set( CAM_WIDTH - (float)msg[0], (float)msg[1], 0 );
                    }

                    if ( msg.Address == "/object3" ) {
                        Object3.Set( CAM_WIDTH - (float)msg[0], (float)msg[1], 0 );
                    }

                    if (msg.Address == "/!object1") {
                        Object1 = Vector3.zero;
                    }

                    if(msg.Address == "/!object2") {
                        Object2 = Vector3.zero;
                    }

                    if ( msg.Address == "/!object3" ) {
                        Object3 = Vector3.zero;
                    }
                    //if ( packetString.StartsWith( "#bundle" ) ) {
                    //    var bundle = OscBundle.Parse( packetString );
                    //    HandCount = bundle.Count;

                    //    var leftMessage = OscMessage.Parse( bundle[0].ToString() );
                    //    var leftX = (float)leftMessage[0];
                    //    var leftY = (float)leftMessage[1];

                    //    LeftHandPosition.Set( leftX, leftY, 0);

                    //    if ( bundle.Count > 1 ) {
                    //        var rightMessage = OscMessage.Parse( bundle[1].ToString() );
                    //        var rightX = (float)rightMessage[0];
                    //        var rightY = (float)rightMessage[1];

                    //        RightHandPosition.Set( rightX, rightY, 0 );

                    //        if(RightHandPosition.x < LeftHandPosition.x) {
                    //            var right = RightHandPosition;
                    //            RightHandPosition = LeftHandPosition;
                    //            LeftHandPosition = right;
                    //        }
                    //    }
                    //} else {
                    //    var msg = OscMessage.Parse( packetString );
                    //    CAM_WIDTH = (int)msg[0];
                    //    CAM_HEIGHT = (int)msg[1];
                    //}
                }
            }
        } catch ( Exception ex ) {
            if ( receiver.State == OscSocketState.Connected ) {
                Debug.LogError( ex.Message );
            }
        }
    }

    void Update() {

    }

    void OnApplicationQuit() {
        if(receiver != null ) {
            receiver.Close();
            listenThread.Join();
        }
    }
}
