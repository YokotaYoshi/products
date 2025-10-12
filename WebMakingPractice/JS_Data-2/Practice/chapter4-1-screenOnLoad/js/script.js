const loading = document.querySelector("#loading");

window.addEventListener("load", () => {
    //ローディングが終わった後の処理
    loading.classList.add("loaded");
});