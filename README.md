### Summary
```text
ホームページからデーターを取得する

F#からSqliteを利用する

ロト６を当てる！？
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

### 実行するときは
```
fscで実行

exeと同じ階層に libSQLite.Interop.dylib, sqliteデーターベースファイルが必要
```
