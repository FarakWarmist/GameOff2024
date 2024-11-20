using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Runtime.CompilerServices;

public class NPC : Interactable
{
    [SerializeField] private NPCDialogueData _npcDialogue;
    private int _currentConversationIndex = 0;
    private int _conversationIndexLimit;
    private int _currentLineIndex = 0;
    private int  _linesIndexLimit;

    private bool _breakConversation;
    private bool _onConversation;
    private bool _leftTalkingArea;
    
    private List<char> _messageDecomposed = new List<char>();
    private WaitForSeconds _textSpeed = new WaitForSeconds(0.05f);
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
        _conversationIndexLimit = _npcDialogue.conversations.Length;
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

        _linesIndexLimit = _npcDialogue.conversations[_currentConversationIndex].conversationLines.Length;

        if(_onConversation == false)
        {
            if(_gotItem == false)
                Interaction(_npcDialogue.conversations[_currentConversationIndex].conversationLines[_currentLineIndex]);
            else
            {
                Interaction(_itemMessage);
                _currentLineIndex = 0;
            }

            _currentLineIndex++;
            if(_currentLineIndex >= _linesIndexLimit)
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
                UIManager.Instance.UpdateDialogueText("");

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
                UIManager.Instance.UpdateDialogueText(currentTextDisplayed);
            }
            else
            {
                UIManager.Instance.UpdateDialogueText("");
                break;
            }
        }

        _onConversation = false;
    }

    private void LoadMessage(string message)
    {
        UIManager.Instance.UpdateDialogueText("");
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
