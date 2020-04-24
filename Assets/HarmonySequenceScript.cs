using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Text.RegularExpressions;

public class HarmonySequenceScript : MonoBehaviour
{

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
    public GameObject[] ModuleInstrumentText;
    public GameObject[] StageIndicators;
    public Material SIUnlit;
    public KMSelectable[] InsCycBtns;


    private int[][] stages = new[]
    {
        new[] { 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0 }
    };

    private static readonly string[][][][] harmonies = new[]
    {
        new[]
        {
            new[]
            {
                new[] { "mb_d4", "mb_f4", "mb_a4", "mb_d5"},
                new[] { "mb_d4", "mb_f4", "mb_ais4", "mb_d5" },
                new[] { "mb_c4", "mb_f4", "mb_a4", "mb_c5" },
                new[] { "mb_c4", "mb_e4", "mb_g4", "mb_c5" }
            },

            new[]
            {
                new[] { "mb_cis4", "mb_f4", "mb_gis4", "mb_cis5"},
                new[] { "mb_fis4", "mb_ais4", "mb_cis5", "mb_fis5" },
                new[] { "mb_dis4", "mb_fis4", "mb_ais4", "mb_dis5" },
                new[] { "mb_c4", "mb_dis4", "mb_gis4", "mb_c5" }
            },

            new[]
            {
                new[] { "mb_a4", "mb_c5", "mb_e5", "mb_a5"},
                new[] { "mb_g4", "mb_c5", "mb_e5", "mb_g5" },
                new[] { "mb_f4", "mb_a4", "mb_c5", "mb_f5" },
                new[] { "mb_c4", "mb_e4", "mb_g4", "mb_c5" }
            },

            new[]
            {
                new[] { "mb_a4", "mb_cis5", "mb_e5", "mb_a5"},
                new[] { "mb_e4", "mb_gis4", "mb_h4", "mb_e5" },
                new[] { "mb_fis4", "mb_a4", "mb_cis5", "mb_fis5" },
                new[] { "mb_d4", "mb_fis4", "mb_a4", "mb_d5" }
            },

            new[]
            {
                new[] { "mb_d4", "mb_f4", "mb_a4", "mb_d5"},
                new[] { "mb_ais4", "mb_d5", "mb_f5", "mb_ais5" },
                new[] { "mb_f4", "mb_a4", "mb_c5", "mb_f5" },
                new[] { "mb_c4", "mb_e4", "mb_g4", "mb_c5" }
            },

            new[]
            {
                new[] { "mb_c4", "mb_e4", "mb_g4", "mb_c5" },
                new[] { "mb_e4", "mb_g4", "mb_h4", "mb_e5" },
                new[] { "mb_a4", "mb_c5", "mb_e5", "mb_a5" },
                new[] { "mb_f4", "mb_a4", "mb_c5", "mb_f5" }
            }
        },
        new[]
        {
            new[]
            {
                new[] { "p_d4", "p_f4", "p_a4", "p_d5"},
                new[] { "p_d4", "p_f4", "p_ais4", "p_d5" },
                new[] { "p_c4", "p_f4", "p_a4", "p_c5" },
                new[] { "p_c4", "p_e4", "p_g4", "p_c5" }
            },

            new[]
            {
                new[] { "p_cis4", "p_f4", "p_gis4", "p_cis5"},
                new[] { "p_fis4", "p_ais4", "p_cis5", "p_fis5" },
                new[] { "p_dis4", "p_fis4", "p_ais4", "p_dis5" },
                new[] { "p_c4", "p_dis4", "p_gis4", "p_c5" }
            },

            new[]
            {
                new[] { "p_a4", "p_c5", "p_e5", "p_a5"},
                new[] { "p_g4", "p_c5", "p_e5", "p_g5" },
                new[] { "p_f4", "p_a4", "p_c5", "p_f5" },
                new[] { "p_c4", "p_e4", "p_g4", "p_c5" }
            },           
                         
            new[]        
            {            
                new[] { "p_a4", "p_cis5", "p_e5", "p_a5"},
                new[] { "p_e4", "p_gis4", "p_h4", "p_e5" },
                new[] { "p_fis4", "p_a4", "p_cis5", "p_fis5" },
                new[] { "p_d4", "p_fis4", "p_a4", "p_d5" }
            },           
                         
            new[]        
            {            
                new[] { "p_d4", "p_f4", "p_a4", "p_d5"},
                new[] { "p_ais4", "p_d5", "p_f5", "p_ais5" },
                new[] { "p_f4", "p_a4", "p_c5", "p_f5" },
                new[] { "p_c4", "p_e4", "p_g4", "p_c5" }
            },           
                         
            new[]        
            {            
                new[] { "p_c4", "p_e4", "p_g4", "p_c5" },
                new[] { "p_e4", "p_g4", "p_h4", "p_e5" },
                new[] { "p_a4", "p_c5", "p_e5", "p_a5" },
                new[] { "p_f4", "p_a4", "p_c5", "p_f5" }
            }

        },
        new[]
        {
            new[]
            {
                new[] { "xy_d5", "xy_f5", "xy_a5", "xy_d6"},
                new[] { "xy_d5", "xy_f5", "xy_ais5", "xy_d6" },
                new[] { "xy_c5", "xy_f5", "xy_a5", "xy_c6" },
                new[] { "xy_c5", "xy_e5", "xy_g5", "xy_c6" }
            },

            new[]
            {
                new[] { "xy_cis5", "xy_f5", "xy_gis5", "xy_cis6"},
                new[] { "xy_fis5", "xy_ais5", "xy_cis6", "xy_fis6" },
                new[] { "xy_dis5", "xy_fis5", "xy_ais5", "xy_dis6" },
                new[] { "xy_c5", "xy_dis5", "xy_gis5", "xy_c6" }
            },

            new[]
            {
                new[] { "xy_a5", "xy_c6", "xy_e6", "xy_a6"},
                new[] { "xy_g5", "xy_c6", "xy_e6", "xy_g6" },
                new[] { "xy_f5", "xy_a5", "xy_c6", "xy_f6" },
                new[] { "xy_c5", "xy_e5", "xy_g5", "xy_c6" }
            },

            new[]
            {
                new[] { "xy_a5", "xy_cis6", "xy_e6", "xy_a6"},
                new[] { "xy_e5", "xy_gis5", "xy_h5", "xy_e6" },
                new[] { "xy_fis5", "xy_a5", "xy_cis6", "xy_fis6" },
                new[] { "xy_d5", "xy_fis5", "xy_a5", "xy_d6" }
            },

            new[]
            {
                new[] { "xy_d5", "xy_f5", "xy_a5", "xy_d6"},
                new[] { "xy_ais5", "xy_d6", "xy_f6", "xy_ais6" },
                new[] { "xy_f5", "xy_a5", "xy_c6", "xy_f6" },
                new[] { "xy_c5", "xy_e5", "xy_g5", "xy_c6" }
            },

            new[]
            {
                new[] { "xy_c5", "xy_e5", "xy_g5", "xy_c6" },
                new[] { "xy_e5", "xy_g5", "xy_h5", "xy_e6" },
                new[] { "xy_a5", "xy_c6", "xy_e6", "xy_a6" },
                new[] { "xy_f5", "xy_a5", "xy_c6", "xy_f6" }
            }
        },
        new[]
        {
            new[]
            {
                new[] { "ha_d5", "ha_f5", "ha_a5", "ha_d6"},
                new[] { "ha_d5", "ha_f5", "ha_ais5", "ha_d6" },
                new[] { "ha_c5", "ha_f5", "ha_a5", "ha_c6" },
                new[] { "ha_c5", "ha_e5", "ha_g5", "ha_c6" }
            },

            new[]
            {
                new[] { "ha_cis5", "ha_f5", "ha_gis5", "ha_cis6"},
                new[] { "ha_fis5", "ha_ais5", "ha_cis6", "ha_fis6" },
                new[] { "ha_dis5", "ha_fis5", "ha_ais5", "ha_dis6" },
                new[] { "ha_c5", "ha_dis5", "ha_gis5", "ha_c6" }
            },

            new[]
            {
                new[] { "ha_a5", "ha_c6", "ha_e6", "ha_a6"},
                new[] { "ha_g5", "ha_c6", "ha_e6", "ha_g6" },
                new[] { "ha_f5", "ha_a5", "ha_c6", "ha_f6" },
                new[] { "ha_c5", "ha_e5", "ha_g5", "ha_c6" }
            },

            new[]
            {
                new[] { "ha_a5", "ha_cis6", "ha_e6", "ha_a6"},
                new[] { "ha_e5", "ha_gis5", "ha_h5", "ha_e6" },
                new[] { "ha_fis5", "ha_a5", "ha_cis6", "ha_fis6" },
                new[] { "ha_d5", "ha_fis5", "ha_a5", "ha_d6" }
            },

            new[]
            {
                new[] { "ha_d5", "ha_f5", "ha_a5", "ha_d6"},
                new[] { "ha_ais5", "ha_d6", "ha_f6", "ha_ais6" },
                new[] { "ha_f5", "ha_a5", "ha_c6", "ha_f6" },
                new[] { "ha_c5", "ha_e5", "ha_g5", "ha_c6" }
            },

            new[]
            {
                new[] { "ha_c5", "ha_e5", "ha_g5", "ha_c6" },
                new[] { "ha_e5", "ha_g5", "ha_h5", "ha_e6" },
                new[] { "ha_a5", "ha_c6", "ha_e6", "ha_a6" },
                new[] { "ha_f5", "ha_a5", "ha_c6", "ha_f6" }
            }
        }
    };

    private bool seqFlashActive = true;
    private bool listen = false;
    private bool harmonyRunning = false;
    private bool stageCompleteActive = false;
    private bool strikeHandlerActive = false;

    private int moduleHarmony;
    private int moduleInstrument = 0;
    private int Strike = 0;
    private int currentStage = 0;
    private int correctNotes = 0;
    private int currentModuleInstrument = 0;
    private int lastModuleInstrument = 0;

    

    private Coroutine seqFlash;
   

    void Awake()
    {
        moduleId = moduleIdCounter++;

        LstnBtn.OnInteract += delegate ()
        {
            if (moduleSolved)
            {
                if (harmonyRunning)
                    return false;
                StartCoroutine(Harmony());
                return false;
            }

            if (stageCompleteActive)
            {

                StartCoroutine(WaitForListen());
                return false;
            }

            LstnBtnPressed();
            return false;
        };
        LstnBtn.OnInteractEnded += delegate ()
        {
            listen = false;
            Text[0].gameObject.SetActive(false);
        };
        for (int i = 0; i < 4; i++)
        {
            SeqBtns[i].OnInteract += SeqBtnsPress(i);
        }
        for (int i = 0; i < 2; i++)
        {
            InsCycBtns[i].OnInteract += InsCycBtnsPress(i);
        }

    }

    private KMSelectable.OnInteractHandler SeqBtnsPress(int btnPressed)
    {
        return delegate ()
        {
            if (moduleSolved || strikeHandlerActive || stageCompleteActive)
                return false;

            StopCoroutine(seqFlash);
            if (seqFlashActive)
            {
                for (int i = 0; i < 4; i++)
                {
                    SeqLights[i].gameObject.SetActive(false);
                }
            }
            seqFlashActive = false;
            Match(btnPressed);
            return false;
        };
    }

    private KMSelectable.OnInteractHandler InsCycBtnsPress(int btnPressed)
    {
        return delegate ()
        {
            if (btnPressed == 0)
            {
                if (moduleInstrument == 0)
                    moduleInstrument = 3;
                else
                    moduleInstrument--;
            }
            else
            {
                if (moduleInstrument == 3)
                    moduleInstrument = 0;
                else
                    moduleInstrument++;
            }
            return false;
        };
    }

    void Start()
    {
        float scalar = transform.lossyScale.x;
        for (var i = 0; i < SeqLights.Length; i++)
            SeqLights[i].range *= scalar;

        moduleHarmony = Random.Range(0, harmonies[moduleInstrument].Length);
        seqFlash = StartCoroutine(SeqFlash());
        ScrambleStages();
    }

    void LstnBtnPressed()
    {
        listen = true;
        if (Text[3].gameObject.activeSelf)
            Text[3].gameObject.SetActive(false);
        Text[0].gameObject.SetActive(true);
    }


    void Match(int btnPressed)
    {
        Debug.LogFormat(@"[Harmony Sequence #{0}] Stage #{1} - Excepted Button #{2} - You pressed Button #{3}", moduleId, currentStage + 1, Array.IndexOf(stages[currentStage], correctNotes) + 1, btnPressed + 1);
        if (btnPressed == Array.IndexOf(stages[currentStage], correctNotes))
        {
            Audio.PlaySoundAtTransform(harmonies[moduleInstrument][moduleHarmony][currentStage][stages[currentStage][btnPressed]], transform);
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
            StartCoroutine(StrikeHandler());
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

    private IEnumerator StrikeHandler()
    {
        strikeHandlerActive = true;
        Text[3].gameObject.SetActive(true);
        Debug.LogFormat(@"[Harmony Sequence #{0}] Stage #{1} - You pressed the wrong button - Strike", moduleId, currentStage + 1);
        correctNotes = 0;
        DisableLights();
        GetComponent<KMBombModule>().HandleStrike();
        Strike++;
        yield return new WaitForSeconds(1f);
        Text[3].gameObject.SetActive(false);
        seqFlashActive = true;
        seqFlash = StartCoroutine(SeqFlash());
        strikeHandlerActive = false;
    }

    private IEnumerator WaitForListen()
    {
       yield return new WaitUntil(() => !stageCompleteActive);
       LstnBtnPressed();
    }

    private IEnumerator StageComplete()
    {
        stageCompleteActive = true;
        yield return new WaitForSeconds(0.5f);
        DisableLights();
        yield return new WaitForSeconds(0.5f);
        Text[1].gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            SeqLights[i].gameObject.SetActive(true);
            Audio.PlaySoundAtTransform(harmonies[moduleInstrument][moduleHarmony][currentStage][i], transform);
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
            stageCompleteActive = false;
        }
        else
        {
            Text[1].gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            seqFlashActive = true;
            seqFlash = StartCoroutine(SeqFlash());
            stageCompleteActive = false;
        }
    }

    private IEnumerator ModulePass()
    {
        moduleSolved = true;
        Text[2].gameObject.SetActive(true);
        StartCoroutine(Harmony());
        yield return new WaitUntil(() => !harmonyRunning);
        GetComponent<KMBombModule>().HandlePass();
        Debug.LogFormat(@"[Harmony Sequence #{0}] You passed the module - Strikes caused by this module: {1}", moduleId, Strike);
        StopAllCoroutines();
    }

    private IEnumerator Harmony()
    {
        harmonyRunning = true;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                SeqLights[j].gameObject.SetActive(true);
                Audio.PlaySoundAtTransform(harmonies[moduleInstrument][moduleHarmony][i][j], transform);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
            DisableLights();
            yield return new WaitForSeconds(0.5f);
        }
        harmonyRunning = false;
    }

    private IEnumerator SeqFlash()
    {
        while (seqFlashActive)
        {

            for (int i = 0; i < 4; i++)
            {
                if (!seqFlashActive)
                {
                    i = 4;
                    break;
                }
                if (listen)
                    Audio.PlaySoundAtTransform(harmonies[moduleInstrument][moduleHarmony][currentStage][stages[currentStage][i]], transform);
                SeqLights[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                SeqLights[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        lastModuleInstrument = currentModuleInstrument;
        currentModuleInstrument = moduleInstrument;
        if (lastModuleInstrument != currentModuleInstrument)
        {
            ModuleInstrumentText[lastModuleInstrument].gameObject.SetActive(false);
            ModuleInstrumentText[currentModuleInstrument].gameObject.SetActive(true);
        }
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} start [start listening to the sequence] | !{0} stop [stop listening] | !{0} sound 1,2,3,4 [presses buttons in that order] | !{0} instrument music/xylo/piano/harp [sets the instrument] | !{0} reset [clears all inputted sounds]";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        do
        {
            yield return "trycancel";
        } while (stageCompleteActive || strikeHandlerActive);
        if (moduleSolved)
        {
            yield return "sendtochaterror The module has entered its Harmony Phase, causing this module to be solve shortly.";
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*start\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (listen)
            {
                yield return "sendtochaterror The module is already listening.";
                yield break;
            }
            yield return null;
            LstnBtn.OnInteract();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*stop\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (!listen)
            {
                yield return "sendtochaterror The module is already not listening.";
                yield break;
            }
            yield return null;
            LstnBtn.OnInteractEnded();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*reset\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            correctNotes = 0;
            DisableLights();
            yield break;
        }
        Match m;
        if ((m = Regex.Match(command, @"^\s*instrument\s+(xylo|piano|music|harp)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)).Success)
        {
            yield return null;
            if (listen)
                LstnBtn.OnInteractEnded();
            var targetInstrument =
                m.Groups[1].Value.EqualsIgnoreCase("music") ? 0 :
                m.Groups[1].Value.EqualsIgnoreCase("piano") ? 1 :
                m.Groups[1].Value.EqualsIgnoreCase("xylo") ? 2 : 3;

            while (moduleInstrument != targetInstrument)
            {
                yield return new WaitForSeconds(0.1f);
                InsCycBtns[0].OnInteract();
            }
            yield break;
        }
        if ((m = Regex.Match(command, @"^\s*sound\s+([\d,;]+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)).Success)
        {
            var numbers = m.Groups[1].Value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(str =>
            {
                int value;
                return int.TryParse(str, out value) ? value : (int?)null;
            }).ToArray();
            if (numbers.Length == 0 || numbers.Any(n => n == null || n.Value < 1 || n.Value > 4))
                yield break;

            yield return null;
            if (listen)
            {
                LstnBtn.OnInteractEnded();
                yield return new WaitForSeconds(.1f);
            }
            yield return numbers.Select(n => SeqBtns[n.Value - 1]).ToArray();
            yield return "solve";
            yield break;
        }
    }
}