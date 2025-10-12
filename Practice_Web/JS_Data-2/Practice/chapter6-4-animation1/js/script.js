const heading=document.querySelector("#heading");

const keyframes = {
    //opacity:[1, 1],
    //translate:["0 50px",0],
    //rotate:["x 360deg",0],
    //color:["#f66", "#fc2", "#0c6", "#0bd"],
    //color:["transparent","#fff"],
    //backgroundPosition:["100% 0", "0 0"],
    borderRadius:[
        "20% 50% 50% 70%/50% 50% 70% 50%",
        "50% 20% 50% 50%/40% 40% 60% 60%",
        "50% 40% 20% 40%/40% 50% 50% 80%",
        "50% 50% 50% 20%/40% 40% 60% 60%",
    ]
};
const options = {
    duration: 8000,//必須
    //easing: "ease",
    direction:"alternate",
    iterations: Infinity,
};

heading.animate(keyframes, options);