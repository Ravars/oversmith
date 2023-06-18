using Oversmith.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] playerCharacters;

    private int selectedCharacter = 0;

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
