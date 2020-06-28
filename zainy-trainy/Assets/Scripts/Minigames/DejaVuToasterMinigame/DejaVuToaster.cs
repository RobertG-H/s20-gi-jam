﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DejaVuToaster : MonoBehaviour
{
    
	[SerializeField]
	private DemoModuleManager moduleManager;
    public DejaVuToasterAudioController audioController;
    public Animator toasterAnim;
    public Animator targetAnim;
    public Transform crateL;
    public Transform crateR;
    public Transform center;
    public Transform edge;
    public GameObject toaster;
    public GameObject target;
    public GameObject hitEffect;
    public GameObject trailEffect;
    public GameObject bullet;
    [Range(0f, 1.0f)]
    public float musicVolLow, musicVolHigh;
    public float musicHighJump;
    public float minWait, maxWait;
    public float lowVolumeFractionTime;
    public float dejaVuTime;
    public float endGameTime;
    public float rampDownTime;
    
    private AudioSource musicSource;    
    private bool canHit;
    private bool go;
    private float timeToWait;
    public float timeToCross;
    private float currentTimeCrossing;
    private int toasterPos;
    private bool shot;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable()
    {
        Initialize();
    }


    // Update is called once per frame
    void Update()
    {
        if(go)
        {
            if(currentTimeCrossing <= timeToCross)
            {
                if(toasterPos == 0) //Left crate to right crate
                {
                    toaster.transform.position = Vector3.Lerp(crateL.position, crateR.position, currentTimeCrossing/timeToCross);

                }
                else //Right crate to left crate
                {
                    toaster.transform.position = Vector3.Lerp(crateR.position, crateL.position, currentTimeCrossing/timeToCross);
                }
                currentTimeCrossing += Time.deltaTime;
            }
            else
            {
                if(toasterPos == 0) //Left crate to right crate
                {
                    toaster.transform.position = crateR.position;

                }
                else //Right crate to left crate
                {
                    toaster.transform.position = crateL.position;
                }
                StartCoroutine(EndGame(0));
                go = false;
            }

        }
    }
    private void Initialize()
    {
        bullet.SetActive(true);
        hitEffect.SetActive(false);
        trailEffect.SetActive(false);
        target.transform.position = center.position;
        targetAnim.Play("TargetIdle");
        canHit = false;
        go = false;
        shot = false;
        currentTimeCrossing = 0;
        toasterPos = Random.Range(0, 2);
        if(toasterPos == 0)
        {
            toaster.transform.position = crateL.position;
            
            Vector3 newScale = toaster.transform.localScale;
            newScale.x = 1;
            toaster.transform.localScale = newScale;
        }
        else
        {
            toaster.transform.position = crateR.position;

            Vector3 newScale = toaster.transform.localScale;
            newScale.x = -1;
            toaster.transform.localScale = newScale;
        }
        timeToWait = Random.Range(minWait, maxWait);
        float songStart = dejaVuTime - timeToWait;
        toasterAnim.Play("ToasterAlive");
        musicSource = audioController.PlayDejaVu();
        musicSource.time = songStart;
        musicSource.volume = musicVolLow;
        musicSource.Play();
        StartCoroutine("RampVolumeUp");
        StartCoroutine("ToasterWaitToGo");
    }
    private IEnumerator RampVolumeUp()
    {
        float currentTime = 0;
        while(currentTime < timeToWait)
        {
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
            if(currentTime >= timeToWait * lowVolumeFractionTime)
            {
                musicSource.volume = Mathf.Lerp(musicVolLow, musicVolHigh-musicHighJump, (currentTime - timeToWait * lowVolumeFractionTime)/(timeToWait * (1-lowVolumeFractionTime)));
            }
        }
        musicSource.volume = musicVolHigh;
    }

    private IEnumerator RampVolumeDown()
    {
        float currentTime = 0;
        while(currentTime < timeToWait)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            musicSource.volume = Mathf.Lerp(musicVolHigh, 0, currentTime/rampDownTime);
        }
        Destroy(musicSource.gameObject);
    }
    private IEnumerator ToasterWaitToGo()
    {
        yield return new WaitForSeconds(timeToWait);
        audioController.PlayDrift();
        StartCoroutine("RampVolumeDown");
        trailEffect.SetActive(true);
        go = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        canHit = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        canHit = false;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if(context.started && !shot)
        {
            targetAnim.Play("TargetShooting");
            shot = true;
            bullet.SetActive(false);
            if(canHit && go)
            {
                hitEffect.SetActive(true);
                hitEffect.transform.position = toaster.transform.position;
                target.transform.position = toaster.transform.position;
                float score = 1 - Mathf.Min((toaster.transform.position - center.position).magnitude/(edge.position - center.position).magnitude, 1);
                toasterAnim.Play("ToasterDead");
                StartCoroutine(EndGame(score));
                go = false;
            }
        }
    }
    private IEnumerator EndGame(float score)
	{
        trailEffect.SetActive(false);
        musicSource.volume = musicVolLow;
		yield return new WaitForSeconds(endGameTime);
        Debug.Log(score);
        // Initialize();
		moduleManager.MinigameCompleted(score);
	}

}
