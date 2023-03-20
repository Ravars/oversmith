using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferredSizeSetter : MonoBehaviour
{
    public bool usePreferredHeight = false;
    public bool usePreferredWidth = false;


    // Start is called before the first frame update
    void Start()
    {
        setSize();
    }

    public void setSize()
    {
        RectTransform rTransform = GetComponent<RectTransform>();
        TMPro.TextMeshProUGUI textMesh = GetComponent<TMPro.TextMeshProUGUI>();

        if(rTransform == null)
        {
            Debug.LogError("Objeto deve conter component RectTransform", this);
            return;
        }

        if (textMesh == null)
        {
            Debug.LogError("Objeto deve conter component Text Mesh Pro", this);
            return;
        }

        if (usePreferredWidth)
            rTransform.sizeDelta = new Vector2(textMesh.preferredWidth, rTransform.sizeDelta.y);

        if(usePreferredHeight)
            rTransform.sizeDelta = new Vector2(rTransform.sizeDelta.x, textMesh.preferredHeight);
    }
}
