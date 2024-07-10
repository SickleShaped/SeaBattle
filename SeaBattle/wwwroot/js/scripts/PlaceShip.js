
function PlaceShip(event) {
    let target = event.target;
    let isgamestarted = localStorage.getItem('IsGameStarted')
    if (localStorage.getItem('IsGameStarted') != true)
    {
        
        //target.style.background = 'Red';
        //console.log(target.id)
        const currentship = +localStorage.getItem('CurrentShip');

        const data = JSON.parse(localStorage.getItem('Ships'));


        let jsonparams = '{ "PlayerTable" : ' + localStorage.getItem('PlayerTable') + ', "ShipLenght" : ' + data[currentship].Lenght +', "Direction" : ' + localStorage.getItem('textposition') + ', "Cell" : ' + target.id + '}'
        // console.log(jsonparams)

        $.ajax({
            type: "Post",
            //url: '@Url.Action("PlaceShip", "Home")',
            url: 'https://localhost:7031/Ship/PlaceShip',
            dataType: "json",
            data: jQuery.param({ json: jsonparams }),


            success: function (result) {
                //console.log(result)
                localStorage.setItem('PlayerTable', JSON.stringify(result.PlayerTable))
                localStorage.setItem('CurrentShip', currentship + 1);
                //console.log(localStorage.getItem('PlayerTable'))
                window.location.reload();
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    }
    

    
}