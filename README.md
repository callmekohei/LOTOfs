[![MIT-LICENSE](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)](https://github.com/callmekohei/lotofs/blob/master/LICENSE)


# lotofs

ロト６、ロト７をあててお寿司を食べるためのスクリプト

### Target
OSX

### Install
```
$ git clone --depth 1 https://github.com/callmekohei/lotofs
```

### Prepare

```
（TODO: ビルドスクリプトにて自動化する）
```

`libSQLite.Interop.dylib`をコンパイルするして`lotofs`フォルダにコピーする

下記からソースコードをダウンロード

[https://system.data.sqlite.org/index.html/doc/trunk/www/downloads.wiki]

```
List of Release Packages
-> Source Code
-> sqlite-netFx-full-source-x.x.xxx.x.zip
```

`zip`を解凍して

```bash
$ cd Setup

$ bash compile-interop-assembly-release.sh

$ tree bin
bin
└── 2013
    └── Release
        └── bin
            └── libSQLite.Interop.dylib
```

lotofsフォルダにコピー

```
lotofs [master]$ cp /path/to/libSQLite.Interop.dylib ./
```

`loto.exe`をコンパイル
```
$ bash build.bash
```


### Usage

```
// 実行ファイルがあるフォルダに移動
$ cd bin_lotofs

// ロト６の予想（初回は時間がかかります）
$ mono loto.exe 

// ロト７の予想（初回は時間がかかります）
$ mono loto.exe 7
```
 
### Files
```
.
├── README.md           // このファイル
└── src                 // コードが格納されているフォルダ
    ├── loto.fsx        // 本体スクリプト
    ├── mizuho.fsx      // ホームページから出目を取得するスクリプト
    ├── register.fsx    // データーベースに出目を登録するスクリプト
    └── sqlite.fsx      // sqlite3への操作をまとめたスクリプト
```


### See also

http://callmekohei00.hatenablog.com/entry/2017/03/19/091654
