using UnityEngine;
using System.Collections;

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

    void Start()
    {
        RenderSettings.skybox = GetRandomSkybox(phaseCycle[currentPhaseIndex]);
        StartCoroutine(CycleSkyboxes());
    }

    IEnumerator CycleSkyboxes()
    {
        while (true)
        {
            yield return new WaitForSeconds(holdTime);

            int nextIndex = (currentPhaseIndex + 1) % phaseCycle.Length;
            Phase nextPhase = phaseCycle[nextIndex];
            Material nextSkybox = GetRandomSkybox(nextPhase);

            yield return StartCoroutine(BlendSkybox(RenderSettings.skybox, nextSkybox, transitionTime));

            RenderSettings.skybox = new Material(nextSkybox); // finalize
            DynamicGI.UpdateEnvironment();

            currentPhaseIndex = nextIndex;
        }
    }

    Material GetRandomSkybox(Phase phase)
    {
        switch (phase)
        {
            case Phase.Dawn:
                return dawnSkyboxes[Random.Range(0, dawnSkyboxes.Length)];

            case Phase.Day:
                bool isSunny = Random.value <= 0.75f;
                return isSunny ? sunnyDaySkyboxes[Random.Range(0, sunnyDaySkyboxes.Length)] : cloudyDaySkybox;

            case Phase.Sunset:
                return sunsetSkyboxes[Random.Range(0, sunsetSkyboxes.Length)];

            case Phase.Night:
                return nightSkyboxes[Random.Range(0, nightSkyboxes.Length)];

            default:
                return null;
        }
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
