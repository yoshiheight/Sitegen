// 全ページ共通の初期化処理用

// Chromeの不具合対策
// bfcacheでの表示時にスクロールバーが表示されない（もしくは一瞬表示されてすぐ消える）場合があるので、
// Chrome側が修正されるまで以下の処理によって強制的に再描画させスクロールバーが表示されるようにする
window.addEventListener("pageshow", e => {
    if (e.persisted) {
        document.body.style.overflow = "hidden"; // スクロールバーちらつき対策
        document.body.style.paddingBottom = "1px";
        window.setTimeout(() => {
            document.body.style.overflow = "auto";
            document.body.style.paddingBottom = "0px";
        }, 100);
    }
});
