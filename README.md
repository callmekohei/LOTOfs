[![MIT-LICENSE](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)](https://github.com/callmekohei/lotofs/blob/master/LICENSE)


# lotofs

ロト６、ロト７をあててお寿司を食べるためのスクリプト

### Install
```
$ git clone https://github.com/callmekohei/lotofs
```

### How to run
```
$ bash build.bash
$ mono bin_lotofs/loto.exe 
$ mono bin_lotofs/loto.exe 7
```
 
### file
```
.
├── README.md                   // このファイル
├── libSQLite.Interop.dylib     // OSXでSqliteをつかうのに必要
├── loto.fsx                    // 本体スクリプト
├── loto.sqlite3                // データーベースファイル
├── mizuho.fsx                  // ホームページから取得するスクリプト
├── register.fsx                // データーベースに登録するスクリプト
└── sqlite.fsx                  // Sqliteへの操作をまとめたスクリプト
```


### See also

http://callmekohei00.hatenablog.com/entry/2017/03/19/091654
