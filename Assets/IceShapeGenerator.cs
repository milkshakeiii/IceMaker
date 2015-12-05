using UnityEngine;
using System.Collections;

public class IceShapeGenerator : MonoBehaviour
{
    public GameObject iceShape;
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
        System.Collections.Generic.List<int> okColumns = new System.Collections.Generic.List<int>();
        for (int i = 0; i < columnCount; i++)
            okColumns.Add(i);

        for (int i = lastColumn - 1; i <= lastColumn + 1; i++)
            if (okColumns.Contains(i))
                okColumns.Remove(i);
        for (int i = twoColumnsAgo - 1; i <= twoColumnsAgo + 1; i++)
            if (okColumns.Contains(i))
                okColumns.Remove(i);


        //if (okColumns.Count == 0)
        //{
        //    column = -2;
        //    twoColumnsAgo = lastColumn;
        //    lastColumn = column;
        //    Debug.Log("not enuogh room");
        //    return;
        //}


        column = okColumns[Random.Range(0, okColumns.Count)];
        twoColumnsAgo = lastColumn;
        lastColumn = column;


        if (column == 0)
            newIceShape.InitializeIceShape(true, false);
        else if (column == columnCount - 1)
            newIceShape.InitializeIceShape(false, true);
        else
            newIceShape.InitializeIceShape(false, false);

        newIceShape.SetColumn(column);

        newShape.transform.position = gameObject.transform.position + new Vector3(column * spacing, 0, 0);
    }
}
