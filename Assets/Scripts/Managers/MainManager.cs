using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public class SaveSetting
    {
        public int widthSize = 1280;
        public int heightSize = 720;
        public bool windowOn = true;
        public int dropdownValue = 3;
        public float bgmValue = 50f;
        public float fxsValue = 50f;
    }

    public class SaveScene
    {
        public string sceneName = "TutorialStage";
    }

    private SaveSetting saveSetting = new SaveSetting();

    private SaveScene saveScene = new SaveScene();

    [Header("�۵��ϴ� ��ư")]
    [SerializeField, Tooltip("���� ���� ��ư")] private Button startButton;
    [SerializeField, Tooltip("���� �ҷ����� ��ư")] private Button loadButton;
    [SerializeField, Tooltip("���� ���� ��ư")] private Button settingButton;
    [SerializeField, Tooltip("���� ���� ��ư")] private Button exitButton;
    [Space]
    [SerializeField, Tooltip("���� �ʱ�ȭ �ٽ� �����")] private GameObject resetChoiceButton;
    [SerializeField, Tooltip("���� ��¥ �ʱ�ȭ ��ư")] private Button resetButton;
    [SerializeField, Tooltip("�������� ���ư��� ��ư")] private Button resetBackButton;
    [Space]
    [SerializeField, Tooltip("���� ���� �� �ٽ� �����")] private GameObject exitChoiceButton;
    [SerializeField, Tooltip("���� ��¥ ���� ��ư")] private Button exitGameButton;
    [SerializeField, Tooltip("�������� ���ư��� ��ư")] private Button exittBackButton;
    [Space]
    [SerializeField, Tooltip("���� ����â")] private GameObject setting;
    [SerializeField, Tooltip("���� ���� ���� ��ư")] private Button settingSave;
    [SerializeField, Tooltip("�������� ���ư��� ��ư")] private Button settingBack;
    [SerializeField, Tooltip("�ػ� ������ ���� ��Ӵٿ�")] private TMP_Dropdown dropdown;
    [SerializeField, Tooltip("â��� ������ ���� ���")] private Toggle toggle;
    [Space]
    [SerializeField, Tooltip("�������")] private Slider bgm;
    [SerializeField, Tooltip("ȿ����")] private Slider fxs;
    [Space]
    [SerializeField, Tooltip("���̵��ξƿ�")] private Image fadeInOut;
    private bool fadeOn = false;
    private float fadeTimer;

    private string saveSettingValue = "saveSettingValue"; //��ũ�� ������ Ű ���� ���� ����

    private string saveSceneName = "saveSceneName"; //���� �����ϱ� ���� ����

    private void Awake()
    {
        if (exitChoiceButton != null)
        {
            exitChoiceButton.SetActive(false);
        }

        if (setting != null)
        {
            setting.gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetString(saveSettingValue) == string.Empty)
        {
            Screen.SetResolution(1280, 720, true);
            dropdown.value = 3;
            toggle.isOn = true;
            bgm.value = 75f / 100f;
            fxs.value = 75 / 100f;

            string getScreenSize = JsonUtility.ToJson(saveSetting);
            PlayerPrefs.SetString(saveSettingValue, getScreenSize);
        }
        else
        {
            string saveScreenData = PlayerPrefs.GetString(saveSettingValue);
            saveSetting = JsonUtility.FromJson<SaveSetting>(saveScreenData);
            setSaveSettingData(saveSetting);
        }

        startButton.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetString(saveSceneName) == string.Empty)
            {
                string setScene = JsonUtility.ToJson(saveScene);
                PlayerPrefs.SetString(saveSceneName, setScene);

                fadeInOut.gameObject.SetActive(true);

                fadeOn = true;
            }
            else
            {
                resetChoiceButton.SetActive(true);
            }
        });

        loadButton.onClick.AddListener(() =>
        {
            string loadSceneData = PlayerPrefs.GetString(saveSceneName);
            saveScene = JsonUtility.FromJson<SaveScene>(loadSceneData);

            if (saveScene != null)
            {
                fadeInOut.gameObject.SetActive(true);

                fadeOn = true;
            }
        });

        settingButton.onClick.AddListener(() =>
        {
            setting.gameObject.SetActive(true);
        });

        exitButton.onClick.AddListener(() =>
        {
            exitChoiceButton.SetActive(true);
        });

        resetButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString(saveSceneName, string.Empty);
            resetChoiceButton.SetActive(false);
        });

        resetBackButton.onClick.AddListener(() =>
        {
            resetChoiceButton.SetActive(false);
        });

        exitGameButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        exittBackButton.onClick.AddListener(() =>
        {
            exitChoiceButton.SetActive(false);
        });

        settingSave.onClick.AddListener(() =>
        {
            dropdownScreenSize();

            Screen.SetResolution(saveSetting.widthSize, saveSetting.heightSize, saveSetting.windowOn);
            saveSetting.dropdownValue = dropdown.value;
            saveSetting.windowOn = toggle.isOn;
            saveSetting.bgmValue = bgm.value;
            saveSetting.fxsValue = fxs.value;

            string getScreenSize = JsonUtility.ToJson(saveSetting);
            PlayerPrefs.SetString(saveSettingValue, getScreenSize);

            string saveScreenData = PlayerPrefs.GetString(saveSettingValue);
            saveSetting = JsonUtility.FromJson<SaveSetting>(saveScreenData);
            setSaveSettingData(saveSetting);
        });

        settingBack.onClick.AddListener(() =>
        {
            setting.gameObject.SetActive(false);
        });
    }

    private void Update()
    {
        if (fadeOn == true)
        {
            fadeTimer += Time.deltaTime / 2;
            Color fadeColor = fadeInOut.color;
            fadeColor.a = fadeTimer;
            fadeInOut.color = fadeColor;

            if (fadeColor.a > 1.0f)
            {
                fadeColor.a = 1.0f;
            }

            if (fadeColor.a >= 1.0f)
            {
                string loadSceneData = PlayerPrefs.GetString(saveSceneName);
                saveScene = JsonUtility.FromJson<SaveScene>(loadSceneData);


                if (PlayerPrefs.GetString(saveSceneName) == string.Empty)
                {
                    SceneManager.LoadSceneAsync("TutorialStage");
                }
                else
                {
                    SceneManager.LoadSceneAsync(saveScene.sceneName);
                }

                fadeOn = false;
            }
        }
    }

    /// <summary>
    /// ��Ӵٿ��� �̿��Ͽ� ��ũ�� ����� �����ϴ� �Լ�
    /// </summary>
    private void dropdownScreenSize()
    {
        if (dropdown.value == 0)
        {
            saveSetting.widthSize = 640;
            saveSetting.heightSize = 360;
        }
        else if (dropdown.value == 1)
        {
            saveSetting.widthSize = 854;
            saveSetting.heightSize = 480;
        }
        else if (dropdown.value == 2)
        {
            saveSetting.widthSize = 960;
            saveSetting.heightSize = 540;
        }
        else if (dropdown.value == 3)
        {
            saveSetting.widthSize = 1280;
            saveSetting.heightSize = 720;
        }
    }

    /// <summary>
    /// ������ ��ũ�� �����͸� ������ �Ҵ�
    /// </summary>
    /// <param name="_saveScreenSize"></param>
    private void setSaveSettingData(SaveSetting _saveScreenSize)
    {
        Screen.SetResolution(_saveScreenSize.widthSize, _saveScreenSize.heightSize, _saveScreenSize.windowOn);
        dropdown.value = _saveScreenSize.dropdownValue;
        toggle.isOn = _saveScreenSize.windowOn;
        bgm.value = _saveScreenSize.bgmValue;
        fxs.value = _saveScreenSize.fxsValue;
    }
}
