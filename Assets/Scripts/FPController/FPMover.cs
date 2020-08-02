using UnityEngine;

public class FPMover : MonoBehaviour
{
    public float speed = 5;
    public float backSpeed = 1;

    public float footStepDelay;
    public AudioSource footStepSource;
    public AudioClip[] stoneSteps;
    public AudioClip[] grassSteps;
    public AudioClip[] dirtSteps;

    Vector2 velocity;

    float nextTime = 0;

    void Update()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                velocity.y = Input.GetAxis("Vertical") * backSpeed * Time.deltaTime;
            }
            else
            {
                velocity.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            }
            velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            transform.Translate(velocity.x, 0, velocity.y);
            if(Time.time > nextTime)
            {
                nextTime = Time.time + footStepDelay;
                footStepSource.PlayOneShot(stoneSteps[Random.Range(0, stoneSteps.Length)]);
            }
        }
    }
}
