using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    void Awake()
    {
        //Guarantee that there is only one instance of the PowerManager
        if (_instance != null && _instance != this)
        {
            Debug.LogError("There was a second instance of the singleton " + this.name + " created, this should never happen!");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //Just in case the object actually exists, but we didn't have it
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject singleton_obj = new GameObject();
                    singleton_obj.name = typeof(T).Name;
                    singleton_obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}
