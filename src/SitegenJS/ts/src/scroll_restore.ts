/**
 * 動的生成ページでリロードor履歴遷移した場合に以前のスクロール位置を復元する。
 */
export class ScrollRestore {
    // 履歴遷移についてはbfcacheが使用された場合はページ状態がそのまま復元されるが、
    // bfcache無効時やキャッシュ期限切れの場合もあるので当該処理が必要

    private readonly _sessionKey = "[ScrollRestore]" + window.location.href;

    /**
     * コンストラクタ。
     */
    public constructor() {
        if (!ScrollRestore.isReloadOrHistory()) {
            sessionStorage.setItem(this._sessionKey, JSON.stringify({ scrollY: 0 } as ScrollState));
        }

        window.addEventListener("scroll", EventUtil.debounce(() => {
            sessionStorage.setItem(this._sessionKey, JSON.stringify({ scrollY: window.scrollY } as ScrollState));
        }, 100));
    }

    /**
     * リロードor履歴遷移した場合に以前のスクロール位置を復元する。
     */
    public restore(): void {
        if (ScrollRestore.isReloadOrHistory()) {
            const json = sessionStorage.getItem(this._sessionKey);
            if (json !== null) {
                const scrollState = JSON.parse(json) as ScrollState;
                window.scrollTo(0, scrollState.scrollY);
            }
        }
    }

    /**
     * ページがリロードor履歴遷移したかどうかを判定する。
     */
    private static isReloadOrHistory(): boolean {
        const type = (window.performance.getEntriesByType("navigation")[0] as PerformanceNavigationTiming).type;
        return (type === "reload" || type === "back_forward");
    }
}

/**
 * スクロール状態。
 */
interface ScrollState {
    readonly scrollY: number;
}

/**
 * アクションデリゲート。
 */
interface Action {
    (): void;
}

/**
 * イベントユーティリティ。
 */
class EventUtil {
    private constructor() { }

    /**
     * イベントの発生が一定間隔以上になった際に処理する為の関数を生成する。
     */
    public static debounce(callback: Action, interval: number): Action {
        let timerId: number | undefined;
        return () => {
            if (timerId !== undefined) {
                window.clearTimeout(timerId);
            }
            timerId = window.setTimeout(() => {
                timerId = undefined;
                callback();
            }, interval);
        };
    }
}
