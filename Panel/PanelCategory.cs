using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCategory : MonoBehaviour {
	[SerializeField]
	private List<BtnCategory> btn_category_list = new List<BtnCategory>();

	public UnityEventInt HandleCategoryButton = new UnityEventInt();

	public int current_category;

	void Start()
	{
		foreach (BtnCategory btn in btn_category_list)
		{
			btn.btn.onClick.AddListener(() =>
			{
				if(current_category != btn.GetCategory())
				{
					SEControl.Instance.Play("Page 01");
				}
				current_category = btn.GetCategory();
				HandleCategoryButton.Invoke(btn.GetCategory());
			});
		}
	}

}
