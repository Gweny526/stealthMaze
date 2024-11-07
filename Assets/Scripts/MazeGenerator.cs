using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private int [,] cells; //[,] tableau a deux index
    
    [SerializeField] private int size_x = 10; //taille en x de la map, taille en abstait si j'ai des image de 5U(unity unité) j'en aurai 10 case d'image de 5u?

    [SerializeField] private int size_y = 8; // taille en y de la map
    
    private List<Vector2Int> walls = new(); //Vector2 de seulement des chiffre entier (Vector2Int)

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float gridSize = 2;

    void Awake()
    {
        //1 Start with a grid full of walls
        cells = new int[size_x, size_y];
        
        
        
        
            
    }

    // Start is called before the first frame update
    void Start()
    {
        RegenerateMaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RegenerateMaze()
    {
        
        for (int i = 0; i < size_x; i++) //pour toute les collone dans le tableau il va mettre un 0 dans toutes les valeurs dans le tableau en x
        {
            for (int j = 0; j < size_y; j++) // ici en y
            {
                cells [i,j] = 0; // et c'est ici qu'il va mettre un 0
            }
        }

        for(int i = 0; i < transform.childCount; i ++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        GenerateMaze();
        SpawnMaze();
    }

    int NumberOfVisitedNeighbours(Vector2Int pos)
    {
        int count = 0;
        //si il on est dans le tableau alors on peut regarder la position
        if (pos.x < size_x - 1 && cells[pos.x + 1, pos.y] > 0)  // si on est a pos.x est qu'on est tjr dans le tableau count++
        {
            count++;
        }
        if(pos.x > 0 && cells[pos.x - 1, pos.y] > 0)
        {
            count++;
        }
        if (pos.y < size_y - 1 && cells[pos.x , pos.y + 1] > 0)  
        { 
            count++;
        }
        if(pos.y > 0 && cells[pos.x , pos.y - 1] > 0)
        {
            count++;
        }
        return count;
    }

    void AddNeighbouringWallsToList(Vector2Int pos)
    {
        if (pos.x < size_x - 1 && cells[pos.x + 1, pos.y] <= 0)  //tout ce qui est plus petit ou égale a 0 est un mur sinon c'est un passage?
        {
            Vector2Int newWall = new Vector2Int(pos.x +1, pos.y);
            if(!walls.Contains(newWall))
            {
                walls.Add(newWall);
            }
            
        }
        if(pos.x > 0 && cells[pos.x - 1, pos.y] <= 0)
        {
            Vector2Int newWall = new Vector2Int(pos.x -1, pos.y);
            if(!walls.Contains(newWall))
            {
                walls.Add(newWall);
            }
        }
        if (pos.y < size_y - 1 && cells[pos.x, pos.y +1] <= 0)  
        {
            Vector2Int newWall = new Vector2Int(pos.x, pos.y +1);
            if(!walls.Contains(newWall))
            {
                walls.Add(newWall);
            }
        }
        if(pos.y > 0 && cells[pos.x , pos.y -1] <= 0)
        {
            Vector2Int newWall = new Vector2Int(pos.x, pos.y -1);
            if(!walls.Contains(newWall))
            {
                walls.Add(newWall);
            }
        }
    }

    void GenerateMaze()
    {
        //2a Pick a cell, mark it as part of the maze
        cells [0,0] = 1;

        //2b add the walls of the cell to the wall list
        walls.Add (new Vector2Int (1,0));
        walls.Add(new Vector2Int (0,1));

        //3. while there are walls in the list
        while(walls.Count > 0)
        {
            // 3.1a Pick a random wall from the list
            int wall_index = Random.Range(0, walls.Count); 
            Vector2Int wall = walls[wall_index]; // position du mur qu'on va regardé


            //3.1b if only one of the cells that the wall divides is visited, then
            if(NumberOfVisitedNeighbours(wall) == 1)
            {
                //3.1.1 Make the wall a passage and mark the unvisited cell as part of the maze
                cells[wall.x, wall.y] = 1; //wall c'est un vector2

                //3.1.2 Add the neighboring walls of the cell to the wall list
                AddNeighbouringWallsToList(wall);
            }
             //3.2 remove the wall from the list
             walls.Remove(wall);


        }
    }
    void SpawnMaze()
    {
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                if(cells[i,j] <= 0)
                {
                    Instantiate(wallPrefab, new Vector3( i*gridSize, 0, j*gridSize), Quaternion.identity, transform); //mnt c'est un vector3 parce que c'est un endroit dans l'expace(3D)
                }
            }
        }

        for(int i = -1; i < size_x + 1; i++)
        {
            Instantiate(wallPrefab, new Vector3( i*gridSize, 0, -1*gridSize), Quaternion.identity,transform);
            Instantiate(wallPrefab, new Vector3( i*gridSize, 0, size_y *gridSize), Quaternion.identity, transform);
            
        }
        for(int j = 0; j < size_y ; j++) // ici on met 0 et pas de size_y + 1 mais juste size_y parce qu'il y a déjà un cube dans l'autre sens pour évité un Z figthing
            {
                Instantiate(wallPrefab, new Vector3( -1*gridSize, 0, j*gridSize), Quaternion.identity, transform);
                Instantiate(wallPrefab, new Vector3( size_x*gridSize, 0, j*gridSize), Quaternion.identity, transform);
            
            }
    }
}
