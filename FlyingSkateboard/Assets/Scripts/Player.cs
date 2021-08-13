using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody myBody;
    private float speed = 10f;    

    private int counter;
    private int coinCounter;
    private GameObject lastRoad;
    private GameObject lastCar;
    public GameObject gameMenuController;

    private Vector3 spawnPos;
    private int controlX;
    private float distance;
    private float distanceL;
    float carSize;
    [SerializeField] int coin;

    public Text coinText;
    public Text bestText;

    public GameObject gameOverPanel;

    //int[] randomNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
    List<int> randomNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
    List<int> randomCoinNumbers = new List<int> { 0, 1 };

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Time.timeScale = 1;
        lastRoad = GameController.Instance.roads[GameController.Instance.roads.Count - 1];
        spawnPos.z = 40f;
        SpawnCars();
        lastCar = GameController.Instance.cars[GameController.Instance.cars.Count - 1];
        coin = 0;

        if (!PlayerPrefs.HasKey("best"))
        {
            PlayerPrefs.SetInt("best", 0);
        }
        bestText.text = "Best: " + PlayerPrefs.GetInt("best").ToString();
    }

    private void Update()
    {
        for (int i = 0; i < GameController.Instance.cars.Count; i++)
        {
            if (myBody.position.z > GameController.Instance.cars[i].transform.position.z + 10f)
            {
                distance = UnityEngine.Random.Range(0, 2) == 1 ? 17f : 20f;
                distanceL = UnityEngine.Random.Range(0, 2) == 1 ? 21f : 25f;

                GameController.Instance.cars[i].SetActive(false);
                GameController.Instance.tempCars.Add(GameController.Instance.cars[i]);

                Vector3 temp = GameController.Instance.tempCars[i].transform.position;
                temp.x = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.z = 0f;
                if (GameController.Instance.tempCars[i].tag == "Car")
                {
                    temp.z += lastCar.transform.position.z + distance;
                }
                else if (GameController.Instance.tempCars[i].tag == "CarL")
                {
                    temp.z += lastCar.transform.position.z + distanceL;
                }
                GameController.Instance.tempCars[i].transform.position = temp;

                GameController.Instance.tempCars[i].SetActive(true);
                lastCar = GameController.Instance.tempCars[i];
            }

            if (i == GameController.Instance.cars.Count)
            {
                GameController.Instance.tempCars.Clear();
            }
        }
    }

    void FixedUpdate()
    {
        //myBody.velocity = new Vector3(0, 0, speed);

        if (coin >= 0 && coin < 15)
        {
            myBody.velocity = new Vector3(0, 0, speed);
        }

        if (coin >= 15 && coin < 30 )
        {
            myBody.velocity = new Vector3(0, 0, speed + 1f);
        }

        if (coin >= 30 && coin < 45)
        {
            myBody.velocity = new Vector3(0, 0, speed + 2f);
        }

        if (coin >= 45 && coin < 60)
        {
            myBody.velocity = new Vector3(0, 0, speed + 3f);
        }

        if (coin >= 60 && coin < 75)
        {
            myBody.velocity = new Vector3(0, 0, speed + 4f);
        }

        if (coin >= 75 && coin < 90)
        {
            myBody.velocity = new Vector3(0, 0, speed + 5f);
        }

        if (coin >= 90 && coin < 120)
        {
            myBody.velocity = new Vector3(0, 0, speed + 6f);
        }

        if (coin > 120)
        {
            myBody.velocity = new Vector3(0, 0, speed + 7f);
        }
    }

    void Shuffle(List<GameObject> arrayToShuffle)
    {
        for (int i = 0; i < arrayToShuffle.Count; i++)
        {
            GameObject temp = arrayToShuffle[i];
            int random = UnityEngine.Random.Range(i, arrayToShuffle.Count);
            arrayToShuffle[i] = arrayToShuffle[random];
            arrayToShuffle[random] = temp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.position.z <= other.gameObject.transform.position.z)
        {
            if (other.transform.tag == "Car" || other.transform.tag == "CarL" || other.transform.tag == "Obstacle")
            {
                GameController.Instance.whoop.gameObject.SetActive(false);
                GameController.Instance.jump.gameObject.SetActive(false);
                GameController.Instance.coin.gameObject.SetActive(false);
                gameMenuController.GetComponent<GameMenuController>().pauseButton.interactable = false;

                Time.timeScale = 0;
                gameOverPanel.SetActive(true);
            }
        }

        if (other.gameObject.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            GameController.Instance.coin.Play();
            coin++;
            coinText.text = "Coin: " + coin.ToString();

            if (coin > PlayerPrefs.GetInt("best"))
            {
                PlayerPrefs.SetInt("best", coin);
                bestText.text = "Best: " + PlayerPrefs.GetInt("best");
            }
        }

        if (other.transform.tag == "Road")
        {
            other.transform.parent.gameObject.SetActive(false);
            GameController.Instance.tempRoads.Add(other.transform.parent.gameObject);

            counter++;

            if (counter == 2)
            {
                Shuffle(GameController.Instance.tempRoads);

                for (int i = 0; i < GameController.Instance.tempRoads.Count; i++)
                {
                    Vector3 temp = GameController.Instance.tempRoads[i].transform.position;
                    temp.z = 0f;
                    temp.z += lastRoad.transform.position.z + 30f;
                    GameController.Instance.tempRoads[i].transform.position = temp;

                    GameController.Instance.tempRoads[i].SetActive(true);

                    lastRoad = GameController.Instance.tempRoads[i];
                }

                counter = 0;
                GameController.Instance.tempRoads.Clear();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CoinGroup" || other.gameObject.tag == "Obstacle")
        {
            GameObject lastCarPos = lastCar;

            other.gameObject.SetActive(false);

            if (other.gameObject.tag == "CoinGroup")
            {
                foreach (Transform child in other.gameObject.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }

            GameController.Instance.coinAndObstacleTemp.Add(other.gameObject);


            coinCounter++;

            if (coinCounter == 2)
            {
                ShuffleAnArray(randomCoinNumbers);

                for (int i = 0; i < GameController.Instance.coinAndObstacleTemp.Count; i++)
                {
                    Vector3 temp = GameController.Instance.coinAndObstacleTemp[randomCoinNumbers[i]].transform.position;
                    temp.z = 0;

                    if (GameController.Instance.coinAndObstacleTemp[randomCoinNumbers[i]].tag == "Obstacle")
                    {
                        temp.z += lastCarPos.transform.position.z;

                        if (lastCarPos.transform.position.x > 0)
                        {
                            temp.x = -2.6f;
                        }

                        if (lastCarPos.transform.position.x < 0)
                        {
                            temp.x = 2.6f;
                        }
                    }

                    if (GameController.Instance.coinAndObstacleTemp[randomCoinNumbers[i]].tag == "CoinGroup")
                    {
                        temp.z += lastCarPos.transform.position.z + lastCarPos.GetComponent<Renderer>().bounds.size.z / 2f + 4f;

                        if (lastCarPos.transform.position.x > 0)
                        {
                            if (UnityEngine.Random.Range(0, 2) == 0)
                            {
                                temp.x = -2.3f;
                            }
                            else
                            {
                                temp.x = 2.3f;
                            }
                        }

                        if (lastCarPos.transform.position.x < 0)
                        {
                            if (UnityEngine.Random.Range(0, 2) == 1)
                            {
                                temp.x = 2.3f;
                            }
                            else
                            {
                                temp.x = -2.3f;
                            }
                        }
                    }

                    GameController.Instance.coinAndObstacleTemp[randomCoinNumbers[i]].transform.position = temp;

                    GameController.Instance.coinAndObstacleTemp[randomCoinNumbers[i]].SetActive(true);

                    if (GameController.Instance.coinAndObstacleTemp[randomCoinNumbers[i]].tag == "CoinGroup")
                    {
                        foreach (Transform child in GameController.Instance.coinAndObstacleTemp[randomCoinNumbers[i]].transform)
                        {
                            child.gameObject.SetActive(true);
                        }
                    }
                }
                coinCounter = 0;
                GameController.Instance.coinAndObstacleTemp.Clear();
            }
        }
    }

    void ShuffleAnArray(List<int> myIntArray)
    {
        for (int i = 0; i < myIntArray.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, myIntArray.Count);
            int temp = myIntArray[randomIndex];
            myIntArray[randomIndex] = myIntArray[i];
            myIntArray[i] = temp;
        }
    }

    void SpawnCars()
    {
        Shuffle(GameController.Instance.cars);

        // Cars
        for (int i = 0; i < GameController.Instance.cars.Count; i++)
        {
            // Car
            Vector3 temp = GameController.Instance.cars[i].transform.position;

            if (controlX == 0)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 1;
            }
            else if (controlX == 1)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 2;
            }
            else if (controlX == 2)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 3;
            }
            else if (controlX == 3)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 4;
            }
            else if (controlX == 4)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 5;
            }
            else if (controlX == 5)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 6;
            }
            else if (controlX == 6)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 7;
            }
            else if (controlX == 7)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 8;
            }
            else if (controlX == 8)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 9;
            }
            else if (controlX == 9)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 10;
            }
            else if (controlX == 10)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 11;
            }
            else if (controlX == 11)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 12;
            }
            else if (controlX == 12)
            {
                float result = UnityEngine.Random.Range(0, 2) == 1 ? 2.3f : -2.3f;
                temp.x = result;

                controlX = 0;
            }

            distance = UnityEngine.Random.Range(0, 2) == 1 ? 17f : 20f;
            distanceL = UnityEngine.Random.Range(0, 2) == 1 ? 21f : 25f;


            if (GameController.Instance.cars[i].tag == "Car")
            {
                temp.z += spawnPos.z + distance;
            }
            else if (GameController.Instance.cars[i].tag == "CarL")
            {
                temp.z += spawnPos.z + distanceL;
            }

            GameController.Instance.cars[i].transform.position = temp;

            GameController.Instance.cars[i].SetActive(true);

            spawnPos.z = GameController.Instance.cars[i].transform.position.z;
        }


        //int rnd;
        //int counter = 0;

        //while (counter < 8)
        //{
        //    rnd = UnityEngine.Random.Range(0, GameController.Instance.cars.Count);
        //    if (Array.IndexOf(randomNumbers, rnd) == -1)
        //    {
        //        randomNumbers[counter] = rnd;
        //        counter++;
        //    }
        //}

        ShuffleAnArray(randomNumbers);

        // Coins
        for (int i = 0; i < 8; i++)
        {
            carSize = GameController.Instance.cars[randomNumbers[i]].GetComponent<Renderer>().bounds.size.z / 2f;

            Vector3 carTemp = GameController.Instance.cars[randomNumbers[i]].transform.position;
            Vector3 coinGroupTemp = GameController.Instance.coinGroup[i].transform.position;
            Vector3 obstacleTemp = GameController.Instance.obstacles[i].transform.position;

            if (carTemp.x > 0)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    coinGroupTemp.x = -2.3f;
                }
                else
                {
                    coinGroupTemp.x = 2.3f;
                    obstacleTemp.x = -2.6f;
                    GameController.Instance.obstacles[i].SetActive(true);
                }
            }

            if (carTemp.x < 0)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    coinGroupTemp.x = 2.3f;
                }
                else
                {
                    coinGroupTemp.x = -2.3f;
                    obstacleTemp.x = 2.6f;
                    GameController.Instance.obstacles[i].SetActive(true);
                }
            }

            coinGroupTemp.z += carTemp.z + carSize + 4f;
            obstacleTemp.z += carTemp.z;

            GameController.Instance.coinGroup[i].transform.position = coinGroupTemp;
            GameController.Instance.obstacles[i].transform.position = obstacleTemp;

            GameController.Instance.coinGroup[i].SetActive(true);
        }
    }
}
