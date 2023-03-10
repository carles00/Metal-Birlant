using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BactrackController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourcePresent;
    [SerializeField] private AudioSource audioSourceFuture;
    [SerializeField] [Range(0, 0.5f)] private float presentVolume = 0.3f;
    [SerializeField] [Range(0, 0.5f)] private float futureVolume = 0.2f;

    private float[] TimeStamps = {00.000f, 16.000f, 32.000f, 44.000f, 52.000f, 62.000f, 78.000f, 96.000f, 120.000f, 136.000f, 152.000f, 172.000f};
    private bool ChangeTurn = false;

    [SerializeField] private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        audioSourcePresent.volume = presentVolume;
        audioSourcePresent.Play();
    }

    // Update is called once per frame
    void Update()
    {
       if((gameController.GetCurrentTurn() == 0) && (ChangeTurn == true))
        { 
            PlayPresentTheme();
            ChangeTurn = false;
        }

       else if((gameController.GetCurrentTurn() == 1) && (ChangeTurn == false))
        {
            PlayFutureTheme();
            ChangeTurn = true;
        }

    }

    private void PlayPresentTheme()
    {
        audioSourceFuture.Stop();
        audioSourcePresent.time = ChooseTimeStamp();
        audioSourcePresent.volume = presentVolume;
        audioSourcePresent.Play();
    }

    private void PlayFutureTheme()
    {
        audioSourcePresent.Stop();
        audioSourceFuture.time = ChooseTimeStamp();
        audioSourceFuture.volume = futureVolume;
        audioSourceFuture.Play();
    }

    private float ChooseTimeStamp()
    {
        int index = Random.Range(0, TimeStamps.Length);

        return TimeStamps[index];
    }
}
