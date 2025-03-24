using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Runtime.CompilerServices;

public class NPC : Interactable
{
    private string currentMessage;
    [SerializeField] private string _characterName = "";
    [SerializeField] private NPCDialogueData _npcDialogue;
    [SerializeField] private string[] _dialogues;
    private int _currentConversationIndex = 0;
    private int _conversationIndexLimit;
    private int _currentLineIndex = 0;

    private bool _breakConversation;
    private bool _onConversation;
    private bool _leftTalkingArea;

    [SerializeField] private string _dialogue1;
    [SerializeField] private string _dialogue2;
    [SerializeField] private string _dialogue3;
    [SerializeField] private string _dialogue4;
    
    private List<char> _messageDecomposed = new List<char>();
    private WaitForSeconds _textSpeed = new WaitForSeconds(0.04f);
    private bool _playerReading;

    [Header("Item")]
    [SerializeField] private bool _needItem;
    [SerializeField] private int _itemNeededIndex = 11;
    string _itemMessage = "Thank you, the code for the safe box is ";
    private bool _gotItem;
    private int[] _code = new int[3];
    private bool _gotCode;

    void Start()
    {
        _dialogue1 = Localization.GetString("npc_1_dialogue_1");
        _dialogue2 = Localization.GetString("npc_1_dialogue_2");
        _dialogue3 = Localization.GetString("npc_1_dialogue_3");
        _dialogue4 = Localization.GetString("npc_1_dialogue_4");
        _dialogues = new string[] {_dialogue1, _dialogue2, _dialogue3, _dialogue4};
        _conversationIndexLimit = _dialogues.Length;
    }

    public override void Interact(int itemIndex, int slotIndex)
    {
        if(_gotCode == false && _needItem)
        {
            _code = GameManager.Instance._safeBoxCode;
            _itemMessage += _code[0].ToString() + " " + _code[1].ToString() + " " + _code[2].ToString();   
            _gotCode = true;
        }

        if(itemIndex == _itemNeededIndex)
        {
            Inventory.Instance.UpdateInventorySlot(slotIndex, null);
            _gotItem = true;
        }

        if(_onConversation == false)
        {
            if(_gotItem == false)
                Interaction(_dialogues[_currentLineIndex]);
            else
            {
                Interaction(_itemMessage);
                _currentLineIndex = 0;
            }

            _currentLineIndex++;
            if (_currentLineIndex >= _conversationIndexLimit)
            {
                if(_needItem == false)
                {
                    _currentConversationIndex++;
                    if(_currentConversationIndex >= _conversationIndexLimit)
                    {
                        _currentConversationIndex = _conversationIndexLimit - 1;
                    }
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
}
