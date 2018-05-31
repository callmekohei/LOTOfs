[![MIT-LICENSE](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)](https://github.com/callmekohei/lotofs/blob/master/LICENSE)


# lotofs

ロト６、ロト７をあててお寿司を食べるためのスクリプト

## Target

OSX

## Requires

Firefox
, [mono](https://github.com/mono/mono)
, [fsharp](https://github.com/fsharp/fsharp)
and [paket](https://github.com/fsprojects/Paket)


## Install

```
$ git clone --depth 1 https://github.com/callmekohei/lotofs
$ cd lotofs/
$ time bash build.bash
real	0m47.601s
user	0m18.274s
sys	0m3.941s
```

## Usage

Initial launch takes a lot times.

```shell
$ cd bin/

$ time mono main_binary.exe
real	3m6.793s
user	3m17.769s
sys	0m50.771s

$ time mono main_binary.exe
real	0m5.669s
user	0m5.482s
sys	0m1.685s

$ time mono main_binary.exe 7
real	1m4.405s
user	1m8.928s
sys	0m18.797s

$ time mono main_binary.exe 7
real	0m5.875s
user	0m5.356s
sys	0m1.656s
```

## Shortcut

Put it on `bashrc`
```bash
alias loto6="( cd /Users/callmekohei/tmp/lotofs/bin/ ; mono main_binary.exe   )"
alias loto7="( cd /Users/callmekohei/tmp/lotofs/bin/ ; mono main_binary.exe 7 )"
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
- 初回起動時の遅さをなんとかする
