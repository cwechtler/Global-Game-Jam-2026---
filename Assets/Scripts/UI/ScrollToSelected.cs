using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollToSelected : MonoBehaviour
{
	private RectTransform scrollRectTransform;
	private RectTransform contentPanel;
	private RectTransform selectedRectTransform;
	private ScrollRect scrollRect;

	private bool hasForcedSelection = false;

	private void Start()
	{
		scrollRectTransform = GetComponent<RectTransform>();
		scrollRect = GetComponent<ScrollRect>();
		contentPanel = scrollRect.content;
	}

	private void Update()
	{
		GameObject selected = EventSystem.current.currentSelectedGameObject;
		if (selected == null) { return; }

		selected = SelectFirstScore(selected);
	}

	private GameObject SelectFirstScore(GameObject selected)
	{
		if (selected.transform.IsChildOf(contentPanel)) {
			if (!hasForcedSelection) {
				if (scrollRect.content.childCount > 0) {
					GameObject firstItem = scrollRect.content.GetChild(0).gameObject;
					if (selected != firstItem) {
						EventSystem.current.SetSelectedGameObject(firstItem);
						selected = firstItem;
					}
				}
				hasForcedSelection = true;
			}
			KeepSelectedItemInView(selected);
		}
		else if (hasForcedSelection == true) {
			hasForcedSelection = false;
		}

		return selected;
	}

	private void KeepSelectedItemInView(GameObject selected)
	{
		selectedRectTransform = selected.GetComponent<RectTransform>();

		// The position of the selected UI element is the absolute anchor position,
		// ie. the local position within the scroll rect + its height if we're
		// scrolling down. If we're scrolling up it's just the absolute anchor position.
		float selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;

		// The upper bound of the scroll view is the anchor position of the content we're scrolling.
		float scrollViewMinY = contentPanel.anchoredPosition.y;

		// The lower bound is the anchor position + the height of the scroll rect.
		float scrollViewMaxY = contentPanel.anchoredPosition.y + scrollRectTransform.rect.height;

		// If the selected position is below the current lower bound of the scroll view we scroll down.
		if (selectedPositionY > scrollViewMaxY) {
			float newY = selectedPositionY - scrollRectTransform.rect.height - (selectedRectTransform.rect.height / 2);
			contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, (newY));
		}
		// If the selected position is above the current upper bound of the scroll view we scroll up.
		else if (Mathf.Abs(selectedRectTransform.anchoredPosition.y) < scrollViewMinY) {
			contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y) - selectedRectTransform.rect.height / 2);
		}
	}
}
