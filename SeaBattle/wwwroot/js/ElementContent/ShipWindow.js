﻿
var shipwindow = document.getElementById("shipWindow")
shipwindow.classList.add('shipplacingwindow'); 
var text = document.createElement("p")
text.textContent = "Текущий корабль для размещения:"
shipwindow.appendChild(text);

if (sessionStorage.getItem("textposition") == null) {
    sessionStorage.setItem("textposition", "Горизонтально")
}

var positionship = document.createElement("p");
positionship.setAttribute("id", "textposition");
positionship.textContent = sessionStorage.getItem("textposition");
shipwindow.appendChild(positionship);

var butt = document.createElement("button");
butt.textContent = "Повернуть корабль";
butt.setAttribute("onclick", "FlipShip()");
shipwindow.appendChild(butt);

const data = JSON.parse(localStorage.getItem('Ships'))
const currentship = localStorage.getItem('CurrentShip');
let x;

console.log(x);

$.ajax({
    type: "GET",
    url: 'https://localhost:7031/Home/GetGame',
    dataType: "json",
    success: function (result) {

        x = result;
        console.log(x)
        console.log("Веби вабу")
    },
    error: function (error) {
        console.error('Error:', error);
    }
});






for (i = 0; i < data[currentship].Lenght; i++)
{
    var z = document.createElement("td")
    z.classList.add('myblock');
    shipwindow.appendChild(z);
}







