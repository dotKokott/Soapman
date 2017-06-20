using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTracker : MonoBehaviour {

    private Transform hand;
    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioSource audioSource3;

    public AudioClip OriginalClip;
    public AudioClip AlternativeSound1;
    public AudioClip AlternativeSound2;

    void Start () {
        hand = GameObject.Find( "Hand" ).transform;
        audioSource = GetComponent<AudioSource>();

        OriginalClip = audioSource.clip;

        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.clip = AlternativeSound1;

        audioSource3 = gameObject.AddComponent<AudioSource>();
        audioSource3.clip = AlternativeSound2;

        audioSource.volume = audioSource2.volume = audioSource2.volume = 0;   
	}
	
	
	void Update () {
		
	}

    void OnTriggerEnter( Collider other ) {
        //if ( !audioSource.isPlaying ) {
        //    //audioSource.pitch = Map( 0f, -16f, 0.8f, 1.5f, hand.transform.position.x );
        //    audioSource.Play();
        //    audioSource.loop = true;
        //}
    }    



    public static float Map( float a1, float a2, float b1, float b2, float s ) {
        return b1 + ( s - a1 ) * ( b2 - b1 ) / ( a2 - a1 );
    }
}
