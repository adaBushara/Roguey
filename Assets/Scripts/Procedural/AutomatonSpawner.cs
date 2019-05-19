using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatonSpawner : MonoBehaviour
{
    //how big our grid of automatons will be
    public Vector2 gridSize;

    //this will be used to find the camera in our scene
    public Camera mainCamera;

    //our base tile, for automaton spawning
    public GameObject baseSpawnable;

    [SerializeField]
    public List<CellularAutomaton> automatonList;

    [SerializeField]
    public Dictionary<int, bool> refreshBuff;

    bool stepEnabled = false;

    [Range(0.1f,1.0f)]
    public float startingDensity = 0.1f;

    public int birthLimit = 4;
    public int deathLimit = 3;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnAutomata();   
        refreshBuff = new Dictionary<int, bool>();
        foreach (CellularAutomaton a in automatonList)
        {
            a.gameObject.SetActive(Random.value < startingDensity);
            a.refreshBuff = refreshBuff;
            a.birthLimit = birthLimit;
            a.deathLimit = deathLimit;
            //refreshBuff[a.gameObject.GetInstanceID()] = a.gameObject.activeSelf;
            refreshBuff.Add(a.gameObject.GetInstanceID(), a.gameObject.activeSelf);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (stepEnabled)
        //{
        //    Step();
        //}
    }

    public void SpawnAutomata()
    {
        Vector2 _min, _max;
        Vector2 blocksize = baseSpawnable.GetComponent<BoxCollider2D>().size;
        _min.x = -mainCamera.orthographicSize * mainCamera.aspect;
        _max.x = _min.x * -1;

        int numblocks = (int)_max.x * 2;
        numblocks = (int)(numblocks / blocksize.x);

        float currentpos = _min.x;

        _min.y = -mainCamera.orthographicSize;
        _max.y = -_min.y;
        int numblocksvert = (int)(_min.y * -1);
        numblocksvert *= (int)(numblocksvert /blocksize.y);
        Vector2 blockpos = new Vector2(0, 0);
        blockpos.y = _max.y;
        for (int j=0; j < numblocksvert; j++)
        {
            blockpos.x = _min.x;
            blockpos.y -= blocksize.y;
            for (int i=-1; i < numblocks+10; i++)
            {
                //gameObject sobject;
                //want ~25% population

                GameObject sObject = Instantiate<GameObject>(baseSpawnable, mainCamera.transform);
                sObject.transform.localPosition = new Vector3(blockpos.x, blockpos.y, 10);
                CellularAutomaton a = sObject.GetComponent<CellularAutomaton>();
                a.gridX = i + 1;
                a.gridY = j;
                automatonList.Add(a);
                a.gameObject.SetActive(Random.value < 0.05f);
                blockpos.x += blocksize.x;
            }
        }
        foreach (CellularAutomaton a in automatonList)
        {
            a.FindNeighbors();
        }
    }

    public void ToggleStep()
    {
        stepEnabled = !stepEnabled;
    }

    public void Step()
    {
        foreach (CellularAutomaton c in automatonList)
        {
            c.step();
        }

        UpdateSimulation();
    }

    public void UpdateSimulation()
    {
       //update the simulation, prevents cascade effect
       foreach (var a in automatonList)
       {
            a.gameObject.SetActive(refreshBuff[a.gameObject.GetInstanceID()]);
       }
    }
}
