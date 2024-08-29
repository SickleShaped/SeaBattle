function WriteErrors(result) {
    var x = 0;
    if (typeof result === 'object') {
        x = result;
    }
    else {
        x = JSON.parse(result);
    }

    const window = document.getElementById("errorMessageWindow");
    switch (x.Condition.LastRequestResult) {
        case 0:
            window.textContent = " "
            break;

        case 1:
            window.textContent = "Игра уже началась!"
            break;

        case 2:
            window.textContent = "Игра еще не началась!"
            break;

        case 3:
            window.textContent = "Не все корабли расставлены!"
            break;

        case 4:
            window.textContent = "Невозможно поставить корабль в эту клетку!"
            break;

        case 5:
            window.textContent = "Вы выстрелили в уже проверенную клетку!"
            break;

        default:
            window.textContent = " "
            break;
    }

}