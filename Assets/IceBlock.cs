using UnityEngine;
using System.Collections;

public class IceBlock : MonoBehaviour 
{

	public float spacing = 4f;
	public Vector3 towerBottomLeft;

	private int lockedRow = 0;
	private IceShape iceShape;

	void OnMouseDown()
	{
		iceShape.Melt ();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (gameObject.tag == "tower")
		{
			return;
		}
		int otherLockedRow = 0;
		if (other.gameObject.tag == "tower")
		{
			otherLockedRow = other.gameObject.GetComponent<IceBlock>().GetLockedRow();
		}
		else if (other.gameObject.tag != "ground")
		{
			return;
		}
		iceShape.LockBlocks(otherLockedRow + 1, gameObject);
	}

	public int GetLockedRow()
	{
		return lockedRow;
	}
	
	void Start () 
	{
		iceShape = gameObject.transform.parent.gameObject.GetComponent<IceShape> ();
	}

	public void LockIn(int column, int row)
	{
		lockedRow = row;
		gameObject.tag = "tower";
		gameObject.transform.position = towerBottomLeft + new Vector3 (column * spacing, row * spacing, 0f);
	}
}
