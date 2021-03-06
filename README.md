# FormsButton

## Overview

ボタンデザインとして3状態の画像ボタンを使用したいケースは多い。
- 通常時
- 押下時
- 非活性時

XamarinFormsの吊るしButtonコントロールでは実現できないらしい。
実現方法を検討する。

## Buttonコントロール

吊るしのコントロールで実現可能な境界線を調べる。

### preview

Textに日本語を設定してもプレビューできないバグがあるらしい。

### Imageプロパティ

> .NET Standard ライブラリ プロジェクトではなく、
> 個々 のプラットフォームのプロジェクト内のリソースとして、
> ビットマップを格納する必要があります。

このプロパティは背景をビットマップにするためのものではない。
コンテントとしてテキストと並べて配置するための画像指定となる。
よってButtonのBackgroundColorの背景の上に部分的に画像が乗る。

以上で調査終了。

## カスタムボタン試作その1

### Grid方式

ボタン背景画像と透明背景Buttonを重ねる。

### ImageResourceExtension

共通部に追加したリソースファイル名をXAMLで指定する場合はコンバーターが必要となる。

### エフェクト

ButtonのBackgroundColorを透明にするとエフェクトが発生しなくなる。

### 画像サイズ

Aspect="Fill"を使用するとボタンサイズに合わせて拡縮されるが当然ゆがむ。

### 結論

アニメーションが消えるのは致命的なので二枚絵方式は必須。
Frameの半透明黒を重ねて押下状態にできるか試したが、
端まで使った四角画像でなければFrameがはみ出てしまう。
不透明マスクなどは無い。
さらにAndroidではFrameの上に部品を配置できない不具合のため、
Clickイベントが発生しなくなる。

## カスタムボタン試作その2

### Buttonとの決別

その後、UWPではフォーカス時配色が表示されてしまうことも発覚した。
Buttonを使っている限りどうにもならない。
UWPのことは放っておくかどうか。

### ゆがむ件

これはとりあえず考えない。
運用でカバーする想定とする。

### 層構成

押下時アニメーションを考えると、最下層に通常を置く必要がある。
しかし、IsEnableとIsVisibleをバインドすることを考慮すると、
最下層には非活性を置くことになる。

1. 非活性時
1. 通常時
1. 押下時
1. 文字列

### クリックイベント

文字列でタップする。

### 結論

まあまあいける。
デザインサイズに合わせて別画像を使うことを考えると部品化メリットがあまりない。
使う側としてはStyleで統一したいので歪み対策は必須だろう。
Buttonを使用しない場合Pressイベントが無くなるので、
UWPのフォーカスと比較してどちらを取るかの選択になる。

## 歪み対策

### SkiaSharp

Xamarin.FormsでBitmap操作を行うための公式ライブラリを使用する。

### DrawBitmapNinePatch

目的の関数を持っているが、9.png形式をそのまま使えるわけではなく、
独自に引数で9分割させる仕様なのでリソース任せの使い方はできない。

### 結論

外周の黒判定を独自実装すれば、
Androidの9patchフォーマットをきれいに拡大可能となる。
3状態をCanvas内で分岐すれば3層作らなくてもよくなる。

## 再利用化

NuGetに登録してみた。

https://www.nuget.org/packages/NPImage/

```html
<np:TSNPButton Text="push" Clicked="Button_Clicked"
                Source="FormsButton.Resources.dl015_button_default.9.png"
                SourcePressed="FormsButton.Resources.dl015_button_pressed.9.png"
                SourceDisabled="FormsButton.Resources.dl015_button_disabled.9.png"
                />

```