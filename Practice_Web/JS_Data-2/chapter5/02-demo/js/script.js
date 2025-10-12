const menu = document.querySelector('#menu');

const content = `<div><img src="images/strawberry.jpg" alt=""></div>`;
//menu.textContent = content;
menu.insertAdjacentHTML('beforeend', content);