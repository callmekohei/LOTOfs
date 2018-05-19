[![MIT-LICENSE](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)](https://github.com/callmekohei/lotofs/blob/master/LICENSE)


# lotofs

ロト６、ロト７をあててお寿司を食べるためのスクリプト

## Target

OSX

## Requires

Firefox  
[mono](https://github.com/mono/mono)  
[fsharp](https://github.com/fsharp/fsharp)  
[paket](https://github.com/fsprojects/Paket)


## Install

```
$ git clone --depth 1 https://github.com/callmekohei/lotofs
$ cd lotofs/
$ bash build.bash
```

## Usage

binary version
```shell
$ cd bin/
$ mono main_binary.exe
$ mono main_binary.exe 7
```

## Shortcut

binary version
```bash
# bash_profile
alias loto6="cd /Users/callmekohei/tmp/lotofs/bin/ ; mono main_binary.exe   ; cd -"
alias loto7="cd /Users/callmekohei/tmp/lotofs/bin/ ; mono main_binary.exe 7 ; cd -"
```

## Files

```
.
├── README.md
└── src
    ├── loto.fsx         // （ここ重要！）ロトを当てるためのアイディアいろいろ
    ├── main_binary.fsx  // 起動用スクリプト
    ├── mizuho.fsx       // ホームページから当選番号を取得するスクリプト
    ├── register.fsx     // データーベースに当選番号を登録するスクリプト
    └── sqlite.fsx       // sqlite3への操作をまとめたスクリプト
```

## TODO

- テストを書く
- wikiページにいろいろアイディアを書く
- 今日の当選番号みたいな感じで、月・木・金はどこかに表示する
