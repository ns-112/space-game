using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor; 

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField]
    private List<GameObject> effects;

    public void SpawnParticleEffect (Vector3 position, GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, GameObject.Find("[Effect] EffectsContainer").transform);
        obj.transform.position = position;
        obj.GetComponent<ParticleSystem>().Play();
        effects.Add(obj);
        
    }

    public void SwitchToGameScene()
    {
        SceneManager.LoadScene("DefaultScene");
    }

    

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        { 
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    void Update()
    {   
        
        for (int i = effects.Count - 1; i >= 0; i--)
        {   
           

            //Check effect type & update accordingly

            //Particle Bursts
            if (effects[i].TryGetComponent(out ParticleSystem sys))
            {
                if (sys.isStopped)
                {
                    Destroy(effects[i]);
                    effects.RemoveAt(i);
                }
            }
            
            
        }
    }
}
