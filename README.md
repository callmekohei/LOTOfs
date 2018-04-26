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
$ bash build.bash
```

### Usage

```
// ===== script version =====
// 実行ファイルがあるフォルダに移動
$ cd src/

// ロト６の予想（初回は時間がかかります）
$ fsharpi main_script.fsx 

// ロト７の予想（初回は時間がかかります）
$ fsharpi main_script.fsx 7


// ===== binary version =====
// 実行ファイルがあるフォルダに移動
$ cd bin/

// ロト６の予想（初回は時間がかかります）
$ mono main_binary.exe

// ロト７の予想（初回は時間がかかります）
$ mono main_binary.exe 7
```
 


### Shortcut( script version)

`.bash_profile`に登録すると便利

```
// ===== script version =====
alias loto6="cd /Users/callmekohei/tmp/lotofs/src/ ; fsharpi main_script.fsx   ; cd -"
alias loto7="cd /Users/callmekohei/tmp/lotofs/src/ ; fsharpi main_script.fsx 7 ; cd -"

// ===== binary version =====
alias loto6="cd /Users/callmekohei/tmp/lotofs/bin/ ; mono main_binary.exe   ; cd -"
alias loto7="cd /Users/callmekohei/tmp/lotofs/bin/ ; mono main_binary.exe 7 ; cd -"
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
