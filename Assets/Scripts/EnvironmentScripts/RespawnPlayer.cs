using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] public GameObject heartsUI;
    [SerializeField] public GameObject heartsPrefab;

    private DataManager dataManager;
    private GameObject[] livesArray;
    private int numLives;
    private void Start()
    {
        if (FindAnyObjectByType<DataManager>())
        {
            dataManager = FindAnyObjectByType<DataManager>();
            numLives = dataManager.data.lives;
            GameObject respawnObject = GameObject.Find(dataManager.data.respawnPoint);
            if (dataManager.data.respawnPoint == "InitialRespawnPoint")
            {
                respawnPoint = respawnObject.transform;
            }
            else
            {
                respawnPoint = respawnObject.transform.Find("SpawnPoint").transform;
            }
            livesArray = new GameObject[numLives];
            for (int i = 0; i < numLives; i++)
            {
                if (livesArray[i] == null)
                    livesArray[i] = Instantiate(heartsPrefab, new Vector3(heartsUI.transform.position.x + (88 * i), heartsUI.transform.position.y, 0), Quaternion.identity, heartsUI.transform);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().RespawnPlayer();
        }
    }

    public void setRespawn(string newRespawn)
    {
        GameObject respawnObject = GameObject.Find(newRespawn);
        if (newRespawn != "InitialRespawnPoint")
        {
            respawnPoint = respawnObject.transform.Find("SpawnPoint").transform;
        }
        else
        {
            respawnPoint = respawnObject.transform;
        }

        if (dataManager)
        {
            dataManager.SetRespawnPoint(newRespawn);
        }
    }

    public void ResetHearts()
    {
        numLives = dataManager.data.lives;
        foreach (Transform child in heartsUI.transform)
        {
            Destroy(child.gameObject);
        }
        livesArray = new GameObject[numLives];
        for (int i = 0; i < numLives; i++)
        {
            if (livesArray[i] == null)
                livesArray[i] = Instantiate(heartsPrefab, new Vector3(heartsUI.transform.position.x + (88 * i), heartsUI.transform.position.y, 0), Quaternion.identity, heartsUI.transform);
        }
        
    }

    public Transform getRespawn()
    {
        return respawnPoint;
    }

    public void removeHeart()
    {
        numLives--;
        if (numLives > 0)
        {
            Destroy(livesArray[numLives]);
        }
        else
        {
            Destroy(livesArray[0]);
            Debug.Log("No more lives - Death");
        }
    }
}
