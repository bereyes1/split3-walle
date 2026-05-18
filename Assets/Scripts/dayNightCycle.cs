using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Sun Settings")]
    [Tooltip("Your scene's Directional Light acting as the sun.")]
    public Light sun;

    [Tooltip("Full day duration in real-world seconds.")]
    public float dayDurationSeconds = 120f;

    [Header("Time Settings")]
    [Range(0f, 1f)]
    [Tooltip("Starting time of day. 0 = midnight, 0.25 = 6am, 0.5 = noon, 0.75 = 6pm.")]
    public float timeOfDay = 0.25f;

    [Header("Sun Color Gradient")]
    [Tooltip("Color of sunlight throughout the day. Left = midnight, center = noon, right = midnight.")]
    public Gradient sunColor;

    [Header("Ambient Light Gradient")]
    [Tooltip("Ambient light color throughout the day.")]
    public Gradient ambientColor;

    [Header("Fog Settings")]
    public bool controlFog = false;
    public Gradient fogColor;
    [Range(0f, 0.1f)]
    public float dayFogDensity = 0.002f;
    [Range(0f, 0.1f)]
    public float nightFogDensity = 0.02f;

    [Header("Sun Intensity")]
    [Tooltip("Light intensity curve over the day. X axis = time (0-1), Y axis = intensity.")]
    public AnimationCurve sunIntensityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public float maxSunIntensity = 1.5f;

    [Header("Skybox Settings")]
    [Tooltip("Your panoramic skybox material (Skybox/Panoramic shader).")]
    public Material skyboxMaterial;
    [Tooltip("Darkest tint color at midnight.")]
    public Color skyboxNightTint = Color.black;
    [Tooltip("Brightest tint color at noon.")]
    public Color skyboxDayTint = Color.white;

    [Header("Events")]
    [Tooltip("Hour (0-24) at which sunrise begins.")]
    public float sunriseHour = 6f;
    [Tooltip("Hour (0-24) at which sunset begins.")]
    public float sunsetHour = 18f;

    public float CurrentHour => timeOfDay * 24f;
    public bool IsDay => CurrentHour >= sunriseHour && CurrentHour < sunsetHour;

    public System.Action OnSunrise;
    public System.Action OnSunset;

    private bool _wasDayLastFrame;

    void Start()
    {
        if (sunColor == null || sunColor.colorKeys.Length == 0)
            sunColor = CreateDefaultSunGradient();

        if (ambientColor == null || ambientColor.colorKeys.Length == 0)
            ambientColor = CreateDefaultAmbientGradient();

        if (fogColor == null || fogColor.colorKeys.Length == 0)
            fogColor = CreateDefaultFogGradient();

        if (sunIntensityCurve == null || sunIntensityCurve.keys.Length < 2)
            sunIntensityCurve = CreateDefaultIntensityCurve();

        _wasDayLastFrame = IsDay;
        ApplyLighting();
    }

    void Update()
    {
        timeOfDay += Time.deltaTime / dayDurationSeconds;
        if (timeOfDay >= 1f) timeOfDay -= 1f;

        ApplyLighting();
        CheckDayNightTransition();
    }

    void ApplyLighting()
    {
        if (sun != null)
        {
            float sunAngle = (timeOfDay * 360f) - 90f;
            sun.transform.rotation = Quaternion.Euler(sunAngle, -30f, 0f);

            sun.color = sunColor.Evaluate(timeOfDay);

            float normalizedIntensity = sunIntensityCurve.Evaluate(timeOfDay);
            sun.intensity = normalizedIntensity * maxSunIntensity;

            sun.enabled = sun.intensity > 0.01f;
        }

        RenderSettings.ambientLight = ambientColor.Evaluate(timeOfDay);

        if (controlFog)
        {
            RenderSettings.fogColor = fogColor.Evaluate(timeOfDay);
            RenderSettings.fogDensity = Mathf.Lerp(nightFogDensity, dayFogDensity,
                sunIntensityCurve.Evaluate(timeOfDay));
        }

        if (skyboxMaterial != null)
        {
            Color tint = Color.Lerp(skyboxNightTint, skyboxDayTint,
                sunIntensityCurve.Evaluate(timeOfDay));
            skyboxMaterial.SetColor("_Tint", tint);
            DynamicGI.UpdateEnvironment();
        }
    }

    void CheckDayNightTransition()
    {
        bool isDayNow = IsDay;

        if (isDayNow && !_wasDayLastFrame)
        {
            OnSunrise?.Invoke();
            Debug.Log("Sunrise!");
        }
        else if (!isDayNow && _wasDayLastFrame)
        {
            OnSunset?.Invoke();
            Debug.Log("Sunset!");
        }

        _wasDayLastFrame = isDayNow;
    }

    public void SetTimeOfDay(float hour)
    {
        timeOfDay = Mathf.Clamp01(hour / 24f);
    }

    private Gradient CreateDefaultSunGradient()
    {
        var g = new Gradient();
        g.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(new Color(0.1f, 0.1f, 0.2f), 0.0f),
                new GradientColorKey(new Color(1.0f, 0.5f, 0.2f), 0.22f),
                new GradientColorKey(new Color(1.0f, 0.95f, 0.8f), 0.5f),
                new GradientColorKey(new Color(1.0f, 0.5f, 0.2f), 0.78f),
                new GradientColorKey(new Color(0.1f, 0.1f, 0.2f), 1.0f),
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
        return g;
    }

    private Gradient CreateDefaultAmbientGradient()
    {
        var g = new Gradient();
        g.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(new Color(0.05f, 0.05f, 0.15f), 0.0f),
                new GradientColorKey(new Color(0.4f, 0.3f, 0.4f),   0.22f),
                new GradientColorKey(new Color(0.5f, 0.6f, 0.7f),   0.5f),
                new GradientColorKey(new Color(0.4f, 0.3f, 0.4f),   0.78f),
                new GradientColorKey(new Color(0.05f, 0.05f, 0.15f), 1.0f),
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
        return g;
    }

    private Gradient CreateDefaultFogGradient()
    {
        var g = new Gradient();
        g.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(new Color(0.05f, 0.05f, 0.1f), 0.0f),
                new GradientColorKey(new Color(0.8f,  0.6f, 0.5f),  0.22f),
                new GradientColorKey(new Color(0.7f,  0.8f, 0.9f),  0.5f),
                new GradientColorKey(new Color(0.8f,  0.6f, 0.5f),  0.78f),
                new GradientColorKey(new Color(0.05f, 0.05f, 0.1f), 1.0f),
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
        return g;
    }

    private AnimationCurve CreateDefaultIntensityCurve()
    {
        return new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.2f, 0f),
            new Keyframe(0.25f, 0.5f),
            new Keyframe(0.5f, 1f),
            new Keyframe(0.75f, 0.5f),
            new Keyframe(0.8f, 0f),
            new Keyframe(1f, 0f)
        );
    }
}