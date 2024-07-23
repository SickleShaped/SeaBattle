function StartGame() {
    window.sessionStorage.clear();
    $.ajax({
        type: "Post",
        url: 'https://localhost:7031/Home/StartGame',
        success: function () {
            //window.location.reload();
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });

    //window.location.reload();
}