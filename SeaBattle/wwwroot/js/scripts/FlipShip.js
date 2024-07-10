function FlipShip() {
    $.ajax({
        type: "Post",
        url: 'https://localhost:7031/Ship/FlipShip',
        dataType: "json",

        success: function (result) {

            sessionStorage.setItem("ShipLenght", result.ShipLenght)

            if (result.Direction) {
                sessionStorage.setItem("textposition", "Горизонтально")
            }
            else {
                sessionStorage.setItem("textposition", "Вертикально")
            }

            window.location.reload();
            
            
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });
}

