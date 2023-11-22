using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private GameObject winMessage;
    [SerializeField] private GameObject looseMessage;
    [SerializeField] private float delayTimeForMainMenu = 5.0f;
    [SerializeField] private AudioSource endOfMatchWhistle;
    
    
    private AudioListener _audioListenerMainCamera;
    private AudioListener _audioListenerFirstPersonCamera;
    private bool _mainCameraOn = true;
    private const KeyCode ChangeCameraKey = KeyCode.Space;
    private const KeyCode GoToMainMenuKey = KeyCode.Escape;
    private int _currentSceneIndex;
    
    private static bool _wonLevel1;
    private static bool _wonLevel2;

    void Start()
    {
        DisableMessages();
        
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        mainCamera = Camera.main;
        if (mainCamera is not null)
        {
            _audioListenerMainCamera = mainCamera.GetComponent<AudioListener>();
        }

        Camera[] allCameras = Camera.allCameras;
        if (allCameras.Length >= 2)
        {
            foreach (Camera someCamera in allCameras)
            {
                if (!someCamera.name.Equals("First Person Camera")) continue;
                firstPersonCamera = someCamera;
            }
        }

        if (firstPersonCamera is not null)
        {
            _audioListenerFirstPersonCamera = firstPersonCamera.GetComponent<AudioListener>();
        }
        ToggleMainCamera();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCamera();
        GoToMainMenu();
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        if (Input.GetKey(GoToMainMenuKey))
        {
            if (_currentSceneIndex != 0)
            {
                SceneManager.LoadScene(0); 
                ToggleMainCamera();
            }
            else
            {
                QuitGame();
            }
            
        }
    }

    private void DisableMessages()
    {
        if (winMessage is not null && looseMessage is not null)
        {
            winMessage.SetActive(false);
            looseMessage.SetActive(false);
        }
    }

    public void GoalWasScored(bool didPlayerScore)
    {
        if (winMessage is not null && looseMessage is not null)
        {
            if (didPlayerScore)
            {
                switch (_currentSceneIndex)
                {
                    case 1:
                        _wonLevel1 = true;
                        break;
                    case 2:
                        _wonLevel2 = true;
                        break;
                }

                winMessage.SetActive(true);
                Invoke(nameof(LoadMainMenu), delayTimeForMainMenu); // Sender os tilbage til Main Menu efter 5 sekunder
            }
            else
            {
                looseMessage.SetActive(true);
                Invoke(nameof(LoadMainMenu), delayTimeForMainMenu); // Sender os tilbage til Main Menu efter 5 sekunder
            }
            endOfMatchWhistle.PlayDelayed(delayTimeForMainMenu * 0.5f);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); 
        ToggleMainCamera();
    }

    public void PlayLevel1(){
        SceneManager.LoadScene(1);
        ToggleMainCamera();
    }
    
    public void PlayLevel2(){
        if (_wonLevel1)
        {
            SceneManager.LoadScene(2);
            ToggleMainCamera();
        }
        
    }

    public void PlayLevel3()
    {
        if (_wonLevel2)
        {
            SceneManager.LoadScene(3);
            ToggleMainCamera();
        }
        
    }


    private void ChangeCamera()
    {
        // Get the current scene index
        int currentSceneIndex = _currentSceneIndex;

        // Toggle the main camera only if not in the first scene
        if (currentSceneIndex != 0)
        {
            // Check for the space key to toggle the main camera
            if (Input.GetKeyDown(ChangeCameraKey))
            {
                if (_mainCameraOn)
                {
                    ToggleFirstPersonCameraOn();
                    ToggleMainCameraOff();
                }
                else
                {
                    ToggleMainCameraOn();
                    ToggleFirstPersonCameraOff();
                }
            }
        }
    }

    private void ToggleMainCamera()
    {
        ToggleMainCameraOn();
        ToggleFirstPersonCameraOff();
    }

    private void ToggleMainCameraOn()
    {
        mainCamera.gameObject.SetActive(true);
        _mainCameraOn = true;
        if (!_audioListenerMainCamera.Equals(null))
        { 
            _audioListenerMainCamera.enabled = true;
        }
    }

    private void ToggleMainCameraOff()
    {
        mainCamera.gameObject.SetActive(false);
        _mainCameraOn = false;
        if (!_audioListenerMainCamera.Equals(null))
        { 
            _audioListenerMainCamera.enabled = false;
        }
    }

    private void ToggleFirstPersonCameraOn()
    {
        if (!_audioListenerFirstPersonCamera.Equals(null))
        {
            firstPersonCamera.gameObject.SetActive(true); 
            if (!_audioListenerFirstPersonCamera.Equals(null))
            {
                _audioListenerFirstPersonCamera.enabled = true;
            }
        }
    }

    private void ToggleFirstPersonCameraOff()
    {
        if (!_audioListenerFirstPersonCamera.Equals(null))
        {
            firstPersonCamera.gameObject.SetActive(false); 
            if (!_audioListenerFirstPersonCamera.Equals(null))
            {
                _audioListenerFirstPersonCamera.enabled = false;
            }
        }
    }
}
