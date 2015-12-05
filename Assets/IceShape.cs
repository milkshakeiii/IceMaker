using UnityEngine;
using System.Collections;

public class IceShape : MonoBehaviour
{

    public GameObject iceBlock;
    public float blockSpacing = 4;
    public Vector2[] blockPositions = new Vector2[3];
    public float slideSpeed;

    private int blocktype = -1;
    private int column = -1;
    private bool lockedIn = false;

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

	public void InitializeIceShape (bool onLeftEdge, bool onRightEdge)
    {
        if (onLeftEdge && onRightEdge)
            throw new UnityException("you can't be on both the left and right edge");
        RandomizeBlockPositions(onLeftEdge, onRightEdge);
        InstantiateBlocks();
	}

    private void RandomizeBlockPositions(bool onLeftEdge, bool onRightEdge)
    {
        blocktype = Random.Range(0, 6);
        if (onLeftEdge) //3, 4, 5 are ok for left edge
            blocktype = Random.Range(3, 6);
        if (onRightEdge) //1, 2, 3 are ok for right edge
            blocktype = Random.Range(1, 4);

        blockPositions[0] = new Vector2(0, 0);

        switch (blocktype)
        {
            case 0:
                blockPositions[1] = new Vector2(1, 0);
                blockPositions[2] = new Vector2(-1, 0);
                break;
            case 1:
                blockPositions[1] = new Vector2(0, 1);
                blockPositions[2] = new Vector2(-1, 0);
                break;
            case 2:
                blockPositions[1] = new Vector2(0, -1);
                blockPositions[2] = new Vector2(-1, 0);
                break;
            case 3:
                blockPositions[1] = new Vector2(0, -1);
                blockPositions[2] = new Vector2(0, 1);
                break;
            case 4:
                blockPositions[1] = new Vector2(0, -1);
                blockPositions[2] = new Vector2(1, 0);
                break;
            case 5:
                blockPositions[1] = new Vector2(0, 1);
                blockPositions[2] = new Vector2(1, 0);
                break; 
            default:
                throw new UnityException("unknown blocktype");
        }
    }

    private void InstantiateBlocks()
    {
        foreach(Vector2 blockPosition in blockPositions)
        {
            GameObject newBlock = Instantiate(iceBlock) as GameObject;
            newBlock.transform.SetParent(gameObject.transform);
            newBlock.transform.localPosition = new Vector3(blockPosition.x * blockSpacing, blockPosition.y * blockSpacing, 0);
        }

    }

}
