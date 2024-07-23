function PlayerRandomlyPlaceAllShips()
{

    $.ajax({
    type: "Post",
        url: 'https://localhost:7031/User/PlaceAllShip',
        success: function() {

            window.location.reload();
        },
        error: function(error) {
            console.error('Error:', error.statusText);
        }
    });
}