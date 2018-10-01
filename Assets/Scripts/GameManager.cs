using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Maze mazePrefab;
    private Maze mazeInstance;
    public Player playerPrefab;
    private Player playerInstance;
    public Win winPrefab;
    private Win winInstance;
    public Ending endingPrefab;
    private Ending endingInstance;
    private Collider2D obPlayer;
    private Collider2D obBox;
    Vector3 trans = new Vector3(1.0f, 0f, 1.0f);

    private void Start()
    {
        StartCoroutine(BeginGame());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
        float dist = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, GameObject.FindGameObjectWithTag("Finish").transform.position);
        if(dist<0.6)
        {
            gameWin();
        }
    }

    private IEnumerator BeginGame()
    {
        yield return new WaitForSeconds(20);
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());
        winInstance = Instantiate(winPrefab) as Win;
        winInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
        }
        StartCoroutine(BeginGame());
    }

    void gameWin()
    { 
            StopAllCoroutines();
            Destroy(mazeInstance.gameObject);
            if (playerInstance != null)
            {
                Destroy(playerInstance.gameObject);
            }
            if (winInstance != null)
            {
               Destroy(winInstance.gameObject);
            }
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        GameObject cam = GameObject.Find("Main Camera");
        var videoPlayer = cam.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCameraAlpha = 0.5F;
        videoPlayer.url = "/Users/erinlhiggins/Desktop/Creative Work/ending.mp4";
        videoPlayer.frame = 50;
        videoPlayer.isLooping = true;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.playbackSpeed = videoPlayer.playbackSpeed / 2.0F;
        videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }
}
