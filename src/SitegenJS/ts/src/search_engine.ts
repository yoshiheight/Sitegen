/**
 * grep型の全文検索を行う。
 */
export class Searcher {
    private readonly _searchCondition: SearchCondition;

    /**
     * コンストラクタ。
     * @param _searchCondition 検索条件
     * @param _dbLinkFormat 検索データベースJSONファイルのURL書式
     * @param _startCallback 検索開始時のコールバック
     * @param _hitCallback 検索がヒットする毎のコールバック
     * @param _completeCallback 検索完了時のコールバック
     */
    public constructor(
        keywordString: string,
        private readonly _dbLinkFormat: string,
        private readonly _startCallback: () => void,
        private readonly _hitCallback: (index: number, hitResult: HitResult) => void,
        private readonly _completeCallback: (hitCount: number) => void,
    ) {
        this._searchCondition = new SearchCondition(keywordString);
    }

    /**
     * 検索開始。
     */
    public search(): void {
        this._startCallback();
        this.searchAjaxJson(0, 0);
    }

    /**
     * 指定番号の検索データベースJSONをフェッチして検索する。
     */
    private searchAjaxJson(dbFileNo: number, hitCount: number): void {
        const dbUrl = this._dbLinkFormat.replace("{0}", String(dbFileNo));
        fetch(dbUrl)
            .then(response => response.json())
            .then((articles: ReadonlyArray<ArticleEntity>) => {
                for (const article of articles) {
                    // 終端の場合
                    if (article.url.length === 0) {
                        this._completeCallback(hitCount);
                        return;
                    }

                    const hitResult = this._searchCondition.searchIn(article);
                    if (hitResult !== undefined) {
                        this._hitCallback(hitCount++, hitResult);
                    }
                }

                // 次のJSONの検索へ
                this.searchAjaxJson(dbFileNo + 1, hitCount);
            });
    }
}

/**
 * 検索条件。
 */
class SearchCondition {
    private readonly _keywords: ReadonlyArray<Keyword>; // 検索ワード配列

    /**
     * コンストラクタ。
     */
    public constructor(keywordString: string) {
        const quoteRegex = /".*?"/g;
        this._keywords = (keywordString.match(quoteRegex) ?? []).map(keyword => keyword.slice(1, -1))
            .concat(keywordString.replace(quoteRegex, "").match(/\S+/g) ?? [])
            .map(keyword => new Keyword(keyword.toLowerCase()))
            .filter(keyword => keyword.isValid);
    }

    /**
     * 指定の記事エンティティ内を検索する。
     */
    public searchIn(article: ArticleEntity): HitResult | undefined {
        const content = article.content.toLowerCase();
        const title = article.title.toLowerCase();
        const tags: ReadonlyArray<string> = article.tags
            .map(tag => tag.name)
            .concat(article.category.name)
            .map(tag => tag.toLowerCase());

        let contentHitIndex = -1;
        const isHit = this._keywords.length > 0
            && this._keywords.every(keyword => (contentHitIndex = keyword.searchIn(title, tags, content)) !== -1);
        return isHit ? new HitResult(article, contentHitIndex) : undefined;
    }
}

/**
 * 検索におけるキーワード1つ分。
 */
class Keyword {
    /**
     * コンストラクタ。
     */
    public constructor(private readonly _keyword: string) { }

    /**
     * 有効なキーワードかどうか。
     */
    public get isValid(): boolean {
        return this._keyword.length > 0;
    }

    /**
     * 指定の記事情報内を検索する。
     */
    public searchIn(title: string, tags: ReadonlyArray<string>, content: string): number {
        if (title.indexOf(this._keyword) !== -1 || tags.some(tag => tag.indexOf(this._keyword) !== -1)) {
            return 0;
        } else {
            return content.indexOf(this._keyword);
        }
    }
}

/**
 * 記事エンティティ。
 */
export interface ArticleEntity {
    readonly url: string;
    readonly title: string;
    readonly date: string;
    readonly category: CategoryEntity;
    readonly tags: ReadonlyArray<TagEntity>;
    readonly content: string;
}

export interface CategoryEntity {
    readonly order: number;
    readonly name: string;
    readonly url: string;
}

export interface TagEntity {
    readonly name: string;
    readonly url: string;
}

/**
 * 検索結果1件分。
 */
export class HitResult {
    /**
     * コンストラクタ。
     * @param article 記事エンティティ
     * @param contentHitIndex コンテンツでヒットした文字列位置
     */
    public constructor(
        public readonly article: ArticleEntity,
        public readonly contentHitIndex: number,
    ) { }
}
