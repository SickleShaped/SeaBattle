function DrawAllShips(result) {

    let ships = 0;
    
    if (typeof result === 'object') {
        const x = result;
        ships = x.Condition.Ships
    }
    else {
        const x = JSON.parse(result);
        ships = x.Condition.Ships
    }

    var shipwindow = document.getElementById("shipWindow")
    shipwindow.classList.add('shipplacingwindow');

    sessionStorage.setItem("textposition", "Горизонтально");

    
    while (shipwindow.firstChild) {
        shipwindow.removeChild(shipwindow.lastChild);
    }
    shipwindow.style.backgroundColor = "white";

    var setAllShips = document.createElement("td")
    setAllShips.textContent = "Расставить все корбали случайно"
    setAllShips.classList.add('flipship_button');
    setAllShips.setAttribute("onclick", "PlayerRandomlyPlaceAllShips()");
    shipwindow.appendChild(setAllShips);

    var button = document.createElement("td")

    button.textContent = sessionStorage.getItem("textposition")
    button.classList.add('flipship_button');
    button.setAttribute("onclick", "FlipShip(event)");
    shipwindow.appendChild(button);

    

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

