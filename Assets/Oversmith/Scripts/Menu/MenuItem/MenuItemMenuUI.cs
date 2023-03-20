using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Oversmith.Scripts.Menu
{
    public class MenuItemMenuUI : MonoBehaviour
    {
        [System.Serializable]
        public struct OptionItemUI
        {
            public string name;
            public Sprite image;
            public string code;
        }

        [Header("Property Title")]
        public string propertyName = "Property";
        public TMPro.TextMeshProUGUI textMesh;
        [Space(10)]
        [Header("Property Options")]
        public OptionItemUI[] options;
        public GameObject optionsParent;
        private GameObject[] selectors;
        private int selectorIndex;
        public string Code { get; private set; }

        public GameObject selectorPrefab;

        private Image image;
        private Color selectColor;
        private Color deselectColor;

        void Awake()
        {
            selectors = new GameObject[options.Length];
            for(int i = 0; i < options.Length; i++)
            {
                selectors[i] = Instantiate(selectorPrefab, optionsParent.transform);
                selectors[i].GetComponent<ItemSelectorMenuUI>().text = options[i].name;
                selectors[i].GetComponent<ItemSelectorMenuUI>().icon = options[i].image;
                selectors[i].GetComponent<RectTransform>().sizeDelta = new Vector2(-140, 0);
                if (i == 0) // Condição de opção padrão => a configuração já pré escolhida.
                {
                    selectors[i].SetActive(true);
                    selectorIndex = i;
                    Code = options[i].code;
                }
                else
                    selectors[i].SetActive(false);
            }

            image = GetComponent<Image>();
            deselectColor = new Color(0.8705882f, 1f, 1f, 0.2705882f);
            selectColor = new Color(0.852362f, 0.8867924f, 0.1798683f, 1f);
        }

        public void onSelect()
        {
            image.color = selectColor;
        }

        public void onDeselect()
        {
            image.color = deselectColor;
        }

        public void onSwitchLeft()
        {
            selectors[selectorIndex].SetActive(false);
            selectorIndex--;
            if (selectorIndex < 0) selectorIndex = selectors.Length - 1;
            selectors[selectorIndex].SetActive(true);
            Code = options[selectorIndex].code;
        }

        public void onSwitchRight()
        {
            selectors[selectorIndex].SetActive(false);
            selectorIndex++;
            if (selectorIndex >= selectors.Length) selectorIndex = 0;
            selectors[selectorIndex].SetActive(true);
            Code = options[selectorIndex].code;
        }
    }
}