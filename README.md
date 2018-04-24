[![MIT-LICENSE](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)](https://github.com/callmekohei/lotofs/blob/master/LICENSE)


# lotofs

ロト６、ロト７をあててお寿司を食べるためのスクリプト

### Target
OSX

### Install

`lotofs` は `mono` と `paket` が必要

```
$ git clone --depth 1 https://github.com/callmekohei/lotofs
$ cd lotofs/
$ bash init.bash
```

### Usage

```
// 実行ファイルがあるフォルダに移動
$ cd src/

// ロト６の予想（初回は時間がかかります）
$ fsharpi loto.fsx 

// ロト７の予想（初回は時間がかかります）
$ fsharpi loto.fsx 7
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

### Shortcut

`.bash_profile`に登録すると便利

```bash
alias loto6="cd /Users/callmekohei/tmp/lotofs/src/ ; fsharpi loto.fsx   ; cd -"
alias loto7="cd /Users/callmekohei/tmp/lotofs/src/ ; fsharpi loto.fsx 7 ; cd -"
```

### See also

http://callmekohei00.hatenablog.com/entry/2017/03/19/091654
