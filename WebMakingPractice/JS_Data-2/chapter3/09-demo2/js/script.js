const text = document.querySelector('#colorText');
const color = document.querySelector('#colorPicker');

// カラーピッカーが変更されたら、カラーコードを表示
color.addEventListener('input', () => {
  text.textContent = `カラーコード：${color.value}`;
});