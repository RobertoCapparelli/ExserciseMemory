using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class MemoryScript : MonoBehaviour
{
    #region CardVariables

    private VisualElement firstCard = null;
    private int numberCoupleCard = 0;

    private Color[] cardColors = new Color[3]
    {
        Color.red,
        Color.green,
        Color.blue,

    };
    #endregion
   
    private void Start()  //Get the cards in the game and give them a color, after add the callback for the click
    {
        List<Color> colorsCards = CreateNewListColors();

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        List<VisualElement> children = root.Query("card").ToList();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].userData = colorsCards[i];
            children[i].style.backgroundColor = Color.black;
        }

        root.RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    #region ColorMethods
    private List<Color> CreateNewListColors()  //get a suffled list of color for the cards
    {
        List<Color> colors = new List<Color>();

        foreach (Color color in cardColors)
        {
            colors.Add(color);
            colors.Add(color);
        }

        ShuffleColors(colors);
        
        return colors;
    }
    void ShuffleColors(List<Color> colors)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            int randomIndex = Random.Range(i, colors.Count);
            Color temp = colors[i];
            colors[i] = colors[randomIndex];
            colors[randomIndex] = temp;
        }
    }
    #endregion

    #region CheckCardMethod

    private void OnMouseDown(MouseDownEvent e) //Core Game 
    {
        
        VisualElement element = (VisualElement)e.target;
               
        if (element.name == "card") //Check for click in the background
        {
            if (element.style.backgroundColor != Color.black) //Check to avoid to click a card flipped
            {
                return;
            }

            if (firstCard == null) //if is the first card flipped
            {
                firstCard = element;
                element.style.backgroundColor = (Color)element.userData;
            }
            else //if is the second one
            {
                if ((Color)element.userData == (Color)firstCard.userData) //Find check
                {
                    element.style.backgroundColor = (Color)element.userData;
                    Debug.Log("Find check!!");
                    numberCoupleCard++;
                    if (numberCoupleCard == 3) { Debug.Log("You Win!"); }
                }
                else //Don't find a check 
                {
                    element.style.backgroundColor = (Color)element.userData;
                    StartCoroutine(FlipCards(firstCard,element));                  
                }
                firstCard = null;
            }
        }
    }
    IEnumerator FlipCards(VisualElement card1, VisualElement card2)
    {
        yield return new WaitForSeconds(0.5f);
        card1.style.backgroundColor = Color.black;
        card2.style.backgroundColor = Color.black;
    }
    #endregion
}
