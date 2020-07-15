using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager L_Instance;

    [SerializeField] public GameObject[] _levelLoaders;
    PlayerManager _player;
    bool _managers;
    public bool _load01 = false;
    public bool _load02 = false;
    public bool _load03 = false;
    // Start is called before the first frame update

    private void Start()
    {
        if (L_Instance == null)
            L_Instance = this;
        else if (L_Instance != this)
            Destroy(L_Instance);
        DontDestroyOnLoad(this.gameObject);
        _player = FindObjectOfType<PlayerManager>();
        if (!_managers)
        {
            SceneManager.LoadSceneAsync("ALLMANAGERS", LoadSceneMode.Additive);
            _managers = true;
        }
    }

    private void Update()
    {
     
    }
    public void LoadScene(string x)
    {
        SceneManager.LoadSceneAsync(x, LoadSceneMode.Additive);
    }
    public void UnloadScene(string x)
    {
        SceneManager.UnloadSceneAsync(x);
    }
}
