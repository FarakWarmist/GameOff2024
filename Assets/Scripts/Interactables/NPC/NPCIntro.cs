using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class DialogueNode
{
    public string _dialogueText;
    public DialogueNode next;

    public DialogueNode(string text)
    {
        this._dialogueText = text;
        this.next = null;
    }
}

public class NPCIntro : Interactable
{
    private string currentMessage;
    [SerializeField] private string _characterName = "";
    private int _currentConversationIndex = 0;
    private int _conversationIndexLimit;
    private int _currentLineIndex = 0;

    private bool _breakConversation;
    private bool _onConversation;
    private bool _leftTalkingArea;
    private bool _firstConversationDone;

    [SerializeField] private string[] _dialogues;
    [SerializeField] private string _dialogue1;
    [SerializeField] private string _dialogue2;
    [SerializeField] private string _dialogue3;
    [SerializeField] private string _dialogue4;
    [SerializeField] private string _dialogue5;

    DialogueNode head;
    DialogueNode nextNode;

    private List<char> _messageDecomposed = new List<char>();
    private WaitForSeconds _textSpeed = new WaitForSeconds(0.04f);
    private bool _playerReading;

    [Header("Movement")]
    [SerializeField] private NavMeshAgent _navAgent;
    [SerializeField] private Transform _printerPosition;
    [SerializeField] private GameObject _characterModel;
    [SerializeField] private float _rotationSpeed = 3.5f;
    [SerializeField] private Animator _animator;


    void Start()
    {
        _dialogue1 = Localization.GetString("npc_intro_1_dialogue_1");
        _dialogue2 = Localization.GetString("npc_intro_1_dialogue_2");
        _dialogue3 = Localization.GetString("npc_intro_1_dialogue_3");
        _dialogue4 = Localization.GetString("npc_intro_2_dialogue_1");
        _dialogue5 = Localization.GetString("npc_intro_2_dialogue_2");
        _dialogues = new string[] { _dialogue1, _dialogue2, _dialogue3, _dialogue4, _dialogue5 };
        _conversationIndexLimit = _dialogues.Length;
        head = new DialogueNode("");
    }

    void Update()
    {
        if(_firstConversationDone && _navAgent.velocity != Vector3.zero)
        {
            _animator.SetBool("Moving", true);
        }
        else
            _animator.SetBool("Moving", false);

    }

    public override void Interact(int itemIndex, int slotIndex)
    {
        if(_onConversation == false)
        {
            ShowDialogue();
            Interaction(head._dialogueText);

            _currentLineIndex++;
            if(_currentLineIndex >= _conversationIndexLimit)
            {
                _currentConversationIndex++;
                if(_firstConversationDone == false)
                {
                    _firstConversationDone = true;
                    //Trigger the logic for going to the printer
                    _characterModel.transform.rotation = Quaternion.Euler(0, 0, 0);
                    _navAgent.SetDestination(_printerPosition.position);
                }

                if(_currentConversationIndex >= _conversationIndexLimit)
                {
                    _currentConversationIndex = _conversationIndexLimit - 1;
                }

                _currentLineIndex = 0;
            }
        }
        else
        {
            _breakConversation = true;
        }
    }
    
    private void Interaction(string message)
    {
        currentMessage = message;

        _playerReading = true;

        LoadMessage(message);

        StartCoroutine(PopulateMessage());
    }

    private IEnumerator PopulateMessage()
    {
        _onConversation = true;

        foreach(char c in _messageDecomposed.ToList())
        {
            if(_breakConversation)
            {
                UIManager.Instance.UpdateDialogueText(currentMessage, _characterName);

                if(_leftTalkingArea)
                {
                    UIManager.Instance.SetDialogueActivity(false);
                    _leftTalkingArea = false;
                    if(_currentLineIndex > 0) _currentLineIndex--;
                }

                _breakConversation = false;
                _onConversation = false;
                break;
            }

            if(_playerReading && _breakConversation == false && _onConversation)
            {
                yield return _textSpeed;
                string currentTextDisplayed = UIManager.Instance.GetCurrentDialogueText();
                currentTextDisplayed += c;
                UIManager.Instance.UpdateDialogueText(currentTextDisplayed, _characterName);
                AudioManager.Instance.PlaySFX("NPCTalk");
            }
            else
            {
                UIManager.Instance.UpdateDialogueText("", _characterName);
                break;
            }
        }

        _onConversation = false;
    }

    private void LoadMessage(string message)
    {
        UIManager.Instance.UpdateDialogueText("", _characterName);
        _messageDecomposed.Clear();

        foreach(char c in message)
        {
            _messageDecomposed.Add(c);
        }
    }

    public void LeaveTalkingArea()
    {
        if(_onConversation)
        {
            _breakConversation = true;
            _leftTalkingArea = true;
        }
    }

    void ShowDialogue()
    {
        nextNode = new DialogueNode(_dialogues[_currentLineIndex]);
        head.next = nextNode;
        head = head.next;
    }
}
