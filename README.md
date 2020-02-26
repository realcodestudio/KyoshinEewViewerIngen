# KyoshinEewViewerIngen
Kyoshin Eew Viewer for ingen

## クレジット/スペシャルサンクス (敬称略)

- [こんぽ](https://twitter.com/compo031)
  - 観測点情報の提供(再配布禁止のため未同梱)
- [M-nohira](https://github.com/M-nohira)
  - 距離と中心座標から円を描画するアルゴリズム( `RenderObjects/EllipseRenderObject.cs` )
- [ResourceBinding](https://stackoverflow.com/questions/20564862/binding-to-resource-key-wpf)
- [Douglas Peucker algorithm](https://www.codeproject.com/Articles/18936/A-C-Implementation-of-Douglas-Peucker-Line-Appro)
- [OpenStreetMap](https://openstreetmap.org)
  - 抽出し加工したものをTopoJsonにし、MessagePack+LZ4加工して使用
- モトヤLマルベリ3等幅
  - [Apache 2.0ライセンス](http://www.apache.org/licenses/LICENSE-2.0)
- FontAwesome 5 Free
- [JMA2001走時表](https://www.data.jma.go.jp/svd/eqev/data/bulletin/catalog/appendix/trtime/trt_j.html) (C) JMA
  - 深さを10km刻み、時間を1000倍し整数にしたものをMessagePack+LZ4に加工して使用

## ビルド方法

### 必要環境

- Windows10
- .NET Core SDK 3.1.100 以上
- (スクリプト実行時のみ)CドライブにインストールしたVisual Studio 2019
  - Warpで出力させたものをeditbinで編集するため `C++ によるデスクトップ開発` が必要です。

### 追加で必要なファイル

`/src/KyoshinEewViewer/Resources/ShindoObsPoints.mpk.lz4`  
こんぽ氏から頂いたものを加工したものですが、再配布は許可されていませんので [KyoshinShindoPlaceEditor](https://github.com/ingen084/KyoshinShindoPlaceEditor) を使用して作成して頂く必要があります。

### お手軽ビルド

1. `publish.bat` を実行します。
2. なんやかんやあって `tmp/KyoshinEewViewer.exe` が生成されます。

### 注意点

[Wrap](https://github.com/dgiagio/warp) を使用しています。  
これはファイルの更新日時を見て一時ファイルを更新しているため、状況によっては最新バージョンが適用されない可能性があります。