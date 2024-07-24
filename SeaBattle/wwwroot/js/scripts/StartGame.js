﻿function StartGame() {
    window.sessionStorage.clear();
    $.ajax({
        type: "Post",
        url: 'https://localhost:7031/Home/StartGame',
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