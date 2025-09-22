using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridLayouts : MonoBehaviour
{
    public static HexGridLayouts instance;
    [Header("網格設定")]
    public Vector2Int gridSize;

    [Header("瓦片設定")]
    public float outerSize = 1f;
    public float innerSize = 0f;
    public float height = 1f;
    public bool isFlatTopped;
    public Material baseMaterial;
    public Color baseColor;
    public Color highColor;

    [Header("清單")]
    public List<HexRenderer> cellList;
    public List<HexRenderer> startList;
    public List<HexRenderer> endList;
    public List<HexRenderer> pathList;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        cellList = new List<HexRenderer>();
        startList = new List<HexRenderer>();
        endList = new List<HexRenderer>();
        pathList = new List<HexRenderer>();
    }

    private void OnEnable()
    {
        layoutGrid();
    }

    public void caculatePath(HexRenderer from, HexRenderer to) 
    {
        clearPath();
        startList.Add(from);
        while (startList.Count > 0) 
        {
            var workCell = startList[0];
            if (workCell == to) 
            {
                getPath(to);
                for(int i = 0; i < pathList.Count; i++) 
                {
                    pathList[i].setColor(highColor);
                }
                pathList.Reverse();
                pathList.RemoveAt(0);
                break;
            }
            else 
            {
                startList.RemoveAt(0);
                endList.Add(workCell);
                var neighboursList = findNeighbours(workCell);
                for (int i = 0; i < neighboursList.Count; i++) 
                {
                    if (endList.Contains(neighboursList[i]) || neighboursList[i].isObstacle) 
                    {
                        continue;
                    }

                    if (!startList.Contains(neighboursList[i])) 
                    {
                        int g = workCell.g + 1;
                        int h = caculateH(neighboursList[i], to);
                        int f = g + h;
                        neighboursList[i].parent = workCell;
                        if (startList.Count == 0) 
                        {
                            startList.Add(neighboursList[i]);
                        }
                        else
                        {
                            if (f < startList[0].f) 
                            {
                                startList.Insert(0, neighboursList[i]);
                            }
                            else 
                            {
                                startList.Add(neighboursList[i]);
                            }
                        }
                    }
                    else 
                    {
                        int g = workCell.g + 1;
                        int h = caculateH(neighboursList[i], to);
                        int f = g + h;
                        if (f < neighboursList[i].f) 
                        {
                            neighboursList[i].f = f;
                            neighboursList[i].g = g;
                            neighboursList[i].parent = workCell;
                        }
                    }
                }
            }
        }
    }

    private void getPath(HexRenderer to) 
    {
        pathList.Add(to);
        if (to.parent != null) 
        {
            getPath(to.parent);
        }
    }

    private int caculateH(HexRenderer from,HexRenderer to) 
    {
        int fromIndex = cellList.IndexOf(from);
        int toIndex = cellList.IndexOf(to);
        int fromRow = fromIndex / gridSize.x + 1;
        int toRow = toIndex / gridSize.x + 1;
        int fromCol = fromIndex % gridSize.x + 1;
        int toCol = toIndex % gridSize.x + 1;

        int distance = Mathf.Abs(fromRow - toRow) + Mathf.Abs(fromCol - toCol);

        return distance;
    }

    private List<HexRenderer> findNeighbours(HexRenderer cell)
    {
        List<HexRenderer> neighboursList = new List<HexRenderer>();
        var index = cellList.IndexOf(cell);
        if (index == 0)
        {
            neighboursList.Add(cellList[index + 1]);
            neighboursList.Add(cellList[index + gridSize.x]);
            neighboursList.Add(cellList[index + gridSize.x + 1]);
        }
        else if (index == gridSize.x - 1)
        {
            neighboursList.Add(cellList[index - 1]);
            neighboursList.Add(cellList[index + gridSize.x]);
            neighboursList.Add(cellList[index + gridSize.x + 1]);
        }
        else if (index == cellList.Count - 1)
        {
            neighboursList.Add(cellList[index - 1]);
            neighboursList.Add(cellList[index - gridSize.x]);
            neighboursList.Add(cellList[index - gridSize.x - 1]);
        }
        else if (index == cellList.Count - gridSize.x)
        {
            neighboursList.Add(cellList[index + 1]);
            neighboursList.Add(cellList[index - gridSize.x]);
            neighboursList.Add(cellList[index - gridSize.x - 1]);
        }
        else if (index / gridSize.x == 0)
        {
            neighboursList.Add(cellList[index + 1]);
            neighboursList.Add(cellList[index - 1]);
            neighboursList.Add(cellList[index + gridSize.x]);
            neighboursList.Add(cellList[index + gridSize.x + 1]);
        }
        else if (index / gridSize.x == gridSize.y - 1)
        {
            neighboursList.Add(cellList[index + 1]);
            neighboursList.Add(cellList[index - 1]);
            neighboursList.Add(cellList[index - gridSize.x]);
            neighboursList.Add(cellList[index - gridSize.x - 1]);
        }
        else if (index % gridSize.x == 0)
        {
            if (index % (2 * gridSize.x) == 0)
            {
                neighboursList.Add(cellList[index + 1]);
                neighboursList.Add(cellList[index + gridSize.x]);
                neighboursList.Add(cellList[index + gridSize.x + 1]);
                neighboursList.Add(cellList[index - gridSize.x]);
                neighboursList.Add(cellList[index - gridSize.x + 1]);
            }
            else
            {
                neighboursList.Add(cellList[index + 1]);
                neighboursList.Add(cellList[index + gridSize.x]);
                neighboursList.Add(cellList[index - gridSize.x]);
            }
        }
        else if ((index + 1) % gridSize.x == 0) 
        {
            if ((index + 1) % (2 * gridSize.x) == 0)
            {
                neighboursList.Add(cellList[index - 1]);
                neighboursList.Add(cellList[index + gridSize.x]);
                neighboursList.Add(cellList[index + gridSize.x - 1]);
                neighboursList.Add(cellList[index - gridSize.x]);
                neighboursList.Add(cellList[index - gridSize.x - 1]);
            }
            else
            {
                neighboursList.Add(cellList[index - 1]);
                neighboursList.Add(cellList[index + gridSize.x]);
                neighboursList.Add(cellList[index - gridSize.x]);
            }
        }
        else 
        {
            if ((index / gridSize.x) % 2 == 1) 
            {
                neighboursList.Add(cellList[index + 1]);
                neighboursList.Add(cellList[index - 1]);
                neighboursList.Add(cellList[index + gridSize.x]);
                neighboursList.Add(cellList[index + gridSize.x - 1]);
                neighboursList.Add(cellList[index - gridSize.x]);
                neighboursList.Add(cellList[index - gridSize.x - 1]);
            }
            else
            {
                neighboursList.Add(cellList[index + 1]);
                neighboursList.Add(cellList[index - 1]);
                neighboursList.Add(cellList[index + gridSize.x]);
                neighboursList.Add(cellList[index + gridSize.x + 1]);
                neighboursList.Add(cellList[index - gridSize.x]);
                neighboursList.Add(cellList[index - gridSize.x + 1]);
            }
        }

        return neighboursList;
    }

    private void clearPath() 
    {
        for (int i = 0; i < cellList.Count; i++) 
        {
            cellList[i].parent = null;
            cellList[i].g = 0;
            cellList[i].f = 0;
            cellList[i].setColor(baseColor);
        }
        startList.Clear();
        endList.Clear();
        pathList.Clear();
    }

    private void layoutGrid() 
    {
        for (int y = 0; y < gridSize.y; y++) 
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex{x},{y}", typeof(HexRenderer));
                tile.transform.position = getPositionForHexFromCoordinate(new Vector2Int(x,y));

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.isFlatTopped = isFlatTopped;
                hexRenderer.outerSize = outerSize;
                hexRenderer.innerSize = innerSize;
                hexRenderer.height = height;
                hexRenderer.setMaterial(new Material(baseMaterial));
                hexRenderer.drawMesh();

                tile.transform.SetParent(transform, true);
                cellList.Add(hexRenderer);
            }
        }
    }

    public Vector3 getPositionForHexFromCoordinate(Vector2Int coordinate) 
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = outerSize;

        if (!isFlatTopped) 
        {
            shouldOffset = row % 2 == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = (shouldOffset) ? width / 2 : 0;
            xPosition = (column * horizontalDistance) + offset;
            yPosition = (row * verticalDistance);
        }
        else 
        {
            shouldOffset = column % 2 == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3) * size;

            horizontalDistance = height * (3f / 4f);
            verticalDistance = width;

            offset = (shouldOffset) ? height / 2 : 0;
            xPosition = (column * horizontalDistance);
            yPosition = (row * verticalDistance) - offset;
        }

        return new Vector3(xPosition, 0, -yPosition);
    }
}
