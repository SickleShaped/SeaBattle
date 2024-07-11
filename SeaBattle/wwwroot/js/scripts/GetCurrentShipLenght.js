function GetCurrentShipLenght() {
    $.ajax({
        type: "Get",
        url: 'https://localhost:7031/Ship/GetCurrentShipLenght',

        success: function (result) {
            const x = JSON.parse(result)
            console.log(x.Lenght)

            var shipwindow = document.getElementById("shipWindow")

            for (i = 0; i < x.Lenght; i++) {
                var z = document.createElement("td")
                z.classList.add('myblock');
                shipwindow.appendChild(z);
            }

        },
        error: function (error) {
            console.error('Error:', error);
        }
    });
}