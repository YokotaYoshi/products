const loadingAreaLeft = document.querySelector('#loading-left');
const loadingAreaRight = document.querySelector('#loading-right');
const keyframes = {
  transform: ['scaleX(1)', 'scaleX(0)'],
};
const options = {
  duration: 1000,
  easing: 'ease',
  fill: 'forwards',
};

window.addEventListener('load', () => {
  // ローディング中（左側）
  loadingAreaLeft.animate(keyframes, options);
  
  // ローディング中（左側）
  loadingAreaRight.animate(keyframes, options);
});