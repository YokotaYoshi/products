const showKirin = (entires) =>{
    
    const keyframes={
        opacity: [0,1],
        translate:["200px 0",0],
    }
    console.log(entires[0].target);
    entires[0].target.animate(keyframes, 600);
};

const kirinObserver = new IntersectionObserver(showKirin);

kirinObserver.observe(document.querySelector("#kirin"));