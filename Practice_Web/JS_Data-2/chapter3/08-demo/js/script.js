const text = document.querySelector('#colorText');
const color = document.querySelector('#colorPicker');

text.textContent = `カラーコード：${color.value}`;

// カラーピッカーが変更されたら colorBg を発動させる
color.addEventListener('input', colorBg);