function PaintShips(result) {
    var x = 0;
    if (typeof result === 'object') {
        x = result;
    }
    else {
        x = JSON.parse(result);
    }
    
    const playertable = document.getElementById("ftable");
    const enemytable = document.getElementById("stable");
    for (i = 0; i < 10; i++) {
        for (j = 0; j < 10; j++) {
            const playerCell = document.getElementById(i * 10 + j);
            const enemyCell = document.getElementById( -(i * 10 + j)-1);

            if (x.Condition.GameState == 2)
            {
                enemyCell.style.backgroundColor = "Black"
            }
            else
            {
                if (x.EnemyTable.CellsVisibility[i][j] == 0) { enemyCell.style.backgroundColor = "FloralWhite" }
                else {
                    if (x.EnemyTable.Cells[i][j] == 1) { enemyCell.style.backgroundColor = "ForestGreen" }
                    else { enemyCell.style.backgroundColor = "black" }


                }

                
            }

            if (x.Condition.GameState == 3)
            {
                playerCell.style.backgroundColor = "Black"
            }
            else
            {
                if (x.PlayerTable.Cells[i][j] == 1 && x.PlayerTable.CellsVisibility[i][j] == 0) { playerCell.style.backgroundColor = "Chartreuse" }
                if (x.PlayerTable.Cells[i][j] == 1 && x.PlayerTable.CellsVisibility[i][j] == 1) { playerCell.style.backgroundColor = "ForestGreen" }
                if (x.PlayerTable.Cells[i][j] == 0 && x.PlayerTable.CellsVisibility[i][j] == 0) { playerCell.style.backgroundColor = "FloralWhite" }
                if (x.PlayerTable.Cells[i][j] == 0 && x.PlayerTable.CellsVisibility[i][j] == 1) { playerCell.style.backgroundColor = "DimGray" }
            }
        }
    }

}



