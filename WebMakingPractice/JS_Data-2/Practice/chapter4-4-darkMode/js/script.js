const btn = document.querySelector("#btn");

btn.addEventListener("click", ()=>{
    //クリックされたときの処理
    document.body.classList.toggle("dark-theme");

    if (btn.textContent ==="ダークモードにする")
    {
        btn.textContent="ライトモードにする";
    }
    else
    {
        btn.textContent="ダークモードにする";
    }
})