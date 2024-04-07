using System.Collections.Generic;
using Gameplay.Items;
using Gameplay.Items.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    internal class TwisterSolutionDisplayer : MonoBehaviour
    {
        public float imageSize;
        public float padding;
        public Sprite playerIcon;
        public Sprite goalIcon;

        internal void DisplaySolution(TwisterLevel twisterLevel)
        {
            KillAllChildrenImmediate();

            GameObject playerDisplay = new();
            playerDisplay.AddComponent<Image>();
            playerDisplay.GetComponent<Image>().sprite = playerIcon;
            playerDisplay.transform.SetParent(transform);
            playerDisplay.GetComponent<RectTransform>().sizeDelta = new(imageSize, imageSize);
            playerDisplay.transform.localPosition = new(0,0,0);

            float w = GetComponent<RectTransform>().rect.width;
            float h = GetComponent<RectTransform>().rect.height;
            
            w -= imageSize;
            h -= imageSize;

            w -= padding * 2;
            h -= padding * 2;

            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;
            for (int i = 0; i < twisterLevel.transform.childCount; i++)
            {
                Vector3 position =  twisterLevel.transform.GetChild(i).transform.localPosition;
                if(position.x < minX) minX = position.x;
                if(position.x > maxX) maxX = position.x;
                if(position.z < minY) minY = position.z;
                if(position.z > maxY) maxY = position.z;
                
                if(minX == maxX) maxX -= 0.1f;
                if(minY == maxY) maxY -= 0.1f;
            }

            float xPosScale = w / (2 * Mathf.Max(Mathf.Abs(maxX), Mathf.Abs(minX)));
            float yPosScale = h / (2 * Mathf.Max(Mathf.Abs(maxY), Mathf.Abs(minY)));

            for (int i = 0; i < twisterLevel.transform.childCount; i++)
            {
                TwisterButton button = twisterLevel.transform.GetChild(i).GetComponent<TwisterButton>();
                if(!button) continue;
                button.SetProperties();
                GameObject buttonDisplay = new();
                buttonDisplay.AddComponent<Image>();
                buttonDisplay.GetComponent<Image>().sprite = button.GetProperty<ShapePropertySO>().representation2D;
                buttonDisplay.GetComponent<Image>().color = button.GetProperty<ColorPropertySO>().matchingColor;
                if(button == twisterLevel.goal) {
                    GameObject victoryDisplay = new();
                    victoryDisplay.AddComponent<Image>();
                    victoryDisplay.GetComponent<Image>().sprite = goalIcon;
                    victoryDisplay.transform.SetParent(transform);
                    victoryDisplay.transform.localPosition = new(button.transform.localPosition.x * xPosScale, button.transform.localPosition.z * yPosScale, 0);
                    victoryDisplay.GetComponent<RectTransform>().sizeDelta = new(imageSize + padding, imageSize + padding);
                    buttonDisplay.transform.SetParent(victoryDisplay.transform);
                    buttonDisplay.transform.localPosition = new(0,0,0);
                }
                else {
                    buttonDisplay.transform.SetParent(transform);
                    buttonDisplay.transform.localPosition = new(button.transform.localPosition.x * xPosScale, button.transform.localPosition.z * yPosScale, 0);
                }
                buttonDisplay.GetComponent<RectTransform>().sizeDelta = new(imageSize, imageSize);
            }
        }

        public void KillAllChildrenImmediate() {
            List<Transform> children = new();
            for(int i = 0; i < transform.childCount; i++) {
                children.Add(transform.GetChild(i));
            }
            foreach (Transform item in children)
            {
                GameObject.DestroyImmediate(item.gameObject);
            }
        }
    }
}