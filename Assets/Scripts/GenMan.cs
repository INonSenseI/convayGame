using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMan : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    [SerializeField] UnityEngine.UI.Slider randomizerSlider;

    [SerializeField] GameObject startScreen;
    [SerializeField] UnityEngine.UI.InputField inputX;
    [SerializeField] UnityEngine.UI.InputField inputY;

    int[] gridSize = new int[2];
    Cell[][] cellMatrix;
    Coroutine runCoroutine;

    void Start()
    {
    }

    void CreateGrid()
    {
        cellMatrix = new Cell[(int)gridSize[0]][]; //Vytvoøím první èást arraye

        float cellOffset = cellPrefab.gameObject.transform.localScale.x / 7;

        for (int col = 0; col < gridSize[0]; col++)
        {
            float colOffset = col - (int)gridSize[0] / 2; //Offset indexu aby byl grid na støedu obrazovky
            cellMatrix[col] = new Cell[(int)gridSize[1]]; //Vytvoøím druhou èást arraye

            for (int row = 0; row < gridSize[1]; row++)
            {
                float rowOffset = row - (int)gridSize[1] / 2; //Offset indexu aby byl grid na støedu obrazovky
                cellMatrix[col][row] = Instantiate(cellPrefab, new Vector3(colOffset * cellOffset, rowOffset * cellOffset, 0), Quaternion.identity).GetComponent<Cell>();
            }
        }
    }

    public void StartButtonClick()
    {
        try
        {
            int valueX = int.Parse(inputX.text);
            int valueY = int.Parse(inputY.text);

            gridSize[0] = valueX;
            gridSize[1] = valueY;
        }

        catch
        {
            return;
        }

        CreateGrid();
        Destroy(startScreen);
    }

    public void NextCycle()
    {
        CalculateFutureStates();
        SwtichCellStates();
    }

    public void RunCycles()
    {
        if (runCoroutine == null)
        {
            runCoroutine = StartCoroutine(Run());
        }
        else
        {
            StopCoroutine(runCoroutine);
            runCoroutine = null;
        }
    }

    public void KillBoard()
    {
        foreach (Cell[] cellArray in cellMatrix)
        {
            foreach (Cell cell in cellArray)
            {
                cell.Alive = false;
                cell.WillBeAlive = false;
            } 
        }
    }

    public void RandomizeBoardState()
    {
        foreach (Cell[] cellArray in cellMatrix)
        {
            foreach (Cell cell in cellArray)
            {
                float randNumber = Random.Range(0f, 1f);
                cell.Alive = randNumber < randomizerSlider.value;
            }
        }
    }

    void CalculateFutureStates()
    {

        for (int col = 0; col < gridSize[0]; col++)
        {
            for (int row = 0; row < gridSize[1]; row++)
            {
                cellMatrix[col][row].WillBeAlive = CheckIfShouldBeAlive(col, row);
            }
        }
    }

    bool CheckIfShouldBeAlive(int col, int row)
    {
        int aliveNeighbours = 0;

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0) continue;

                try
                {
                    if (cellMatrix[col + x][row + y].Alive)
                    {
                        aliveNeighbours++;
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        if (cellMatrix[col][row].Alive)
        {
            if (aliveNeighbours < 2 || aliveNeighbours > 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (aliveNeighbours == 3)
            {
                return true;
            }
        }

        return false;
    }

    void SwtichCellStates()
    {
        foreach (Cell[] cellArray in cellMatrix)
        {
            foreach (Cell cell in cellArray)
            {
                cell.Alive = cell.WillBeAlive;
            }
        }
    }

    IEnumerator Run()
    {
        while(true)
        {
            NextCycle();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
//TODO: vypnout WillBeActive a Active ve vhodný moment
