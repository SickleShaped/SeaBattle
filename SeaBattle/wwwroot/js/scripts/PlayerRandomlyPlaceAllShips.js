﻿function PlayerRandomlyPlaceAllShips()
{

    $.ajax({
    type: "Post",
        url: 'https://localhost:7031/User/PlaceAllShip',
        success: function (result) {
            DrawAllShips(result)
            PaintShips(result)
            WriteErrors(result)
            return result
        },
        error: function(error) {
            console.error('Error:', error.statusText);
        }
    });
}