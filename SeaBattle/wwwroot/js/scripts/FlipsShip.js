function FlipShip(event) {
    const target = event.target;
    if ((sessionStorage.getItem("textposition") != "Горизонтально"))
    {
        sessionStorage.setItem("textposition", "Горизонтально")
        target.textContent = "Горизонтально"
    }
    else {
        sessionStorage.setItem("textposition", "Вертикально")
        target.textContent = "Вертикально"
    }

}

