using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseControl : MonoBehaviour
{
    [SerializeField] public GameObject pausemenu;
    [SerializeField] public GameObject healthbar;
    public bool ispaused;
    [SerializeField] Slider slidemus;
    [SerializeField] Slider slidesound;
    [SerializeField] AudioMixer mixer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausemenu.SetActive(false);
        healthbar.SetActive(true);
        float musvol = LoadPrefs("MusVol", 0.2f);
        float soundvol = LoadPrefs("SoundVol", 0.5f);
        mixer.SetFloat("MusVol", Mathf.Log10(musvol) * 20f);
        mixer.SetFloat("SoundVol", Mathf.Log10(soundvol) * 20f);
        slidemus.value = musvol;
        slidesound.value = soundvol;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ispaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pausemenu.SetActive(true);
        healthbar.SetActive(false);
        Time.timeScale = 0f;
        ispaused = true;
    }

    public void ResumeGame()
    {
        pausemenu.SetActive(false);
        healthbar.SetActive(true);
        Time.timeScale = 1f;
        ispaused = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetMusicVolume()
    {
        float volume = slidemus.value;
        SavePrefs("MusVol", volume);
        mixer.SetFloat("MusVol", Mathf.Log10(volume) * 20f);
    }
    public void SetSoundVolume()
    {
        float volume = slidesound.value;
        SavePrefs("SoundVol", volume);
        mixer.SetFloat("SoundVol", Mathf.Log10(volume) * 20f);
    }
    public void SavePrefs(string name, float val)
    {
        PlayerPrefs.SetFloat(name, val);
        PlayerPrefs.Save();
    }
    
    public float LoadPrefs(string name, float val)
    {
        return PlayerPrefs.GetFloat(name, val); 
    }
}
