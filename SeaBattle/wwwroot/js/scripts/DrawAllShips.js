function DrawAllShips(ships) {

    console.log(ships);

    var shipwindow = document.getElementById("shipWindow")
    shipwindow.style.backgroundColor = "white";

    //ships.forEach((ship) => { console.log("корабль") })

   
    

    for (const ship in ships) {
        if (!ships[ship].IsPlaced) {
            var text = document.createElement("p")
            text.textContent = ship;
            text.classList.add('placeship_number')

            var choseThisShip = document.createElement("td");
            choseThisShip.classList.add('placeship_button');
            let functt = "SelectThisShip(event, " + ship+")";
            choseThisShip.setAttribute("onclick", functt);
            let id = "chose_" + ship
            choseThisShip.setAttribute("id", id);
            

            var z = document.createElement("tr")
            for (j = 0; j < ships[ship].Lenght; j++) {
                
                var zz = document.createElement("td")
                zz.classList.add('myblock');
                zz.classList.add('placeship_pixels');
                z.appendChild(zz);
            }
            shipwindow.appendChild(text);
            shipwindow.appendChild(choseThisShip);
            shipwindow.appendChild(z);
        }
    }

    for (const ship in ships) {
        if (!ships[ship].IsPlaced) {
            sessionStorage.setItem("selectedShip", ship);
            let idd = "chose_" + ship
            let x = document.getElementById(idd);
            x.textContent = "X"
            break;
        }
    }


}

