using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSection : MonoBehaviour
{
    bool _goingto7;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 29 || other.gameObject.layer == 30)
        {
            switch (this.gameObject.name)
            {
                case "Load_01":
                    if (!LevelManager.L_Instance._load01)
                    {
                        LevelManager.L_Instance.LoadScene("Section1");
                        LevelManager.L_Instance._load01 = true;
                        if (LevelManager.L_Instance._load03)
                        {
                            LevelManager.L_Instance.UnloadScene("Section3");
                            LevelManager.L_Instance._load03 = false;
                        }
                    }
                    break;
                case "Load_02":
                    if (!LevelManager.L_Instance._load02)
                    {
                        LevelManager.L_Instance.LoadScene("Section2");
                        LevelManager.L_Instance._load02 = true;
                    }
                    break;
                case "Load_03":
                    if (!LevelManager.L_Instance._load03)
                    {
                        LevelManager.L_Instance.LoadScene("Section3");
                        LevelManager.L_Instance._load03 = true;

                        if (LevelManager.L_Instance._load01)
                        {
                            LevelManager.L_Instance.UnloadScene("Section1");
                            LevelManager.L_Instance._load01 = false;
                        }
                    }
                    break;
                case "Load_04":
                    if (!LevelManager.L_Instance._load04)
                    {
                        LevelManager.L_Instance.LoadScene("Section4");
                        LevelManager.L_Instance._load04 = true;

                        if (LevelManager.L_Instance._load02)
                        {
                            LevelManager.L_Instance.UnloadScene("Section2");
                            LevelManager.L_Instance._load02 = false;
                        }
                        if (LevelManager.L_Instance._load07)
                        {
                            LevelManager.L_Instance.UnloadScene("Section7");
                            LevelManager.L_Instance._load07 = false;
                        }
                    }
                    break;
                case "Load_04B":
                    if (!_goingto7)
                    {
                        if (!LevelManager.L_Instance._load04)
                        {
                            LevelManager.L_Instance.LoadScene("Section4");
                            LevelManager.L_Instance._load04 = true;

                            if (LevelManager.L_Instance._load07)
                            {
                                LevelManager.L_Instance.UnloadScene("Section7");
                                LevelManager.L_Instance._load07 = false;
                            }
                        }
                    }
                    break;
                case "Load_05":
                    if (!LevelManager.L_Instance._load05)
                    {
                        LevelManager.L_Instance.LoadScene("Section5");
                        LevelManager.L_Instance._load05 = true;
                        if (LevelManager.L_Instance._load03)
                        {
                            LevelManager.L_Instance.UnloadScene("Section3");
                            LevelManager.L_Instance._load03 = false;
                        }
                        if (LevelManager.L_Instance._load06)
                        {
                            LevelManager.L_Instance.UnloadScene("Section6");
                            LevelManager.L_Instance._load06 = false;
                        }
                    }
                    break;
                case "Load_06":
                    if (!LevelManager.L_Instance._load06)
                    {
                        LevelManager.L_Instance.LoadScene("Section6");
                        LevelManager.L_Instance._load06 = true;

                        if (LevelManager.L_Instance._load03)
                        {
                            LevelManager.L_Instance.UnloadScene("Section3");
                            LevelManager.L_Instance._load03 = false;
                        }
                        if (LevelManager.L_Instance._load05)
                        {
                            LevelManager.L_Instance.UnloadScene("Section5");
                            LevelManager.L_Instance._load05 = false;
                        }
                        _goingto7 = true;
                    }
                    break;
                case "Load_07":
                    if (!LevelManager.L_Instance._load07)
                    {
                        LevelManager.L_Instance.LoadScene("Section7");
                        LevelManager.L_Instance._load07 = true;
                        if (LevelManager.L_Instance._load04)
                        {
                            LevelManager.L_Instance.UnloadScene("Section4");
                            LevelManager.L_Instance._load04 = false;
                        }
                        _goingto7 = false;
                    }
                    break;
                default:
                    break;
            }
     
        }
    }
}
