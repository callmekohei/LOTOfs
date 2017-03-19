// ===========================================================================
//  FILE    : lotoRegister.fsx
//  AUTHOR  : callmekohei <callmekohei at gmail.com>
//  License : MIT license
// ===========================================================================

/// みずほ銀行のホームページから取得した
/// ロト６の当選番号情報をデーターベースに登録するスクリプト

namespace register


#load @"./mizuho.fsx"
open mizuho.Mizuho

#load @"./sqlite.fsx"
open sqlite.Database

#r @"./packages/FSharp.Data/lib/portable-net45+sl50+netcore45/FSharp.Data.dll"

#r @"./packages/System.Data.SQLite.Core/lib/net46/System.Data.SQLite.dll"
open System.Data.SQLite

open System.Collections.Concurrent

module Register =

    let db:SQ3 = sqlite.Database.SQ3( sqlite_connection ) 

    /// データーベースに登録するための当選情報の文字列を作成する関数 
    let atariList atariData : string list =
        let str = @"INSERT INTO loto6 ( id, date, n1, n2, n3, n4, n5, n6 ) VALUES ("
        atariData
        |> List.map( fun l -> l |> List.fold ( fun acc s -> acc + "," + "'" + s + "'" |> fun s -> s.TrimStart(',') ) "" )
        |> List.map( fun s -> str + s + ")" )

    /// データーベースに当選番号情報を登録する関数
    let register (db:SQ3) atariStringsList : unit =
        db.sqlite_open
        atariStringsList |> List.iter ( fun s -> db.sqlite_insert s )
        db.sqlite_close

    /// 最新のid をデーターベースから取得する関数
    let lastID_sqlite (db:SQ3) : int =

        let cq = new ConcurrentQueue<_>()

        let str = @"SELECT id FROM loto6"

        let f = fun (r:SQLiteDataReader) -> 
            cq.Enqueue ( r.GetInt32(0) ) |> ignore

        db.sqlite_open
        db.sqlite_select str f
        db.sqlite_close 

        cq.ToArray()
        |> fun arr ->
            if Array.isEmpty arr
            then 0
            else Array.max arr

    /// 最新の抽選回数をwebから取得する関数
    let lastID_mizuho : int =
        Atari loto6_head
        |> List.head
        |> List.head
        |> int

    /// 不足している当選情報リストをデーターベースに追加する
    let doRegister () :unit =
        let short = lastID_mizuho - lastID_sqlite db
        match short with
        | _ when short <  0   -> failwith "error!"
        | _ when short =  0   -> ()
        | _ when short <= 5   ->
            ( atariList (Atari loto6_head) )
            |> List.rev
            |> List.take short
            |> List.rev
            |> register db
        | _ when short <= 100 ->
            let body = atariList (Atari loto6_body |> List.rev)
            let head = atariList (Atari loto6_head |> List.rev)
            ( body @ head )
            |> List.rev
            |> List.take short
            |> List.rev
            |> register db
        | _ ->
            let foot = atariList (Atari loto6_foot)
            let body = atariList (Atari loto6_body |> List.rev)
            let head = atariList (Atari loto6_head |> List.rev)
            ( foot @ body @ head )
            |> List.rev
            |> List.take short
            |> List.rev
            |> register db



