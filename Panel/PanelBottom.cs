using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBottom : MonoBehaviour {

	[SerializeField]
	private PanelFooter m_PanelFooter;

	public PanelFooter panel_footer { get { return m_PanelFooter; } }
}
