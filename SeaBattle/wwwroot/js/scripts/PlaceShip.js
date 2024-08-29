function PlaceShip(event) {

    if (sessionStorage.getItem("selectedShip") != null) {
        const target = event.target;


        var x = sessionStorage.getItem("textposition");
        if (x != "Горизонтально") {
            x = 1
        }
        else {
            x = 0
        }

        const jsonparams = '{"CellId" : ' + target.id + ', "ShipId" : ' + sessionStorage.getItem("selectedShip") + ', "Direction":' + x + '}'

        $.ajax({
            type: "Post",
            url: 'https://localhost:7031/User/PlaceShip',
            data: jQuery.param({ json: jsonparams }),
            success: function (result) {

                DrawAllShips(result)
                PaintShips(result)
                WriteErrors(result)
                return result
            },
            error: function (error) {
                console.error('Error:', error.statusText);
            }
        });
    }

    
}