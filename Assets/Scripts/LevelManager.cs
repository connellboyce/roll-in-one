using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI strokeUI;
    [Space(10)]
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private TextMeshProUGUI levelCompletedStrokeUI;
    [SerializeField] private TextMeshProUGUI splashText;
    [Space(10)]
    [SerializeField] private GameObject gameOverUI;

    [Header("Attributes")]
    [SerializeField] private int par;
    [HideInInspector] public int maxStrokes;
    private int strokes;
    [HideInInspector] public bool outOfStrokes;
    [HideInInspector] public bool levelCompleted;
    [HideInInspector] public bool isLastHole;

    private void Awake()
    {
        main = this;
        maxStrokes = par * 2;
        strokes = 0;
        outOfStrokes = false;
        levelCompleted = false;
    }

    private void Start()
    {
        UpdateStrokeUI();
    }

    public void Stroke()
    {
        strokes++;
        UpdateStrokeUI();
        if (strokes >= maxStrokes)
        {
            outOfStrokes = true;
        }
    }

    private void UpdateStrokeUI()
    {
        strokeUI.text = $"{strokes}";
    }

    public void LevelComplete()
    {
        levelCompleted = true;

        splashText.text = GetRandomRolyPolyFact();
        if (outOfStrokes)
        {
            levelCompletedStrokeUI.text = $"Ran out of strokes! Finished with {strokes}.";
        } else if (strokes == 1)
        {
            levelCompletedStrokeUI.text = $"Roll in One!";
        } else if (strokes == par)
        {
            levelCompletedStrokeUI.text = $"Par! Finished with {strokes}.";
        } else if (strokes == par + 1)
        {
            levelCompletedStrokeUI.text = $"Bogey! Finished with {strokes}.";
        } else if (strokes == par + 2)
        {
            levelCompletedStrokeUI.text = $"Double bogey! Finished with {strokes}.";
        } else if (strokes == par - 1)
        {
            levelCompletedStrokeUI.text = $"Birdie! Finished with {strokes}.";
        } else if (strokes == par - 2)
        {
            levelCompletedStrokeUI.text = $"Eagle! Finished with {strokes}.";
        } else if (strokes == par - 3)
        {
            levelCompletedStrokeUI.text = $"Albatross! Finished with {strokes}.";
        } else if (strokes < par - 3)
        {
            levelCompletedStrokeUI.text = $"Impossible! Finished with {strokes}.";
        }

        if (isLastHole)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            levelCompleteUI.SetActive(true);
        }
    }

    private static readonly string[] facts = new string[]
    {
        "Pill bugs aren't actually insects—they're crustaceans, more closely related to shrimp and crabs.",
        "Roly-polies are one of the few land-dwelling crustaceans and breathe through gill-like structures.",
        "Pill bugs require moist environments because their gills must stay wet to function properly.",
        "Roly-polies can roll into a ball as a defense mechanism, a behavior called conglobation.",
        "Only true pill bugs in the genus Armadillidium can roll into a complete ball—other similar species cannot.",
        "Roly-polies help decompose organic matter, making them beneficial for soil health and composting.",
        "Pill bugs can absorb heavy metals like copper and zinc from soil, helping detoxify polluted environments.",
        "Roly-polies do not urinate; instead, pill bugs release ammonia gas directly through their exoskeleton.",
        "Pill bugs are mostly nocturnal and avoid sunlight to prevent dehydration.",
        "Roly-polies have seven pairs of legs and a segmented, armor-like exoskeleton.",
        "Female pill bugs carry their eggs in a fluid-filled pouch called a marsupium located on their underside.",
        "Newly hatched roly-polies, called mancae, resemble miniature adults and molt several times as they grow.",
        "Pill bugs can live for up to 2–5 years, which is relatively long for such small terrestrial arthropods.",
        "In dry environments, roly-polies often cluster together to help conserve moisture.",
        "Pill bugs do not bite or sting and are completely harmless to humans and pets.",
        "Roly-polies play a vital role in nutrient cycling by breaking down decaying plant material in the soil.",
        "Some scientists use pill bugs as indicators of soil health and pollution levels.",
        "The scientific name of roly-polies, Armadillidium, refers to their armadillo-like ability to roll into a ball.",
        "Roly-polies are the only crustaceans that have fully adapted to life on land.",
        "Roly-polies curl into a ball to protect their soft underbellies from predators.",
        "Pill bugs can survive mild freezing temperatures by hiding under leaf litter or soil.",
        "Roly-polies have simple eyes made up of many light-sensitive cells called ocelli.",
        "Pill bugs rely on chemical cues to recognize their environment and find food.",
        "Roly-polies are detritivores and feed primarily on decaying plant matter.",
        "Pill bugs can sometimes be seen drinking moisture from their own feces to conserve water.",
        "Roly-polies communicate through touch and movement, not sound or vision.",
        "Pill bugs occasionally eat live plants when other food sources are scarce.",
        "Roly-polies go through five to seven molts before reaching full adulthood.",
        "Pill bugs can regenerate lost antennae and legs over several molting cycles.",
        "Roly-polies have a waxy coating on their exoskeleton to slow moisture loss.",
        "Pill bugs may enter a dormant state during extreme heat or cold.",
        "Roly-polies can detect light and dark but cannot form detailed images.",
        "Pill bugs contribute to natural composting by shredding plant debris into smaller pieces.",
        "Roly-polies are often mistaken for insects due to their size and segmented bodies.",
        "Pill bugs have blue-colored blood due to the presence of copper-based hemocyanin.",
        "Roly-polies can be used in classrooms as low-maintenance model organisms for science experiments."
    };

    public static string GetRandomRolyPolyFact()
    {
        int index = Random.Range(0, facts.Length);
        return facts[index];
    }
}
