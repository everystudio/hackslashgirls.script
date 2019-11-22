using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelFooter : MonoBehaviour {

	[SerializeField]
	private List<BtnFooter> btn_footer_list = new List<BtnFooter>();

	public UnityEventString HandleFooterButton = new UnityEventString();

	void Start()
	{
		foreach( BtnFooter btn in btn_footer_list)
		{
			btn.btn.onClick.AddListener(() =>
			{
				HandleFooterButton.Invoke(btn.GetFooterName());
			});
		}
	}
	
}
