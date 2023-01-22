import { ScrollRestore } from "./scroll_restore.js";
import * as searchjs from "./search_engine.js";

/**
 * 検索ページビュー。
 */
export class SearchView {
    private readonly _inputKeyword = document.querySelector<HTMLInputElement>("#sdi-input-search-keyword")!;
    private readonly _searchForm = document.querySelector<HTMLFormElement>("#sdi-container form")!;
    private readonly _headerArticleList = document.querySelector<HTMLElement>("#sdi-header-article-list")!;
    private readonly _articleList = document.querySelector<HTMLElement>("#sdi-body-article-list")!;
    private readonly _footer = document.querySelector<HTMLElement>("#sdi-footer")!;

    /**
     * コンストラクタ。
     */
    public constructor(
        private readonly _dbLinkFormatWithCacheBusting: string,
    ) {
        this._searchForm.addEventListener("submit", e => {
            if (this._inputKeyword.value.trim().length === 0) {
                this._inputKeyword.focus();
                e.stopPropagation();
                e.preventDefault();
            }
        });
        this._inputKeyword.focus();
    }

    /**
     * ビューの構築を開始する。
     */
    public start(): void {
        const scrollRestore = new ScrollRestore();
        const keywordString = new URLSearchParams(window.location.search).get("q")?.trim();
        // URLSearchParamsを使用すれば decodeURIComponent(keywordString.replace(/\+/g, "%20")) の実行は不要となる

        if (keywordString === undefined) {
            this._footer.style.visibility = "visible";
            return;
        }

        this._inputKeyword.value = keywordString;
        const articleElementFactory = new ArticleElementFactory();

        new searchjs.Searcher(
            keywordString,
            this._dbLinkFormatWithCacheBusting,
            () => {
                SearchView.setDocumentTitle(keywordString);
                this._headerArticleList.textContent = "検索中...";
            },
            (index, hitResult) => {
                this._articleList.appendChild(articleElementFactory.create(hitResult));
            },
            hitCount => {
                this._headerArticleList.textContent = `検索結果 (${hitCount}件)`;
                this._footer.style.visibility = "visible";
                scrollRestore.restore();
            }).search();
    }

    private static setDocumentTitle(keywordString: string): void {
        const titleWord = keywordString.replace(/\s+/g, " ");
        const separateIndex = document.title.indexOf("|");
        document.title = document.title.slice(0, separateIndex) + titleWord + document.title.slice(separateIndex - 1);
    }
}

/**
 * 検索結果の表示要素の生成用。
 */
class ArticleElementFactory {
    /**
     * 検索結果1件分の表示要素を生成する。
     */
    public create(hitResult: searchjs.HitResult): HTMLDivElement {
        const article = hitResult.article;

        // 日付
        const dateElem = DomUtil.createDiv({ cls: "sdc-article-list-date", text: article.date });
        // タイトル
        const titleElem = DomUtil.createDiv({ cls: "sdc-article-list-title" });
        titleElem.appendChild(DomUtil.createAnchor({ url: article.url, text: article.title }));
        // タグ
        const tagsElem = DomUtil.createDiv({ cls: "sdc-tags sdc-article-list-tags" });
        const categoryDiv = DomUtil.createDiv({ cls: `sdc-tag sdc-primary-tag sdc-primary-tag${article.category.order}` });
        categoryDiv.appendChild(DomUtil.createAnchor({ url: article.category.url, text: article.category.name }));
        tagsElem.appendChild(categoryDiv);
        for (const tag of article.tags) {
            const tagDiv = DomUtil.createDiv({ cls: "sdc-tag" });
            tagDiv.appendChild(DomUtil.createAnchor({ url: tag.url, text: tag.name }));
            tagsElem.appendChild(tagDiv);
        }

        // 行アイテム
        const itemElem = DomUtil.createDiv({ cls: "sdc-article-list-item" });
        itemElem.appendChild(titleElem);
        itemElem.appendChild(tagsElem);
        itemElem.appendChild(dateElem);

        // 概要
        const startIndex = Math.max(0, hitResult.contentHitIndex - 70);
        const summary = (startIndex === 0 ? "" : "…") + article.content.substr(startIndex, 140) + "…";
        itemElem.appendChild(DomUtil.createDiv({ cls: "sdc-article-list-summary", text: summary }));

        return itemElem;
    }
}

/**
 * DOMユーティリティ。
 */
class DomUtil {
    private constructor() { }

    /**
     * div要素を生成する。
     */
    public static createDiv(props: { id?: string, cls?: string, text?: string }): HTMLDivElement {
        const elem = document.createElement("div");
        if (props.id !== undefined) {
            elem.id = props.id;
        }
        if (props.cls !== undefined) {
            elem.className = props.cls;
        }
        if (props.text !== undefined) {
            elem.textContent = props.text;
        }
        return elem;
    }

    /**
     * a要素を生成する。
     */
    public static createAnchor(props: { url: string, text: string }): HTMLAnchorElement {
        const elem = document.createElement("a");
        elem.href = props.url;
        elem.textContent = props.text;
        return elem;
    }
}
