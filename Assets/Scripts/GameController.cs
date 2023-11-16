using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera firstPersonCamera;
    private AudioListener _audioListenerMainCamera;
    private AudioListener _audioListenerFirstPersonCamera;
    private bool _mainCameraOn = true;
    private const KeyCode ChangeCameraKey = KeyCode.Space;
    private const KeyCode GoToMainMenuKey = KeyCode.Escape;
    private int _currentSceneIndex;

    void Start()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        mainCamera = Camera.main;
        if (mainCamera != null)
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

        if (firstPersonCamera != null)
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

    private void GoToMainMenu()
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

    public void PlayLevel1(){
        SceneManager.LoadScene(1);
        ToggleMainCamera();
    }
    
    public void PlayLevel2(){
        SceneManager.LoadScene(2);
        ToggleMainCamera();
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
