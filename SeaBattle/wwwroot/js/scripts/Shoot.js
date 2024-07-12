function Shoot(event) {
    const target = event.target;
    const jsonparams = '{"CellId" : ' + target.id + '}'
    //console.log("Отредактированный метод" + jsonparams);

    $.ajax({
        type: "PUT",
        url: 'https://localhost:7031/User/Shoot',
        dataType: "json",
        data: jQuery.param({ json: jsonparams }),
        success: function () {
            //console.log(result.playerWin);
            window.location.reload();
        },
        error: function (error) {
            console.error('Error:', error.statusText);
        }
    });
}