function Shoot(event) {
    const target = event.target;
    const jsonparams = '{"CellId" : ' + target.id + '}'
    //console.log("Отредактированный метод" + jsonparams);

    $.ajax({
        type: "PUT",
        url: '/User/Shoot',
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