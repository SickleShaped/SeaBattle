function RestartGame() {
    window.sessionStorage.clear();
    $.ajax({
        type: "Put",
        url: 'https://localhost:7031/Home/RestartGame',
        success: function (result) {
            //console.log(result)

            window.location.reload();
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });

    //window.location.reload();
}
