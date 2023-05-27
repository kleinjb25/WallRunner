using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform player;
    float cameraVerticalRotation = 0f;
    public Camera firstPersonCamera;
    public RenderTexture renderTexture;
    public Image imageToSample;
    private Texture2D texture2D;
    public static bool is2D = false;
    public GameObject platformPrefab;
    public Sprite platformSprite;
    public Material platformMaterial;
    private bool isPlayerLookingAtWhitePixel = false;

    void Start()
    {

        Cursor.visible = false; // hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor to the center of the screen
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X"); // get the mouse X axis
        float inputY = Input.GetAxis("Mouse Y"); // get the mouse Y axis
        cameraVerticalRotation -= inputY; // subtract the Y axis input from the camera rotation
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, 0f, 0f); // clamp the camera rotation to prevent the player from looking too far up or down
        if (!is2D)
        {
            transform.localEulerAngles = Vector3.right * cameraVerticalRotation; // rotate the camera on the X axis
            player.Rotate(Vector3.up * inputX); // rotate the player on the Y axis
        }
        RectTransform imageRect = imageToSample.GetComponent<RectTransform>();

        // Calculate the UV coordinates of the image on the RenderTexture
        Rect uvRect = new Rect(imageRect.position.x / Screen.width, imageRect.position.y / Screen.height,
            imageRect.rect.width / Screen.width, imageRect.rect.height / Screen.height);

        // Calculate the pixel position of the bottom-left corner of the image on the RenderTexture
        Vector2Int pixelPosition = new Vector2Int((int)(uvRect.x * renderTexture.width), (int)(uvRect.y * renderTexture.height));
        // Read the pixel data from the RenderTexture into the Texture2D
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        // Apply the modifications to the RenderTexture
        texture2D.Apply();
        // Use the sampled pixel color
        Color pixelColor = texture2D.GetPixel(pixelPosition.x + (int)(imageRect.rect.width / 2), pixelPosition.y + (int)(imageRect.rect.height / 2));
        //Debug.Log("Pixel Color: " + pixelColor);
        bool isLookingAtWhitePixel = (pixelColor == Color.white);

        // Enable/disable the image based on the pixel color
        if (isLookingAtWhitePixel)
        {
            Activate2DMechanics();
            GeneratePlatforms();
        }
    }

    private void GeneratePlatforms()
    {
        if (!isPlayerLookingAtWhitePixel)
            return;

        int numPlatforms = Random.Range(1, 6); // Generate between 1 and 5 platforms

        for (int i = 0; i < numPlatforms; i++)
        {
            // Generate random X and Y coordinates within the RenderTexture bounds
            float randomX = Random.Range(0, renderTexture.width);
            float randomY = Random.Range(0, renderTexture.height);

            // Instantiate the platform at the generated position
            Vector3 platformPosition = new Vector3(randomX, randomY, 0);
            GameObject platform = Instantiate(platformPrefab, platformPosition, Quaternion.identity);

            float scaleBoundsX = Random.Range(0.01f, 1);
            float scaleBoundsY = Random.Range(0.05f, 0.9f);
            platform.transform.localScale = new Vector3(scaleBoundsX, scaleBoundsY, 1);
            SpriteRenderer spriteRenderer = platform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Assign a sprite to the platform
            // Replace "YourSprite" with the actual sprite you want to use
            spriteRenderer.sprite = platformSprite;
        }
        else
        {
            Renderer renderer = platform.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Assign a material to the platform
                // Replace "YourMaterial" with the actual material you want to use
                renderer.material = platformMaterial;
            }
        }
        }

        
    }

    private void Activate2DMechanics()
    {
        imageToSample.gameObject.SetActive(true);
        platformPrefab.SetActive(true);
        is2D = true;
        isPlayerLookingAtWhitePixel = true;
    }

    private void Deactivate2DMechanics()
    {
        // Add your code here to deactivate the 2D mechanics
        Debug.Log("2D mechanics deactivated!");
    }
    private void OnDestroy()
    {
        // Release the temporary Texture2D
        if (texture2D != null)
            Destroy(texture2D);
    }
}
