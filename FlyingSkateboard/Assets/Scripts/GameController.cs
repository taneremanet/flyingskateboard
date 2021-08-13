using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Rigidbody player;
    bool swipeLeft, swipeRight, swipeUp;    

    public List<GameObject> roads;
    public List<GameObject> tempRoads = new List<GameObject>(3);

    public List<GameObject> cars;
    public List<GameObject> tempCars = new List<GameObject>();

    public List<GameObject> obstacles;    

    public List<GameObject> coinGroup;

    public AudioSource whoop;
    public AudioSource jump;
    public AudioSource coin;

    [SerializeField]public List<GameObject> coinAndObstacleTemp;    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        whoop.gameObject.SetActive(true);
        jump.gameObject.SetActive(true);
        coin.gameObject.SetActive(true);

        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].SetActive(true);

            Vector3 temp = roads[i].transform.position;
            temp.z += 30f * i;
            roads[i].transform.position = temp;
        }
    }

    void Update()
    {
        if (MobileInput.Instance.SwipeLeft)
        {
            swipeLeft = true;            
        }

        if (MobileInput.Instance.SwipeRight)
        {
            swipeRight = true;
        }

        if (MobileInput.Instance.SwipeUp)
        {            
            swipeUp = true;
        }

        //if (MobileInput.Instance.SwipeDown)
        //{

        //}

        if(swipeLeft)
        {           
            whoop.Play();
            Vector3 pos = player.position;
            pos.x = Mathf.MoveTowards(pos.x, -2.3f, Time.deltaTime * 14f);
            player.position = pos;

            if ((int)player.position.x == (int)-2.3f)
            {
                swipeLeft = false;
            }
        }

        if(swipeRight)
        {
            whoop.Play();
            Vector3 pos = player.position;
            pos.x = Mathf.MoveTowards(pos.x, +2.3f, Time.deltaTime * 14f);
            player.position = pos;

            if ((int)player.position.x == (int)2.3f)
            {
                swipeRight = false;
            }
        }

        if (swipeUp)
        {
            swipeLeft = swipeRight = false;
            jump.Play();
            Quaternion currentRot = player.rotation;            
            currentRot = Quaternion.Slerp(currentRot, Quaternion.Euler(-40f, 0, 0), Time.deltaTime * 10f);
            player.rotation = currentRot;

            Vector3 pos = player.position;            
            pos.y = Mathf.MoveTowards(pos.y, +2f, Time.deltaTime * 10f);
            player.position = pos;

            

            if ((int)player.position.y == (int)2f)
            {                
                StartCoroutine(Wait());
            }    
        }        

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.4f);

            Vector3 pos = player.position;
            pos.y = Mathf.MoveTowards(pos.y, +0.5f, Time.deltaTime * 8f);
            player.position = pos;

            Quaternion currentrot = player.rotation;
            currentrot = Quaternion.Slerp(currentrot, Quaternion.Euler(0, 0, 0), Time.deltaTime * 8f);
            player.rotation = currentrot;           
            swipeUp = false;            
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Replay()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
