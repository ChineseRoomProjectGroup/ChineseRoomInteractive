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
    public Animator scene_animator;



    // PUBLIC MODIFIERS


    public static void LoadStage(DemoStage stage)
    {
        Instance.stage = stage;
        switch (stage)
        {
            case DemoStage.Original: Application.LoadLevel(0); break;
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
        Instance.StartCoroutine("DelayBeforeNextAnimation");
    }


    // PRIVATE MODIFIERS

    private void Initialize()
    {

    }

    private IEnumerator DelayBeforeNextAnimation()
    {
        yield return new WaitForSeconds(3);
        Instance.scene_animator.SetTrigger("Next");
    }
}
