using System;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Gameplay
{
    public class CharacterSelect : MonoBehaviour
    {
        public GameObject[] playerCharacters;
        private int selectedCharacter = 0;
        public UnityAction OnCharacterSelected;
        public UnityAction OnCloseCharacterSelection;
        [SerializeField] private InputReader _inputReader;

        private void OnEnable()
        {
            _inputReader.MenuCloseEvent += CloseCharacterSelectionButton;
        }

        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseCharacterSelectionButton;
        }

        public void Setup()
        {
            playerCharacters[selectedCharacter].gameObject.SetActive(false);
            selectedCharacter = GameManager.Instance.characterIndex;
            playerCharacters[selectedCharacter].gameObject.SetActive(true);
        }

        public void ButtonCharacterSelected()
        {
            OnCharacterSelected?.Invoke();
        }

        public void CloseCharacterSelectionButton()
        {
            OnCloseCharacterSelection?.Invoke();
        }
    
        public void LeftButton()
        {
            playerCharacters[selectedCharacter].gameObject.SetActive(false);

            if (selectedCharacter == 0)
            {
                selectedCharacter = playerCharacters.Length - 1;
            }
            else
            {
                selectedCharacter--;
            }

            playerCharacters[selectedCharacter].gameObject.SetActive(true);
            GameManager.Instance.characterIndex = selectedCharacter;
        }

        public void RightButton()
        {
            playerCharacters[selectedCharacter].gameObject.SetActive(false);

            if (selectedCharacter == playerCharacters.Length - 1)
            {
                selectedCharacter = 0;
            }
            else
            {
                selectedCharacter++;
            }

            playerCharacters[selectedCharacter].gameObject.SetActive(true);
            GameManager.Instance.characterIndex = selectedCharacter;
        }
    }
}
