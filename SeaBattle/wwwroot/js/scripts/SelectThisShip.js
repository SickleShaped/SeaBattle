function SelectThisShip(event, ship)
{
    console.log(ship)
    console.log(event.target.id)
    const target = event.target.id;
    let xx = "chose_"+sessionStorage.getItem("selectedShip")
    var x = document.getElementById(xx)
    x.textContent = "";

    var newww = document.getElementById(target);
    newww.textContent = "X";

    sessionStorage.setItem("selectedShip", ship)


}

