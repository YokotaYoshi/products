const images = document.querySelectorAll('.gallery img');
const body = document.body;

images.forEach((image)=>{
  image.addEventListener('mouseover', (event) => {
      body.style.backgroundImage = `url(${event.target.src})`;
      event.target.animate({opacity: [0, 1]}, 500);
  });
});