using UnityEngine;
using UnityEngine.UI;

public class TabPanel : MonoBehaviour
{
	[SerializeField] Color normalButtonColor;
	[SerializeField] Color selectedButtonColor;
	[SerializeField] Button[] buttons;
	[SerializeField] GameObject[] Tabs;

    void Start()
    {
		for (int i = 0; i < Tabs.Length; i++)
		{
			if (Tabs[i].activeSelf)
			{
				SetButtonColor(i, selectedButtonColor);
			}
		}
	}

	public void SelectTab(int index)
	{
        for (int i = 0; i < Tabs.Length; i++) {
            if (i == index)
            {
                Tabs[i].SetActive(true);
				SetButtonColor(i, selectedButtonColor);
			}
            else
			{
				Tabs[i].SetActive(false);
				SetButtonColor(i, normalButtonColor);
			}
		}
		
	}

	private void SetButtonColor(int i, Color color)
	{
		ColorBlock colors = buttons[i].colors;
		colors.normalColor = color;
		buttons[i].colors = colors;
	}
}
