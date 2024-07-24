var shipwindow = document.getElementById("shipWindow")


$.ajax({
    type: "GET",
    url: 'https://localhost:7031/Home/GetGame',
    dataType: "json",
    success: function (result) {

        DrawAllShips(result)
        WriteErrors(result)
        PaintShips(result)
        return result
    },
    error: function (error) {
        console.error('Error:', error);
    }
});

for (var i = 0; i < 10; i++)
{
    var table1 = document.getElementById('ftable');
    var rowrow = document.createElement('tr')

    for (j = 0; j < 10; j++)
    {
        var cell1 = document.createElement('td');
        cell1.classList.add('myblock');
        cell1.setAttribute("onclick", "PlaceShip(event)");
        cell1.setAttribute("id", i * 10 + j);
        rowrow.appendChild(cell1);

    }
    table1.appendChild(rowrow);
}

for (var i = 0; i < 10; i++)
{
    var table1 = document.getElementById('stable');
    var rowrow = document.createElement('tr')

    for (j = 0; j < 10; j++)
    {
        var cell1 = document.createElement('td');
        cell1.classList.add('enemyblock');
        cell1.setAttribute("onclick", "Shoot(event)");
        cell1.setAttribute("id", -(i * 10 + j)-1);
        rowrow.appendChild(cell1);

    }
    table1.appendChild(rowrow);
}


var shipdraw = document.createElement("table");
//positionship.setAttribute("id", "textposition");

