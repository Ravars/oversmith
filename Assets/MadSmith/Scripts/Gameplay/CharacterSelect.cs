using MadSmith.Scripts.Managers;
using UnityEngine;

namespace MadSmith.Scripts.Gameplay
{
    public class CharacterSelect : MonoBehaviour
    {
        public GameObject[] playerCharacters;
        private int selectedCharacter = 0;


        public void Setup()
        {
            playerCharacters[selectedCharacter].gameObject.SetActive(false);
            selectedCharacter = GameManager.Instance.characterIndex;
            playerCharacters[selectedCharacter].gameObject.SetActive(true);
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
