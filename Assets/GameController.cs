using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Client.Models;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public static class Secrets

{
    public static string client_secret = "juki9vj12wl7ej6e6nolxsnadtppy4";

}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    List<TwitchPlayer> players;

    public GameObject twitchPlayerPrefab;
    public GameObject newPlayerTextPrefab;

    public Canvas canvas;

    public Transform startPlayerNode;

    public TMP_Text startGameText;
    public TMP_Text endGameText;
    public TMP_Text waitingText;

    public AudioMixer audioMixer;
    public AudioSource gameMusic;
    public AudioSource winJingle;
    public AudioSource winCheer;

    private Client client;

    private Queue<System.Tuple<string, string>> requests;

    private bool gameStarted = false;
    public bool gameRunning { get; private set; }

    private List<Egg> eggs;

    private void Awake()
    {
        instance = this;
        gameRunning = false;
        audioMixer.SetFloat("MusicVol", -7.0f);

        players = new List<TwitchPlayer>();
        eggs = new List<Egg>();
    }

    // Start is called before the first frame update
    void Start()
    {
        requests = new Queue<System.Tuple<string, string>>();

        //HTTPHelper.Instance.StartNewListener(HTTPCallback);

        ConnectionCredentials credentials = new ConnectionCredentials("moramunch", Secrets.client_secret);
        client = new Client();
        client.Initialize(credentials, "moramunch");
        client.Connect();

        client.OnMessageReceived += OnChatMessageReceived;
    }

    private void OnChatMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        ChatMessage ChatMessage = e.ChatMessage;
        requests.Enqueue(new System.Tuple<string, string>(ChatMessage.Message, ChatMessage.Username));
    }

    private void Update()
    {
        if(!gameStarted && Input.GetAxis("StartGame") > 0)
        {
            StartGame();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        if(requests.Count > 0)
        {
            System.Tuple<string, string> nextRequest = requests.Dequeue();

            string command = nextRequest.Item1;
            string username = nextRequest.Item2;

            TwitchPlayer player = players.Find(entry => entry.username == username);
            if (player == null && !gameRunning)
            {
                if (command == "join")
                {
                    player = Instantiate(twitchPlayerPrefab, startPlayerNode.transform.position, Quaternion.identity).GetComponent<TwitchPlayer>();
                    Vector3 textPos = new Vector3(Random.Range(-300.0f, 300.0f), Random.Range(-250.0f, 250.0f), 0.0f);
                    GameObject newPlayerText = Instantiate(newPlayerTextPrefab, canvas.transform);
                    newPlayerText.transform.localPosition = textPos;
                    newPlayerText.GetComponent<NewPlayerText>().nameText.text = username;
                    player.SetName(username);
                    players.Add(player);
                }
            }
            else
            {
                player.NewInput(command);
            }
        }
    }

    private void HTTPCallback(System.IAsyncResult result)
    {
        var context = HTTPHelper.EndContext(result);
        if (context.Request.HttpMethod == "POST")
        {
            string newName = "";
            string data_text = new StreamReader(context.Request.InputStream,
                                context.Request.ContentEncoding).ReadToEnd();
            string[] text_split = data_text.Split(',');
            
            string command = text_split[0];

            if (text_split.Length > 1 && text_split[1] != string.Empty)
            {
                newName = text_split[1];
            }

            requests.Enqueue(new System.Tuple<string, string>(command, newName));

        }
        context.Response.Close();
    }

    private void StartGame()
    {
        gameStarted = true;
        gameRunning = true;
        StartCoroutine(ShowStartText());
        gameMusic.Play();
    }

    private void EndGame(string winner)
    {
        endGameText.text = winner + " wins!";
        endGameText.enabled = true;
        gameRunning = false;
        StartCoroutine(FadeMusic());
        winJingle.Play();
        winCheer.Play();
    }

    public void RegisterEgg(Egg egg)
    {
        if (!eggs.Contains(egg))
        {
            eggs.Add(egg);
        }
    }

    public void DeRegisterEgg(Egg egg)
    {
        eggs.Remove(egg);

        if(eggs.Count == 0)
        {
            EndGame("Mora");
        }
    }

    public void CatchMora(TwitchPlayer player)
    {
        EndGame(player.username);
    }

    private IEnumerator ShowStartText()
    {
        waitingText.enabled = false;
        startGameText.enabled = true;
        yield return new WaitForSeconds(5.0f);
        startGameText.enabled = false;
    }

    private IEnumerator FadeMusic()
    {
        float timer = 1.0f;

        float startVol;
        string musicVolParam = "MusicVol";
        audioMixer.GetFloat(musicVolParam, out startVol);
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            audioMixer.SetFloat(musicVolParam, Mathf.Lerp(-60.0f, startVol, timer));
            yield return 0;
        }
    }
}
