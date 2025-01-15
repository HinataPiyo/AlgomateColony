using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] SoundSO soundSO;

    [Header("サウンドソース")]
    [SerializeField] AudioSource bgm_AudioSource;
    [SerializeField] AudioSource se_AudioSource;

    [Header("オーディオミキサー")]
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider masterSlider;   // 全ての音量
    [SerializeField] Slider bgmSlider;      // BGM用スライダー
    [SerializeField] Slider seSlider;       // SE用スライダー
    

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }
    void Start()
    {
        bgm_AudioSource.PlayOneShot(soundSO.bgms[0]);

        float master_vol;
        float bgm_vol;
        float se_vol;

        // 初期値をAudioMixerから取得

        audioMixer.GetFloat("METER_Volume", out master_vol);
        audioMixer.GetFloat("BGM_Volume", out bgm_vol);
        audioMixer.GetFloat("SE_Volume", out se_vol);
        
        masterSlider.value = master_vol;
        bgmSlider.value = bgm_vol;
        seSlider.value = se_vol;

        // スライダーイベントのリスナー設定
        masterSlider.onValueChanged.AddListener(SetMETERVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
    }

    // Masterの音量を設定
    public void SetMETERVolume(float volume)
    {
        audioMixer.SetFloat("METER_Volume", volume);
    }

    // BGMの音量を設定
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM_Volume", volume);
    }

    // SEの音量を設定
    public void SetSEVolume(float volume)
    {
        audioMixer.SetFloat("SE_Volume", volume);
    }

    public void PlayAudio(string _name)
    {
        switch (_name)
        {
            case "Back":
                se_AudioSource.PlayOneShot(soundSO.se_system[0]);
                break;
            case "Back_2":
                se_AudioSource.PlayOneShot(soundSO.se_system[1]);
                break;
            case "ButtonClick":
                se_AudioSource.PlayOneShot(soundSO.se_system[2]);
                break;
            case "LevelUp":
                se_AudioSource.PlayOneShot(soundSO.se_system[3]);
                break;
        }
    }

   

}
