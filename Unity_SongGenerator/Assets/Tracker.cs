using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour {

    public AudioTracker CurrentSource;
    public bool Switching = false;

    public float LifeSpeed = 0.01f;

    public float DistanceTreshold = 0.1f;

    private OscInterface osc;

    public float MinX = 8;
    public float MaxX = -22;
    public float MinY = -4;
    public float MaxY = 22;

    public float FollowSpeed = 5f;

    public bool FollowOSC = true;

    public bool TwoSoundVersion = false;

	void Start () {
        osc = GetComponent<OscInterface>();
    }

    IEnumerator StandingStillCheck() {
        if ( CurrentSource ) {      
            var oldPos = transform.position;
            yield return new WaitForSeconds( 0.5f );
            var newPos = transform.position;

            if(Vector3.Distance(oldPos, newPos) < DistanceTreshold) {
                if(CurrentSource) {
                    CurrentSource.audioSource.volume = CurrentSource.audioSource2.volume -= LifeSpeed * Time.deltaTime;                    
                } 
            } else {
                CurrentSource.audioSource.volume = CurrentSource.audioSource2.volume += LifeSpeed * Time.deltaTime;                
            }

            CurrentSource.audioSource.volume = CurrentSource.audioSource2.volume = Mathf.Clamp(CurrentSource.audioSource.volume, 0f, 1f );
        }
    }

    bool source1On = false;
    bool source2On = false;
    bool source3On = false;

    void updateVolumes() {
        source1On = osc.Object1 != Vector3.zero;
        source2On = osc.Object2 != Vector3.zero;
        source3On = osc.Object3 != Vector3.zero;

        if ( !CurrentSource ) return;        

        if (source1On) {
            CurrentSource.audioSource.volume += LifeSpeed * 2 * Time.deltaTime;
        } else {
            CurrentSource.audioSource.volume -= LifeSpeed * Time.deltaTime;
        }

        if ( source2On ) {
            CurrentSource.audioSource2.volume += LifeSpeed * 2 * Time.deltaTime;
        } else {
            CurrentSource.audioSource2.volume -= LifeSpeed * Time.deltaTime;
        }

        if ( source3On ) {
            CurrentSource.audioSource3.volume += LifeSpeed * 2 * Time.deltaTime;
        } else {
            CurrentSource.audioSource3.volume -= LifeSpeed * Time.deltaTime;
        }

        CurrentSource.audioSource.volume = Mathf.Clamp( CurrentSource.audioSource.volume, 0f, 1f );
        CurrentSource.audioSource2.volume = Mathf.Clamp( CurrentSource.audioSource2.volume, 0f, 1f );
        CurrentSource.audioSource3.volume = Mathf.Clamp( CurrentSource.audioSource3.volume, 0f, 1f );
    }
	
	void Update () {
        //StartCoroutine( StandingStillCheck() );
        if(TwoSoundVersion) {
            updateVolumes();
        }        

        if(FollowOSC) {
            var x = AudioTracker.Map( 0, osc.CAM_WIDTH, MinX, MaxX, osc.LeftHandPosition.x );
            var y = AudioTracker.Map( 0, osc.CAM_HEIGHT, MinY, MaxY, osc.LeftHandPosition.y );

            var target = new Vector3( x, 0, y );
            var direction = ( target - transform.position ).normalized;

            transform.position += direction * FollowSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other) {
        if ( Switching ) return;

        if ( CurrentSource ) CurrentSource.audioSource.loop = CurrentSource.audioSource2.loop = CurrentSource.audioSource3.loop = false;                

        StartCoroutine( SwitchWhenReady( other.GetComponent<AudioTracker>() ) );        
    }

    IEnumerator SwitchWhenReady( AudioTracker newSource ) {
        Switching = true;


        //while (CurrentSource && ( 1f / CurrentSource.audioSource.clip.length ) * CurrentSource.audioSource.time < 0.99f) {
        if ( TwoSoundVersion ) {
            while ( CurrentSource && (CurrentSource.audioSource.isPlaying || CurrentSource.audioSource2.isPlaying || CurrentSource.audioSource3.isPlaying ) ) {
                yield return null;
            }
        } else {
            while ( CurrentSource && ( 1f / CurrentSource.audioSource.clip.length ) * CurrentSource.audioSource.time < 0.99f ) {
                yield return null;
            }
        }


        if ( CurrentSource ) {
            newSource.audioSource.volume = CurrentSource.audioSource.volume;
            newSource.audioSource2.volume = CurrentSource.audioSource2.volume;
            newSource.audioSource3.volume = CurrentSource.audioSource3.volume;
        }
        

        if(!TwoSoundVersion) {
            if ( osc.Object1 != Vector3.zero ) {
                newSource.audioSource.clip = newSource.GetComponent<AudioTracker>().AlternativeSound1;
            } else {
                newSource.audioSource.clip = newSource.GetComponent<AudioTracker>().OriginalClip;
            }
        }

        
        newSource.audioSource.Play();            
        if ( TwoSoundVersion ) {
            newSource.audioSource2.Play();
            newSource.audioSource3.Play();
        }        
        
        newSource.audioSource.loop = newSource.audioSource2.loop = newSource.audioSource3.loop = true;

        CurrentSource = newSource;

        Switching = false;
    }
}
