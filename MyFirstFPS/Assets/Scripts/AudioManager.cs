using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;

    public AudioClip Walking;
    public AudioClip Running;
    public AudioClip Jumping;
    public AudioClip Landing;
}
