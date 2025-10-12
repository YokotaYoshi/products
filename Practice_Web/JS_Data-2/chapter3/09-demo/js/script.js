const text = document.querySelector('#colorText');
const color = document.querySelector('#colorPicker');

// カラーピッカーを操作したときの一連の動作
const colorBg = () => {
  // カラーコードを表示
  text.textContent = `カラーコード：${color.value}`;
}

// カラーピッカーが変更されたら colorBg を発動させる
color.addEventListener('input', colorBg);