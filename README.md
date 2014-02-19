SoundUtils
==========

信号処理、音声処理に使えそうなクラスを集めた C# 用のライブラリです。必要なものがあれば少しずつ追加していきます。


## 搭載クラス群

含まれているものは以下のクラス群です。

- 配列型変換 (ArrayConvert)
  + Single <-> Double 相互変換
- ビット/バイト操作 (BitOperate)
  + エンディアンネスの逆転
- チャネル・インターリーブ操作 (Channel)
  + L, R の分解・統合
  + インターリーブ
  + デインターリーブ
- 高速フーリエ変換 (Math/FastFourier)
  + 複素数 -> 複素数
  + 実数部 -> 複素数
- 汎用的数学クラス (Math/SoundMath)
  + Sinc 関数
  + 第1種0次変形ベッセル関数
  + 逆数での階乗
- 窓関数 (Math/Window)
  + ハン窓 (ハニング窓)
  + ハミング窓
  + バートレット窓
  + ナットール窓
  + ブラックマン窓
  + ブラックマン-ハリス窓
  + ブラックマン-ナットール窓
  + フラットトップ窓
  + ウェルチ窓
  + カイザー窓 (カイザー-ベッセル窓)
- WAVE PCM書き込み (IO/WaveFormatWriter)
- フィルタリング (Filtering 名前空間)
  + FFTフィルタリング (FFTFiltering)
  + フィルタバッファ (FilterBuffer)
  + オーバーサンプリング (OverSampling)
  + 2 チャネル用フィルタクラス (SoundFilter)
- インパルス応答 (Filtering/ImpulseResponse)
  + FIR
    - ローパス
    - ハイパス
    - バンドパス
    - バンドイリミネーション
    - FIR ジェネレータ (Filtering/FIR/ImpulseGenerator)
  + IIR
    - コムフィルタ
    - リゾネータ

## ライセンス

__MIT ライセンス__

一部クラスの出典は各ファイルのヘッダに記述してあります。
