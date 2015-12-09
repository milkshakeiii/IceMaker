using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour 
{
	public int towerWidth = 9;
	public int towerHeight = 20;

	private GameObject[,] towerBlocks;
	
	void Start () 
	{
		towerBlocks = new GameObject[towerWidth, towerHeight];
	}

	public void AddBlock(GameObject block, int column, int row)
	{
		if (row >= towerHeight)
		{
			GameOver ();
			return;
		}
		towerBlocks[column, row] = block;
		CheckForClears ();
	}

	private void CheckForClears()
	{
		for (int i = 0; i < towerHeight; i++)
		{
			bool rowFull = true;
			for (int j = 0; j < towerWidth; j++)
			{
				if (towerBlocks[j, i] == null)
				{
					rowFull = false;
				}
			}
			if (rowFull)
			{
				DoRowClear(i);
				CheckForClears();
				return;
			}
		}
	}

	private void DoRowClear(int row)
	{
		//clear that row
		for (int i = 0; i < towerWidth; i++)
		{
			Destroy(towerBlocks[i, row]);
			towerBlocks[i, row] = null;
		}

		//move the rest down
		for (int i = row; i < towerHeight; i++)
		{
			for (int j = 0; j < towerWidth; j++)
			{
				if (towerBlocks[j, i] != null)
				{
					towerBlocks[j, i].GetComponent<IceBlock>().LockIn(j, i-1);
					towerBlocks[j, i-1] = towerBlocks[j, i];
					towerBlocks[j, i] = null;
				}
			}
		}

		Debug.Log ("row " + row.ToString () + " clear");
	}

	private void GameOver()
	{
		Debug.Log ("game over :(");
		Application.LoadLevel (Application.loadedLevel);
	}
}
