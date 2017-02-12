# MVVM+VC (Model-View-ViewModel + ViewController)
TwichirpのアーキテクチャはMVVMパターンをもとにしています  
Android開発への最適化を行っています  
テスト段階なのでアーキテクチャの構成は変わるかもしれません(特にUWPやiOSなどの他のプラットフォームへ組み込むとき)

## フォルダ・ファイル構造

### Core
- [Other]
- App
  - [Other]
  - Model
     - ドメイン層、データアクセス層、データ層など
     - Modelは変更通知可能なプロパティ、Model内で起きたイベントを通知するイベントハンドラー、Model内を操作する返り値なしのメソッド、ModelのデータをJsonとしてエクスポートするメソッドから構成されています
     - ModelはViewの状態補足・参照をしません、また基本的にViewModelを参照しません
  - ViewModel
     - 情報伝達層
     - ViewModelはViewへ出力またはViewから入力されるデータのプロパティ、Viewにバインディングするコマンド、Viewに関わる定数から構成されています
     - ViewModelはViewとModelとの橋渡し役を担っており、必要に応じてデータなどの加工を行います
     - ただし、ViewModelはViewを参照しません
  - ITwichirpApplication
     - 各プラットフォームの機能を抽象化して表現するインターフェース
     - Setting、Manager、Resourceから構成されています
     - ITwichirpApplicationはアプリケーションプロセス内で1つのインスタンスしかないことを前提に開発を行います(iOSやUWPで表現できるかは未確認)
  - Setting
     - Twichirpのアプリケーション内設定に関するデータを表現します 
  - Manager
     - データの取得・管理などを行う何でも屋 

### Android
- [Other]
- App
  - [Other]
  - Model
    - Coreで表現できないAndroidプラットフォーム固有のデータを扱うModel
  - View
    - Interface
      - ViewControllerから参照される各Viewを抽象化したインターフェース群
    - Activity
      - 抽象化されたインターフェースを実装し、ViewModelとViewControllerを用意するのみのActivity 
    - Fragment
      - 抽象化されたインターフェースを実装し、ViewModelとViewControllerを用意するのみのFragment 
    - Holder 
      - 抽象化されたインターフェースを実装し、ViewControllerを用意するのみのHolder
      - RecyclerViewで使用され、ViewModelはRecyclerViewのAdapterから提供されます
  - ViewModel
    - Coreで表現できないAndroidプラットフォーム固有のデータを扱うViewModel 
  - ViewController
    - UI操作層
    - ViewControllerはViewによって用意され、ViewとViewModelのバインディングとViewの操作を行います

## 参照関係
~~~
ViewController
 ↑↓        ↓
View → ViewModel → Model
~~~

## データの方向
~~~
ViewController
           ↑
View ⇔ ViewModel ⇔ Model
~~~


## MVVMとの差異
基本的にMVVMの構造になっていると思いますが、ViewとViewModelとの関係にViewControllerが追加されます  
Viewを抽象化し、ViewControllerによって操作することにより以下のような問題のAndroidプラットフォームにおいてのMVVMパターンの最適化を行えています
> しかし2012年4月現在、XAMLを使用するWPFなどのテクノロジ以外で使用されるMVVMは実質Presentation Modelと変わらず、Viewの抽象化などはできない。
> https://ja.wikipedia.org/wiki/Model_View_ViewModel#Presentation_Model.E3.83.91.E3.82.BF.E3.83.BC.E3.83.B3.E3.81.A8.E3.81.AE.E9.81.95.E3.81.84 より

## 問題点
ソースコードを見ればすぐにわかると思いますが、Viewの抽象化により一つの画面を作り上げるために必要なファイル数が多くなっています