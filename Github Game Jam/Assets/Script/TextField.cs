//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using Yarn.Unity;
//using UnityEngine.UI;

//public class TextField : MonoBehaviour
//{
//    [SerializeField]
//    TextMeshProUGUI text;

//    [SerializeField]
//    DialogueRunner runner;

//    [SerializeField]
//    Image pfpBox;

//    [SerializeField]
//    Sprite playerPfp;

//    public bool inDialog { get; set; } = false;
//    Dictionary<string, Sprite> nameToPfp = new Dictionary<string, Sprite>();

//    public static TextField instance = null;

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else if (instance != this)
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void Start()
//    {
//        runner.AddCommandHandler("SetPic", HandleSetPic);
//        runner.AddCommandHandler("SetPlayerPic", HandleSetPlayerPic);
//    }

//    private void Update()
//    {
//        if (inDialog && Input.GetKeyDown(KeyCode.Space))
//        {
//            runner.Dialogue.Continue();
//        }
//    }

//    public void ShowLine(string line)
//    {
//        text.text = line;
//    }

//    public void RegisterSpeaker(YarnProgram script, string name = "", Sprite pfp = null)
//    {
//        runner.Add(script);

//        if (pfp && name != "")
//        {
//            nameToPfp.Add(name, pfp);
//        }
//    }

//    public void StartDialog(string node)
//    {
//        if (!inDialog)
//            runner.StartDialogue(node);
//    }

//    private void HandleSetPic(string[] args)
//    {
//        if (args.Length != 1)
//            throw new UnityException("Error: Command 'SetPic' should have 1 argument, came with " + args.Length.ToString() + " args.");

//        string name = args[0];
//        if (!nameToPfp.ContainsKey(name))
//            throw new UnityException("Error: Command 'SetPic' has unknown name '" + name + "'.");

//        pfpBox.sprite = nameToPfp[name];
//    }

//    private void HandleSetPlayerPic(string[] args)
//    {
//        if (args != null)
//            throw new UnityException("Error: Command 'SetPlayerPic' should have no argument, came with " + args.Length.ToString() + " args.");

//        pfpBox.sprite = playerPfp;
//    }
//}
