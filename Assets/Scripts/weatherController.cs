using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weatherController : MonoBehaviour
{
    public ParticleSystem rain;
    public ParticleSystem lightning;
    public Light light;
    public Transform playerTransform;
    public AudioSource lightningSound;

    private float _rainLengthMax;
    private float _rainLengthMin;

    private void Start()
    {
        startRain();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            startRain();
        if (Input.GetKeyDown(KeyCode.O))
            startHeavyRain();
        if (Input.GetKeyDown(KeyCode.P))
            playLightning();
        if (Input.GetKeyDown(KeyCode.L))
            stopRain();

    }

    public void startRain()
    {
        rain.enableEmission = true;
    }
    public void startHeavyRain()
    {
        rain.emissionRate = 7000;
    }

    public void stopRain()
    {
        rain.enableEmission = false;
    }

    public void playLightning()
    {
        lightning.Play();
        Invoke("showLight", 1.05f);
    }

    private void showLight()
    {
        lightningSound.GetComponent<AudioSource>();
        lightningSound.Play();
        light.intensity = 25;
        Invoke("hideLight", 0.2f);
    }

    private void hideLight()
    {
        light.intensity = 0;
    }
}
