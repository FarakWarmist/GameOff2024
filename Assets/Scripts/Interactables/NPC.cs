using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class NPC : Interactable
{
    [SerializeField] private NPCDialogueData _npcDialogue;
    private int _currentConversationIndex = 0;
    private int _conversationIndexLimit;
    private int _currentLineIndex = 0;
    private int  _linesIndexLimit;

    private bool _breakConversation;
    private bool _onConversation;
    
    private List<char> _messageDecomposed = new List<char>();
    private WaitForSeconds _textSpeed = new WaitForSeconds(0.05f);
    private bool _playerReading;

    void Start()
    {
        _conversationIndexLimit = _npcDialogue.conversations.Length;
    }

    public override void Interact(int itemIndex, int slotIndex)
    {
        _linesIndexLimit = _npcDialogue.conversations[_currentConversationIndex].conversationLines.Length;

        if(_onConversation == false)
        {
            Interaction(_npcDialogue.conversations[_currentConversationIndex].conversationLines[_currentLineIndex]);

            _currentLineIndex++;
            if(_currentLineIndex >= _linesIndexLimit)
            {
                _currentConversationIndex++;
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

    private void StopInteraction()
    {
        _playerReading = false;
        UIManager.Instance.UpdateDialogueText("");
    }
}
