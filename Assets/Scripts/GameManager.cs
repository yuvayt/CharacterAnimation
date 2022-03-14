using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Variables

    [SerializeField] private GameObject obstacle;
    [SerializeField] private LayerMask otherMask;
    private int numCubes;
    private bool spawnable = true;
    #endregion


    #region MonoBehavior
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    void Update()
    {
        
        if (spawnable)
        {
            RandomSpawn();
        } else
        {
            //Debug.Log(numCubes);
        }

    }
    #endregion


    #region Methods

    private void RandomSpawn()
    {

        Vector3 rp = RandomPosition();
        //Debug.Log(rp);
        if (rp != Vector3.zero)
        {
            SpawnCube(rp);
            numCubes++;
        }
        else
        {
            spawnable = false;
            Debug.Log(numCubes);
        }
           
    }

    private void SpawnCube(Vector3 pos)
    {
        float xScale = Random.value * 2f + 1f;
        float yScale = Random.value * 2f + 1f;
        float zScale = Random.value * 2f + 1f;

        obstacle.transform.localScale = new Vector3(xScale, yScale, zScale);

        pos.y += yScale;

        Instantiate(obstacle, pos, Quaternion.identity);
    }

    private Vector3 RandomPosition(int time = 0)
    {
        if (time > 20)
            return Vector3.zero;
        
        float x = Random.Range(-250, 250);
        float z = Random.Range(-250, 250);

        Vector3 spherePos = new Vector3(x, 1f, z);
        if (Physics.CheckSphere(spherePos, 10f, otherMask))
            return RandomPosition(++time);
        return spherePos;

    }

    #endregion

}
