using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HarmonySequenceScript : MonoBehaviour {

    public KMAudio Audio;
    public KMBombInfo Bomb;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    public KMSelectable[] SeqBtns;
    public KMSelectable LstnBtn;
    public Light[] SeqLights;
    public AudioClip[] Sounds;
    public GameObject[] Text;
    public GameObject[] StageIndicators;
    public Material SIUnlit;


    private static int[][] stages = new[]
    {
        new[] { 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0 }
    };

    private static readonly string[][][] harmonies = new[]
    {
        new[]
        { 
            new[] { "d4", "f4", "a4", "d5"}, 
            new[] { "d4", "f4", "ais4", "d5" },
            new[] { "c4", "f4", "a4", "c5" },
            new[] { "c4", "e4", "g4", "c5" }
        },
        new[]
        { 
            new[] { "d4", "f4", "a4", "d5"}, 
            new[] { "d4", "f4", "ais4", "d5" },
            new[] { "c4", "f4", "a4", "c5" },
            new[] { "c4", "e4", "g4", "c5" }
        },
        new[]
        { 
            new[] { "d4", "f4", "a4", "d5"}, 
            new[] { "d4", "f4", "ais4", "d5" },
            new[] { "c4", "f4", "a4", "c5" },
            new[] { "c4", "e4", "g4", "c5" }
        },
        new[]
        { 
            new[] { "d4", "f4", "a4", "d5"}, 
            new[] { "d4", "f4", "ais4", "d5" },
            new[] { "c4", "f4", "a4", "c5" },
            new[] { "c4", "e4", "g4", "c5" }
        },
    };

    private bool seqFlashActive = true;
    private bool listen = false;

    private int moduleHarmony;
    private int Strike = 0;
    private int currentStage = 0;
    private int correctNotes = 0;


    void Awake()
    {
        moduleId = moduleIdCounter++;

        LstnBtn.OnInteract += delegate () { listen = true; Text[0].gameObject.SetActive(true); return false; };
        LstnBtn.OnInteractEnded += delegate () { listen = false; Text[0].gameObject.SetActive(false); };
        for (int i = 0; i < 4; i++)
        {
        SeqBtns[i].OnInteract += SeqBtnsPress(i);
        }
    }

    private KMSelectable.OnInteractHandler SeqBtnsPress(int btnPressed)
    {
        return delegate ()
        {   

            seqFlashActive = false;
            Match(btnPressed);
            return false;
        };
    }

    void Start ()
    {
        moduleHarmony = Random.Range(0, harmonies.Length);
        StartCoroutine(SeqFlash());
        ScrambleStages();
	}

    void Match(int btnPressed)
    {
        Debug.LogFormat(@"[Harmony Sequence #{0}] Stage #{1} - Excepted Button #{2} - You pressed Button #{3}", moduleId, currentStage, Array.IndexOf(stages[currentStage], correctNotes), btnPressed);
        if (btnPressed == Array.IndexOf(stages[currentStage], correctNotes))
        {
            Audio.PlaySoundAtTransform(harmonies[moduleHarmony][currentStage][stages[currentStage][btnPressed]], transform);
            SeqLights[btnPressed].gameObject.SetActive(true);
            correctNotes++;
            if (correctNotes == 4)
            {
                correctNotes = 0;
                StartCoroutine(StageComplete());
            }
        }
        else
        {
            Text[3].gameObject.SetActive(true);
            Debug.LogFormat(@"[Harmony Sequence #{0}] Stage #{1} - You pressed the wrong button - Strike", moduleId, currentStage);
            correctNotes = 0;
            DisableLights();
            GetComponent<KMBombModule>().HandleStrike();
            Strike++;
            seqFlashActive = true;
        }
    }

    void DisableLights()
    {
        for (int i = 0; i < 4; i++)
        {
            SeqLights[i].gameObject.SetActive(false);
        }
    }

    void ScrambleStages()
    {
        for (int i = 0; i < 4; i++)
        {
            var sound = Enumerable.Range(0, 4).ToList();

            for (int j = 0; j < 4; j++)
            {
                int index = Random.Range(0, sound.Count);
                stages[i][j] = sound[index];
                sound.RemoveAt(index);
            }
        }
    }

    private IEnumerator StageComplete()
    {
        yield return new WaitForSeconds(0.5f);
        DisableLights();
        yield return new WaitForSeconds(0.5f);
        Text[1].gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            SeqLights[i].gameObject.SetActive(true);
            Audio.PlaySoundAtTransform(harmonies[moduleHarmony][currentStage][i], transform);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        DisableLights();
        StageIndicators[currentStage].GetComponent<Renderer>().material = SIUnlit;
        currentStage++;
        if (currentStage == 4)
        {
            Text[1].gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            StartCoroutine(ModulePass());
        }
        else
        {
            Text[1].gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            seqFlashActive = true;
            StartCoroutine(SeqFlash());
        }
    }

    private IEnumerator ModulePass()
    {
        Text[2].gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                SeqLights[j].gameObject.SetActive(true);
                Audio.PlaySoundAtTransform(harmonies[moduleHarmony][i][j], transform);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
            DisableLights();
            yield return new WaitForSeconds(0.5f);
        }
        GetComponent<KMBombModule>().HandlePass();
        Debug.LogFormat(@"[Harmony Sequence #{0}] You passed the module - Strikes caused by this module: {1}", moduleId, Strike);
        StopAllCoroutines();
    }

    private IEnumerator SeqFlash()
    {
        while (seqFlashActive)
        {
            if (Text[3].gameObject.activeSelf)
                Text[3].gameObject.SetActive(false);

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < 4; i++)
            {
                if (!seqFlashActive)
                {
                    i = 4;
                    break;
                }
                if (listen)
                    Audio.PlaySoundAtTransform(harmonies[moduleHarmony][currentStage][stages[currentStage][i]], transform);
                SeqLights[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                SeqLights[i].gameObject.SetActive(false);
            }
        }
    }
}