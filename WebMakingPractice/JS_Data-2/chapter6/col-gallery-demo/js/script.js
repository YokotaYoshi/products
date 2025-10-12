const menu = document.querySelector('#menu');

const lists = [
    'strawberry.jpg',
    'lime.jpg',
    'mango.jpg',
    'lemon.jpg',
    'fig.jpg',
    'apple.jpg',
];

for(let i = 0; i < lists.length; i++){
    // HTMLに要素を生成
    const content = `<div class="img-item"><img src="images/${lists[i]}" alt=""></div>`;
    menu.insertAdjacentHTML('beforeend', content);
    
    // すべての img-item クラスを取得
    const items = document.querySelectorAll('.img-item');
    
    // アニメーションの設定
    const keyframes = {
      opacity: [0, 1]
    };
    const options = {
      duration: 600,
      delay: i * 300,
      fill: 'forwards',
    };
    items[i].animate(keyframes, options);
}