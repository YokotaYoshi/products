const menu = document.querySelector('#menu');

const lists = [
    'strawberry.jpg',
    'lime.jpg',
    'mango.jpg',
    'lemon.jpg',
    'fig.jpg',
    'apple.jpg',
];
//console.log(lists[0]);

const content = `<div><img src="images/${lists[0]}" alt=""></div>
    <div><img src="images/${lists[1]}" alt=""></div>
    <div><img src="images/${lists[2]}" alt=""></div>
    <div><img src="images/${lists[3]}" alt=""></div>
    <div><img src="images/${lists[4]}" alt=""></div>
    <div><img src="images/${lists[5]}" alt=""></div>
`;

menu.insertAdjacentHTML('beforeend', content);