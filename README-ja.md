# デスクトップマスコット たみちゃん

[English README is here](https://github.com/sabanishi/TamiChan/blob/main/README.md)

![Image1](images/demo.gif)

## システム要件

MacOS

## インストール

Releasesから最新のバージョンの物をダウンロードして使用してください。

zipファイルを解答し、TampChan.appを実行してください。
以下のような警告が出た場合、システム設定から「プライバシーとセキュリティ」を開き、「このまま開く」を押してください。

<img src="images/image2.png" width=40%>　<img src="images/image3.png" width=40%>

## 使い方

### たみちゃん

たみちゃんはドラッグすることで場所を移動できます。

また、右クリックでコンソールを開いたり閉じたりできます。

### コマンド

コンソールからメッセージを入力することで命令を実行できます。

対応している命令は以下の通りです。

<br />

#### OpenAI APIキーの登録

- APIキーを登録することで、おしゃべり機能が使用可能になります。
- キーの取得方法は[こちら]を参照してください。

コマンド
- `!setup_chat`

引数
- `-k`,`--key`: OpenAPIキー

入力例
```
!setup_chat -k [your api key]
```

<br />

#### 翻訳

コマンド
- `!trans`

引数
- `-s`, `--sentence`: 翻訳対象文字列
- `-i`, `--input`: 入力言語
  - 指定しなかった場合、翻訳対象文字列から自動で推測されます
- `-o`, `-output`: 出力言語
  - 指定しなかった場合、日本語になります

入/出力言語として使用できる言語は以下の通りです。

- `en`: 英語
- `ja`: 日本語
- `fr`: フランス語
- `de`: ドイツ語
- `nl`: オランダ語
- `el`: ギリシャ語
- `iw`: ヘブライ語
- `hi`: ヒンディー語
- `id`: インドネシア
- `it`: イタリア語
- `ko`: 韓国語
- `la`: ラテン語
- `pl`: ポルトガル語
- `ru`: ロシア語
- `sv`: スペイン語
- `ur`: ウルドゥー語

入力例
```
!trans -s こんにちは　-i jp -o en
```

<br />

#### Gitリポジトリの登録

- ローカルのGitリポジトリを登録することで、新しいコミットをした際にたみちゃんが一緒に喜んでくれます。
- .gitがあるフォルダを絶対パスで指定してください。

コマンド
- `!git_repo`

引数
- `-p`,`--path`: Gitリポジトリのパス

入力例
```
!git_repo -p [.git path]
```
<br />

#### コンソールサイズの変更

コマンド
- `!console_size`

引数
- `-w`,`--width`: 横幅
- `-h`,`-height`: 縦幅

入力例
```
!console_size -w 1000 -h 500
```