[![MIT-LICENSE](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)](https://github.com/callmekohei/lotofs/blob/master/LICENSE)


# lotofs

🍣 ロト６をあててお寿司を食べるためのスクリプト 

 
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

### 実行するときは
```
fscで実行

exeと同じ階層に libSQLite.Interop.dylib, sqliteデーターベースファイルが必要
```

### See also

http://callmekohei00.hatenablog.com/entry/2017/03/19/091654
