const heading = document.querySelector('.animation-js');

heading.animate({
    scale: [1, 2, 1],
  }, {
    duration: 2000,
    iterations: Infinity,
});