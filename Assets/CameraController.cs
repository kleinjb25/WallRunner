using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Camera firstPersonCamera;
    public RenderTexture renderTexture;
    public Image imageToSample;
    private Texture2D texture2D;
    Vector3 player3DPosition;
    public GameObject Right;
    public GameObject Left;
    public GameObject Up;
    public GameObject Down;
    public float gravity;
    public float groundTime;
    Vector3 lastPlayerPos;
    Vector3 lastCamRot;
    public Image whiteBackground; //end of the game, either when u die or win
    public GameObject youWin;
    public GameObject youLose;
    public GameObject whatToDo;
    public AudioSource audioSource;
    public AudioClip success;
    public AudioClip die;
    public AudioClip jump;
    private bool gameEnded = false;
    public GameObject music;
    void Awake()
    {
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
    }
    void Start()
    {
        Time.timeScale = 1f;
        whiteBackground.enabled = false;
        youWin.SetActive(false);
        whatToDo.SetActive(false);
        Cursor.visible = false; // hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor to the center of the screen
        
    }
    
    void Update()
    {

        if (!gameEnded)
        {
            if (checkCollision(Left).Equals(Color.green) || checkCollision(Right).Equals(Color.green) || checkCollision(Up).Equals(Color.green) || checkCollision(Down).Equals(Color.green))
            {
                //uncomment for full game development, also when testing, need to make it so success audioclip actually plays when changing scenes
                /*try { 
                    
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
                    
                }
                catch {
                    audioSource.PlayOneShot(success);
                    gameEnded = true;
                    Time.timeScale = 0f;
                    whiteBackground.enabled = true;
                    youWin.SetActive(true);
                    whatToDo.SetActive(true);
                }*/
                audioSource.PlayOneShot(success);
                gameEnded = true;
                Time.timeScale = 0f;
                whiteBackground.enabled = true;
                youWin.SetActive(true);
                whatToDo.SetActive(true);

            }
            if (imageToSample.transform.position.y < 0)
            {
                gameEnded = true;
                audioSource.PlayOneShot(die);
                Time.timeScale = 0f;
                whiteBackground.enabled = true;
                youLose.SetActive(true);
                whatToDo.SetActive(true);
            }
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        texture2D.Apply();

        Vector3 imageSamplePos = imageToSample.GetComponent<RectTransform>().position;
        if (player3DPosition != new Vector3())
        {
            Vector3 oldImageSamplePos = imageSamplePos;
            imageSamplePos.x = firstPersonCamera.WorldToScreenPoint(player3DPosition).x;
            imageSamplePos.y = firstPersonCamera.WorldToScreenPoint(player3DPosition).y;
            if (Vector3.Distance(oldImageSamplePos, imageSamplePos) > 50)
            {
                player.position = lastPlayerPos;
                firstPersonCamera.transform.eulerAngles = lastCamRot;
                imageSamplePos = oldImageSamplePos;
            }
            
        }
        imageToSample.GetComponent<RectTransform>().position = imageSamplePos;
        gravity -= Physics.gravity.y * Time.deltaTime;
        imageToSample.GetComponent<RectTransform>().position += Vector3.down*gravity;
        imageToSample.GetComponent<RectTransform>().position += Vector3.right * Input.GetAxis("Horizontal") *300*Time.deltaTime;
        while (checkCollision(Left).Equals(Color.white))
            imageToSample.GetComponent<RectTransform>().position += Vector3.right;
        while (checkCollision(Right).Equals(Color.white))
            imageToSample.GetComponent<RectTransform>().position += Vector3.left;
        while (checkCollision(Up).Equals(Color.white))
            imageToSample.GetComponent<RectTransform>().position += Vector3.down;
        while (checkCollision(Down).Equals(Color.white))
        {
            imageToSample.GetComponent<RectTransform>().position += Vector3.up;
            gravity = 0;
            groundTime = Time.realtimeSinceStartup + 0.1f;
        }
        if (Time.realtimeSinceStartup < groundTime && Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(jump);
            gravity = -3;
        }
        
        RaycastHit hit;
        Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out hit);
        player3DPosition = firstPersonCamera.ScreenToWorldPoint(new Vector3(imageToSample.GetComponent<RectTransform>().position.x, imageToSample.GetComponent<RectTransform>().position.y, hit.distance));
        lastPlayerPos = player.position;
        lastCamRot = firstPersonCamera.transform.eulerAngles;
        player.transform.LookAt(firstPersonCamera.ScreenToWorldPoint(new Vector3(imageToSample.GetComponent<RectTransform>().position.x, imageToSample.GetComponent<RectTransform>().position.y, 1)));
        Vector3 eulerAngles = player.transform.eulerAngles;
        eulerAngles.x = 0;
        eulerAngles.z = 0;
        player.transform.eulerAngles = eulerAngles;
        
    }
    public Color checkCollision(GameObject check)
    {
        RectTransform imageRect = check.GetComponent<RectTransform>();
        Rect uvRect = new Rect(imageRect.position.x / Screen.width, imageRect.position.y / Screen.height,
        imageRect.rect.width / Screen.width, imageRect.rect.height / Screen.height);
        Vector2Int pixelPosition = new Vector2Int((int)(uvRect.x * renderTexture.width), (int)(uvRect.y * renderTexture.height));
        
        return texture2D.GetPixel(pixelPosition.x + (int)(imageRect.rect.width / 2), pixelPosition.y + (int)(imageRect.rect.height / 2));
        
    }



}
