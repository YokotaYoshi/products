const menu = document.querySelector('#menu');

const lists = [
    'strawberry.jpg',
    'lime.jpg',
    'mango.jpg',
    'lemon.jpg',
    'fig.jpg',
    'apple.jpg',
];
console.log(lists);

const content = `<div><img src="images/strawberry.jpg" alt=""></div>`;

menu.insertAdjacentHTML('beforeend', content);