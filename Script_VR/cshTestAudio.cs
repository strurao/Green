using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshTestAudio : MonoBehaviour
{
    AudioSource ass;
    // Start is called before the first frame update
    void Start()
    {

        ass = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            ass.Play();
    }
}
