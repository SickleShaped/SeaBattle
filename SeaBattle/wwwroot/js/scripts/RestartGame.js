function RestartGame() {
    window.sessionStorage.clear();
    $.ajax({
        type: "Put",
        url: '/Home/RestartGame',
        success: function (result) {
            DrawAllShips(result)
            PaintShips(result)
            WriteErrors(result)
            return result
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });

    //window.location.reload();
}
