using UnityEngine;
using System.Collections;

public class IceShapeGenerator : MonoBehaviour
{
    public GameObject iceShape;
	public Tower tower;
    public float frequency = 4;
    public float spacing = 4;
    public int columnCount = 8;

    private float lastGeneration;
    private int lastColumn = -2;
    private int twoColumnsAgo = -2;

   

	void Update ()
    {
	    if (Time.time - lastGeneration > frequency)
        {
            DoGeneration();
            lastGeneration = Time.time;
        }
	}
    
    private void DoGeneration()
    {
        GameObject newShape = Instantiate(iceShape) as GameObject;
        IceShape newIceShape = newShape.GetComponent<IceShape>();

        int column = -1;
		//the list of all columns for which it is OK to generate a shape centered in that column
        System.Collections.Generic.List<int> okColumns = new System.Collections.Generic.List<int>();
        
		//start with all possible columns
		for (int i = 0; i < columnCount; i++)
            okColumns.Add(i);

		//remove everything surrounding the previous and two previous shapes
        for (int i = lastColumn - 1; i <= lastColumn + 1; i++)
            if (okColumns.Contains(i))
                okColumns.Remove(i);
        for (int i = twoColumnsAgo - 1; i <= twoColumnsAgo + 1; i++)
            if (okColumns.Contains(i))
                okColumns.Remove(i);

		//we shouldn't need this since we should always have enough room
		//because there are 9 columns and we only eliminate 6 max
        //if (okColumns.Count == 0)
        //{
        //    column = -2;
        //    twoColumnsAgo = lastColumn;
        //    lastColumn = column;
        //    Debug.Log("not enuogh room");
        //    return;
        //}

		//randomly choose an ok column to make the shape in
        column = okColumns[Random.Range(0, okColumns.Count)];

		//update record of previous chosen columns
        twoColumnsAgo = lastColumn;
        lastColumn = column;

		//make the shape, and tell its initializer whether or not it is on edges
		//because edges cannot have all shapes
        if (column == 0)
            newIceShape.RandomizeIceShape(true, false);
        else if (column == columnCount - 1)
            newIceShape.RandomizeIceShape(false, true);
        else
            newIceShape.RandomizeIceShape(false, false);

		newIceShape.SetTower (tower);
        newIceShape.SetColumn(column);

        newShape.transform.position = gameObject.transform.position + new Vector3(column * spacing, 0, 0);
    }
}
