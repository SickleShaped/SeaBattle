function PaintShips(result) {
    const x = result
    //console.log(x)
    const playertable = document.getElementById("ftable");

    const enemytable = document.getElementById("stable");
    for (i = 0; i < 10; i++) {
        for (j = 0; j < 10; j++) {
            const playerCell = document.getElementById(i * 10 + j);
            //console.log(x.PlayerTable.Cells[i][j]);
            const enemyCell = document.getElementById(i * 10 + j + 100);
            if (x.PlayerTable.Cells[i][j] == 1) {playerCell.style.backgroundColor = "Red" }
            if (x.EnemyTable.Cells[i][j] == 1) { enemyCell.style.backgroundColor = "Chartreuse" }
            
        }
    }

}



