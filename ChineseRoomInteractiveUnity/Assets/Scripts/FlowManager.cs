using UnityEngine;
using System.Collections;


public enum DemoStage { Original, Digitized, FileCabinet, ScrapPaper, Robot, Brain };

/// <summary>
/// Singleton for animation and stage handling
/// </summary>
public class FlowManager : MonoBehaviour
{
    private static FlowManager _instance;
    public static FlowManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<FlowManager>();

                if (_instance == null) Debug.LogError("Missing FlowManager");
                else
                {
                    DontDestroyOnLoad(_instance);
                    _instance.Initialize();
                }
            }
            return _instance;
        }
    }


    private DemoStage stage = DemoStage.Original;
    private int stage_state = 0; // state in the current stage
    private bool output_correct = false;

    public Animator scene_animator;
    public Messenger native_messenger; // messenger for the native speaker dialog
    public Messenger room_messenger; // messenger for help and explanation dialog


    // PUBLIC MODIFIERS

    public void Awake()
    {
        // if this is the first instance, make this the singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            // destroy other instances that are not the already existing singleton
            if (this != _instance)
            {
                // but save their inspector set parameters
                _instance.scene_animator = this.scene_animator;
                _instance.native_messenger = this.native_messenger;
                _instance.room_messenger = this.room_messenger;

                // destroy
                Destroy(this.gameObject);
            }
        }

        Initialize();
    }
    public void Start()
    {
        // TESTING 
        LoadStage(DemoStage.Original);
    }

    public static void LoadStage(DemoStage stage)
    {
        Instance.stage = stage;
        Instance.stage_state = 0;

        Instance.StopAllCoroutines();

        switch (stage)
        {
            case DemoStage.Original: Application.LoadLevel("stage_original"); break;
            case DemoStage.Digitized: Application.LoadLevel(0); break;
            case DemoStage.FileCabinet: Application.LoadLevel(0); break;
            case DemoStage.ScrapPaper: Application.LoadLevel(0); break;
            case DemoStage.Robot: Application.LoadLevel(0); break;
            case DemoStage.Brain: Application.LoadLevel(0); break;
        }
    }
    /// <summary>
    /// Move on to the next phase after correct output is given
    /// </summary>
    public static void OnCorrectOutputGiven()
    {
        Instance.output_correct = true;
        //Instance.StartCoroutine("DelayBeforeNextAnimation");
    }


    // PRIVATE MODIFIERS

    private void Initialize()
    {

    }


    // PUBLIC ACCESSORS

    public static bool Continue()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    public static bool OutputCorrect()
    {
        return Instance.output_correct;
    }
}
