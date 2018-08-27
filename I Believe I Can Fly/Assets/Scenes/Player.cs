using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    [SerializeField] float rcsthrust = 100f;
    [SerializeField] float mainthrust = 100f;

    [SerializeField] AudioClip mainengine;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip victory;

    [SerializeField] ParticleSystem mainengineparticles;
    [SerializeField] ParticleSystem crashparticles;
    [SerializeField] ParticleSystem victoryparticles;

    Rigidbody rigidbody;
    AudioSource JetPackAudio;

    enum State { alive, dying, transcending }
    State state = State.alive;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        JetPackAudio = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update() {
        if (state == State.alive) {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(state != State.alive) return;
        if (collision.gameObject.tag == "Friendly") { //player should not dieS
            print("fine");
        }
        else if (collision.gameObject.tag == "Finish") { //player reaches finish
            victoryparticles.Play();
            state = State.transcending;
            JetPackAudio.Stop();
            JetPackAudio.PlayOneShot(victory);
            Invoke("LoadNextLevel", 1f);
        }
        else { //player crashes
            crashparticles.Play();
            state = State.dying;
            JetPackAudio.Stop(); 
            JetPackAudio.PlayOneShot(crash);
            Invoke("Reset", 1f);
        }
    }

    private void LoadNextLevel(){
        SceneManager.LoadScene(1);
    }

    private void Reset() {
        SceneManager.LoadScene(0);
    }

    private void RespondToRotateInput() {
        rigidbody.freezeRotation = true; //take manual control of rotation

        float rotationspeed = rcsthrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationspeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationspeed);
        }

        rigidbody.freezeRotation = false; //resume physics control of rotation
    }

    private void RespondToThrustInput() {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            JetPackAudio.Stop();
            mainengineparticles.Stop();
        }
    }

    private void ApplyThrust() { //able to thrust and rotate the same time
        rigidbody.AddRelativeForce(Vector3.up * mainthrust * Time.deltaTime);
        if (JetPackAudio.isPlaying == false) {
            JetPackAudio.PlayOneShot(mainengine);
        }
        mainengineparticles.Play();
    }
}
