function WriteTurns() {

    $.ajax({
        type: "GET",
        url: 'https://localhost:7031/User/GetTurns',
        dataType: "json",
        success: function (result) {
            console.log(result)
            return result
            const window = document.getElementById("turnMessageWindow");
            window.textContent = result;
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });

    

}