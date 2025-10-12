// 監視対象が範囲内に現れたら実行する動作
const animateScroll = (entries, obs) => {
  entries.forEach((entry) => {
    if (entry.isIntersecting) {
      entry.target.animate(
        {
          translate: [0, '100%',],
        },
        {
          duration: 2000,
          pseudoElement: '::before',
          easing: 'ease',
          fill: 'forwards',
        }
      );
      
      // 一度実行されたら監視をやめる
      obs.unobserve(entry.target);
    }
  });
};

// 監視設定
const scrollObserver = new IntersectionObserver(animateScroll);

// 監視の指示
const scrollElements = document.querySelectorAll('.scroll');
scrollElements.forEach((scrollElement) => {
  scrollObserver.observe(scrollElement);
});