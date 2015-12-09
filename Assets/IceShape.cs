using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceShape : MonoBehaviour
{

    public GameObject iceBlock;
    public float blockSpacing = 4;
	public Dictionary<GameObject, Vector2> blockPositions = new Dictionary<GameObject, Vector2>();
    public float slideSpeed;

    private int blocktype = -1;
    private int column = -1;
    private bool lockedIn = false;
	private Tower myTower;

    void OnCollisionEnter2D(Collision2D collision)
    {
        lockedIn = true;

    }

    public void SetColumn(int newColumn)
    {
        column = newColumn;
    }

    void Update()
    {
        if (!lockedIn)
            gameObject.transform.position = gameObject.transform.position - new Vector3(0, Time.deltaTime * slideSpeed, 0);
    }

	public void InitializeIceShape (bool onLeftEdge, bool onRightEdge, Tower myNewTower)
    {
        if (onLeftEdge && onRightEdge)
            throw new UnityException("you can't be on both the left and right edge");
		myTower = myNewTower;
        Vector2[] randomBlockPositions = RandomBlockPositions(onLeftEdge, onRightEdge);
        InstantiateBlocks(randomBlockPositions);
	}

    private Vector2[] RandomBlockPositions(bool onLeftEdge, bool onRightEdge)
    {
		Vector2[] randomBlockPositions = new Vector2[3];

        blocktype = Random.Range(0, 6);
        if (onLeftEdge) //3, 4, 5 are ok for left edge
            blocktype = Random.Range(3, 6);
        if (onRightEdge) //1, 2, 3 are ok for right edge
            blocktype = Random.Range(1, 4);

        randomBlockPositions[0] = new Vector2(0, 0);

		//these are the random shapes, just different L and I shapes
        switch (blocktype)
        {
            case 0:
				randomBlockPositions[1] = new Vector2(1, 0);
				randomBlockPositions[2] = new Vector2(-1, 0);
                break;
            case 1:
				randomBlockPositions[1] = new Vector2(0, 1);
				randomBlockPositions[2] = new Vector2(-1, 0);
                break;
            case 2:
				randomBlockPositions[1] = new Vector2(0, -1);
				randomBlockPositions[2] = new Vector2(-1, 0);
                break;
            case 3:
				randomBlockPositions[1] = new Vector2(0, -1);
				randomBlockPositions[2] = new Vector2(0, 1);
                break;
            case 4:
				randomBlockPositions[1] = new Vector2(0, -1);
				randomBlockPositions[2] = new Vector2(1, 0);
                break;
            case 5:
				randomBlockPositions[1] = new Vector2(0, 1);
				randomBlockPositions[2] = new Vector2(1, 0);
                break; 
            default:
                throw new UnityException("unknown blocktype");
        }

		return randomBlockPositions;
    }

    private void InstantiateBlocks(Vector2[] randomBlockPositions)
    {
        foreach(Vector2 blockPosition in randomBlockPositions)
        {
            GameObject newBlock = Instantiate(iceBlock) as GameObject;
            newBlock.transform.SetParent(gameObject.transform);
            newBlock.transform.localPosition = new Vector3(blockPosition.x * blockSpacing, blockPosition.y * blockSpacing, 0);
			blockPositions.Add(newBlock, blockPosition);
        }

    }

	public void LockBlocks(int blockRow, GameObject triggerBlock)
	{
		lockedIn = true;
		for(int i = 0; i < transform.childCount; i++)
		{
			GameObject block = transform.GetChild(i).gameObject;
			Vector2 blockPosition = blockPositions[block];
			Vector2 triggerBlockPosition = blockPositions[triggerBlock];
			int lockColumn = column + (int)blockPosition.x;
			int lockRow = blockRow + (int)blockPosition.y - (int)triggerBlockPosition.y;
			block.GetComponent<IceBlock>().LockIn(lockColumn, 
			                                         lockRow);
			myTower.AddBlock(block, lockColumn, lockRow);
		}
	}

	public void Melt()
	{
		Destroy (gameObject);
	}

}
