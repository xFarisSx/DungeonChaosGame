using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public bool shake = false;
    CinemachineBasicMultiChannelPerlin perlinNoise;
    CinemachineVirtualCamera vcam;
    float duration = 1f;
    float shakeTimer = 0;

    public void Shake(float d)
    {
        duration = d;
        shake = true;
    }
    // Start is called before the first frame update
    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void StartTimer()
    {
        if(shakeTimer >= duration)
        {
            shake = false;
            shakeTimer = 0;
        } else
        {
            shakeTimer+=Time.deltaTime;
            shake = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shake)
        {
            StartTimer();
            perlinNoise.m_FrequencyGain = 1;
            perlinNoise.m_AmplitudeGain = 1;
        } else
        {
            perlinNoise.m_FrequencyGain = 0;
            perlinNoise.m_AmplitudeGain = 0;
        }
    }
}
