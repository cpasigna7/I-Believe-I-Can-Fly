using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    [SerializeField] float rcsthrust = 100f;
    [SerializeField] float mainthrust = 100f;
    [SerializeField] float levelloaddelay = 2f;

    [SerializeField] AudioClip mainengine;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip victory;

    [SerializeField] ParticleSystem mainengineparticles;
    [SerializeField] ParticleSystem crashparticles;
    [SerializeField] ParticleSystem victoryparticles;

    Rigidbody rigidbody;
    AudioSource JetPackAudio;

    bool CurrentlyLoading = false;
    bool CollisionsDisabled = false;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        JetPackAudio = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update() {
        if (CurrentlyLoading == false) {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild) {
            RespondtoDebugKeys();
        }
    }

    private void RespondtoDebugKeys() {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadNextLevel();
        }
        if(Input.GetKeyDown(KeyCode.C)) { //toggle collisions
            CollisionsDisabled = !CollisionsDisabled;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(CurrentlyLoading == true || CollisionsDisabled == true) return;
        if (collision.gameObject.tag == "Friendly") { //player should not die
            print("fine");
        }
        else if (collision.gameObject.tag == "Finish") { //player reaches finish
            CurrentlyLoading = true;
            victoryparticles.Play();
            JetPackAudio.Stop();
            JetPackAudio.PlayOneShot(victory);
            Invoke("LoadNextLevel", levelloaddelay);
        }
        else { //player crashes
            CurrentlyLoading = true;
            crashparticles.Play();
            JetPackAudio.Stop(); 
            JetPackAudio.PlayOneShot(crash);
            Invoke("Reset", levelloaddelay);
        }
    }

    private void LoadNextLevel() {
        int totallevels = SceneManager.sceneCountInBuildSettings;
        int currentlevel = SceneManager.GetActiveScene().buildIndex;
        if (currentlevel == totallevels-1) { //loop back to level 1
            Reset();
        }
        else {
            SceneManager.LoadScene(currentlevel + 1);
        }
    }

    private void Reset() {
        SceneManager.LoadScene(0);
    }

    private void RespondToRotateInput() {
        rigidbody.angularVelocity = Vector3.zero; //remove rotation due to physics
        float rotationspeed = rcsthrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationspeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationspeed);
        }
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
