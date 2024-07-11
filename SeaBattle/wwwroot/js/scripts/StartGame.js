function StartGame() {
    window.sessionStorage.clear();
    $.ajax({
        type: "Get",
        url: 'https://localhost:7031/Home/StartGame',
        success: function (result) {
            console.log(result)

            //window.location.reload();
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });

    //window.location.reload();
}