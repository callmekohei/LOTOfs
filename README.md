### Summary
```text
F#からSqliteを利用する

ホームページからデーターを取得する

ロト６を当てる！？
```

### MacOSXでF#からSqliteを利用する方法
```
System.Data.SQLiteをnugetする

libSQLite.Interop.dylibをコンパイルする

FSharpコード内でDLLImportする
```

参照: http://callmekohei00.hatenablog.com/entry/2017/02/18/084909



### Sqlite table のつくりかた
```bash
// データーベースファイル loto.sqlite3 をつくる
$ sqlite3 loto.sqlite3

// テーブルをつくる
sqlite> create table loto6 ( id, date, n1, n2, n3, n4, n5, n6 );

// テーブルの確認
sqlite> .table

// exit
sqlite> .exit
```

### 実行するときは
```
fscで実行

exeと同じ階層に libSQLite.Interop.dylib, sqliteデーターベースファイルが必要
```
