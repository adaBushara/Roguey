using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CellularAutomaton : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public List<CellularAutomaton> neighbors;
    

    public void FindNeighbors()
    {
        //in here we will find all of our neighbors
        //this will be run every frame

        //neighbors are a max possible total of 8 cells
        //up, down, left, right, + diagonals (l/r/u/d)
        var automatonList = GameObject.FindObjectOfType<AutomatonSpawner>().automatonList;
        for (int i = 0; i < automatonList.Count; i++)
        {
            //up is gridy + 1
            //down is gridy -1
            //left/right +-

            int x, y;
            x = automatonList[i].gridX;
            y = automatonList[i].gridY;
            if (x == (gridX + 1) && y == gridY)
            {
                neighbors.Add(automatonList[i]);
            }
            if (x == (gridX - 1) && y == gridY)
            {
                neighbors.Add(automatonList[i]);
            }
            if (y == (gridY + 1) && x == gridX)
            {
                neighbors.Add(automatonList[i]);
            }
            if (y == (gridY - 1) && x == gridX)
            {
                neighbors.Add(automatonList[i]);
            }

            if (x == gridX + 1 && y == gridY + 1)
            {
                neighbors.Add(automatonList[i]);
            }
            if (x == gridX - 1 && y == gridY + 1)
            {
                neighbors.Add(automatonList[i]);
            }
            if (x == gridX -1 && y == gridY - 1)
            {
                neighbors.Add(automatonList[i]);
            }
            if (x == gridX - 1 && y == gridY + 1)
            {
                neighbors.Add(automatonList[i]);
            }
        }

    }

    //one step of our automaton lifecycle
    public void step()
    {
        int liveNeighbors = 0;
        foreach (CellularAutomaton c in neighbors)
        {
            if (c.gameObject.activeSelf)
            {
                liveNeighbors++;
            }
        }
        
        //die from underpopulation
        if (liveNeighbors < 2)
        {
            gameObject.SetActive(false);
        }
        //keep alive
        if (liveNeighbors == 2 || liveNeighbors == 3)
        {
            gameObject.SetActive(true);
        }
        if (liveNeighbors > 3)
        {
            gameObject.SetActive(false);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
