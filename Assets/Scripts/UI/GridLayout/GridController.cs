using System.Collections.Generic;
using Model;
using UnityEngine;

namespace UI.GridLayout
{
    public class GridController : MonoBehaviour
    {
        public GameObject grid;
        public GameObject cellPrefab;

        public void SetGridCells(List<LeaderboardEntry> leaderboardEntries)
        {
            RemoveGridCells();
        
            foreach (var leaderboardEntry in leaderboardEntries)
            {
                GameObject cell = Instantiate(cellPrefab, grid.transform);
                cell.GetComponent<CellController>().SetCellText(leaderboardEntry);
            }
        }
    
        private void RemoveGridCells()
        {
            foreach (Transform child in grid.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
