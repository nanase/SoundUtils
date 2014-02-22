SoundUtils
==========

信号処理、音声処理に使えそうなクラスを集めた C# 用のライブラリです。必要なものがあれば少しずつ追加していきます。


## TODO

- サンプルプログラムの作成
- WAVE PCM読み込みクラス


## 搭載クラス群

含まれているものは以下のクラス群です。

- 配列型変換 ([ArrayConvert](src/ArrayConvert.cs))
  + Single <-> Double 相互変換
- ビット/バイト操作 ([BitOperate](src/BitOperate.cs))
  + エンディアンネスの逆転
- チャネル・インターリーブ操作 ([Channel](src/Channel.cs))
  + L, R の分解・統合
  + インターリーブ
  + デインターリーブ
- 高速フーリエ変換 ([Math/FastFourier](src/Math/FastFourier.cs))
  + 複素数 -> 複素数
  + 実数部 -> 複素数
- 汎用的数学クラス ([Math/SoundMath](src/Math/SoundMath.cs))
  + Sinc 関数
  + 第1種0次変形ベッセル関数
  + 逆数での階乗
- 窓関数 ([Math/Window](src/Math/Window.cs))
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
- WAVE PCM書き込み ([IO/WaveFormatWriter](src/IO/WaveFormatWriter.cs))
- フィルタリング (Filtering 名前空間)
  + FFTフィルタリング ([FFTFiltering](src/Filtering/FFTFiltering.cs))
  + フィルタバッファ ([FilterBuffer](src/Filtering/FilterBuffer.cs))
  + オーバーサンプリング ([OverSampling](src/Filtering/OverSampling.cs))
  + 2 チャネル用フィルタクラス ([SoundFilter](src/Filtering/SoundFilter.cs))
- インパルス応答 ([Filtering/ImpulseResponse](src/Filtering/ImpulseResponse.cs))
  + FIR
    - ローパス
    - ハイパス
    - バンドパス
    - バンドイリミネーション
    - FIR ジェネレータ ([Filtering/FIR/ImpulseGenerator](src/Filtering/FIR/ImpulseGenerator.cs))
  + IIR
    - コムフィルタ
    - リゾネータ


## ライセンス

__MIT ライセンス__

一部クラスの出典は各ファイルのヘッダに記述してあります。
