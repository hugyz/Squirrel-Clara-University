using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SkyboxPhaseTransition : MonoBehaviour
{
    [Header("Skybox Materials")]
    public Material[] dawnSkyboxes;         
    public Material[] sunnyDaySkyboxes;     
    public Material cloudyDaySkybox;        
    public Material[] sunsetSkyboxes;       
    public Material[] nightSkyboxes;        

    public float holdTime = 30f;
    public float transitionTime = 10f;

    private enum Phase { Dawn, Day, Sunset, Night }
    private Phase[] phaseCycle = { Phase.Dawn, Phase.Day, Phase.Sunset, Phase.Night };
    private int currentPhaseIndex = 0;

    private Dictionary<Phase, Queue<Material>> phaseSkyboxQueue = new Dictionary<Phase, Queue<Material>>();

    void Awake()
    {
        // Shuffle and initialize each skybox queue
        phaseSkyboxQueue[Phase.Dawn] = ShuffleQueue(dawnSkyboxes);
        phaseSkyboxQueue[Phase.Day] = ShuffleQueue(sunnyDaySkyboxes.Concat(new Material[] { cloudyDaySkybox }).ToArray());
        phaseSkyboxQueue[Phase.Sunset] = ShuffleQueue(sunsetSkyboxes);
        phaseSkyboxQueue[Phase.Night] = ShuffleQueue(nightSkyboxes);
    }

    void Start()
    {
        RenderSettings.skybox = GetNextSkybox(phaseCycle[currentPhaseIndex]);
        StartCoroutine(CycleSkyboxes());
    }

    IEnumerator CycleSkyboxes()
    {
        while (true)
        {
            yield return new WaitForSeconds(holdTime);

            int nextIndex = (currentPhaseIndex + 1) % phaseCycle.Length;
            Phase nextPhase = phaseCycle[nextIndex];
            Material nextSkybox = GetNextSkybox(nextPhase);

            yield return StartCoroutine(BlendSkybox(RenderSettings.skybox, nextSkybox, transitionTime));

            RenderSettings.skybox = new Material(nextSkybox); // finalize
            DynamicGI.UpdateEnvironment();

            currentPhaseIndex = nextIndex;
        }
    }

    Material GetNextSkybox(Phase phase)
    {
        if (phaseSkyboxQueue[phase].Count == 0)
        {
            Material[] refill = null;
            switch (phase)
            {
                case Phase.Dawn:
                    refill = dawnSkyboxes;
                    break;
                case Phase.Day:
                    refill = sunnyDaySkyboxes.Concat(new Material[] { cloudyDaySkybox }).ToArray();
                    break;
                case Phase.Sunset:
                    refill = sunsetSkyboxes;
                    break;
                case Phase.Night:
                    refill = nightSkyboxes;
                    break;
            }
            phaseSkyboxQueue[phase] = ShuffleQueue(refill);
        }

        return phaseSkyboxQueue[phase].Dequeue();
    }

    Queue<Material> ShuffleQueue(Material[] materials)
    {
        Material[] shuffled = materials.OrderBy(x => Random.value).ToArray();
        return new Queue<Material>(shuffled);
    }

    IEnumerator BlendSkybox(Material from, Material to, float duration)
    {
        Material blendMat = new Material(from.shader);
        blendMat.CopyPropertiesFromMaterial(from);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            blendMat.Lerp(from, to, t);
            RenderSettings.skybox = blendMat;
            DynamicGI.UpdateEnvironment();
            yield return null;
        }
    }
}
