// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const zone1 = document.querySelector('.zone-1');
const zone2 = document.querySelector('.zone-2');
const ship = document.querySelector('#ship');

zone1.ondragover = allwDrop;

function allowDrop(event) {
    event.preventDefault();
}

ship.ondragstart = drag;

function drag(event) {
    event.dataTransfer.setData('id', event.target.id)
}

zone1.ondrop = drop;

function drop(event) {
    let itemId = event.dataTransfer.getData('id');
    event.target.append(document.getElementById(itemId));
}