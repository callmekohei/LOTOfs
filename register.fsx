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

    type LOTO  = Loto6 | Loto7
    type Loto  = { number: int ; lst:int list ; kind: LOTO }
    let  loto6 = { number = 6 ; lst = [1..43] ; kind = Loto6 }
    let  loto7 = { number = 7 ; lst = [1..37] ; kind = Loto7 }

    let db:SQ3 = sqlite.Database.SQ3( sqlite_connection ) 

    /// データーベースに登録するための当選情報の文字列を作成する関数 
    let atariList (loto:Loto) atariData  : string list =

        let str = match loto.kind with
                    | Loto6 -> @"INSERT INTO loto6 ( id, date, n1, n2, n3, n4, n5, n6 )     VALUES (" 
                    | Loto7 -> @"INSERT INTO loto7 ( id, date, n1, n2, n3, n4, n5, n6, n7 ) VALUES ("

        atariData
        |> List.map( fun l -> l |> List.fold ( fun acc s -> acc + "," + "'" + s + "'" |> fun s -> s.TrimStart(',') ) "" )
        |> List.map( fun s -> str + s + ")" )

    /// データーベースに当選番号情報を登録する関数
    let register (db:SQ3) atariStringsList : unit =
        db.sqlite_open
        atariStringsList |> List.iter ( fun s -> db.sqlite_insert s )
        db.sqlite_close

    /// 最新のid をデーターベースから取得する関数
    let lastID_sqlite (db:SQ3) (loto:Loto) : int =

        let cq = new ConcurrentQueue<_>()

        let str = match loto.kind with
                    | Loto6 -> @"SELECT id FROM loto6"
                    | Loto7 -> @"SELECT id FROM loto7"

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
    let lastID_mizuho (loto:Loto) : int =

        Atari ( match loto.kind with Loto6 -> loto6_head | Loto7 -> loto7_head )
        |> List.head
        |> List.head
        |> int

    /// 不足している当選情報リストをデーターベースに追加する
    let doRegister (loto:Loto) :unit =
        let short = lastID_mizuho loto - lastID_sqlite db loto
        match short with
        | _ when short <  0   -> failwith "error!"
        | _ when short =  0   -> ()
        | _ when short <= 5   ->
            if loto.kind = Loto6
            then 
                ( atariList loto (Atari loto6_head) )
                |> List.take short
                |> register db
            else 
                ( atariList loto (Atari loto7_head) )
                |> List.take short
                |> register db
        | _ when short <= 100 ->
            if loto.kind = Loto6
            then 
                let body = atariList loto (Atari loto6_body |> List.rev)
                let head = atariList loto (Atari loto6_head |> List.rev)
                ( body @ head )
                |> List.take short
                |> register db
            else 
                let body = atariList loto (Atari loto7_body |> List.rev)
                let head = atariList loto (Atari loto7_head |> List.rev)
                ( body @ head )
                |> List.take short
                |> register db
        | _ ->
            if loto.kind = Loto6
            then          
                let foot = atariList loto (Atari loto6_foot)
                let body = atariList loto (Atari loto6_body |> List.rev)
                let head = atariList loto (Atari loto6_head |> List.rev)
                ( foot @ body @ head )
                |> List.rev
                |> List.take short
                |> List.rev
                |> register db
            else
                let foot = atariList loto (Atari loto7_foot)
                let body = atariList loto (Atari loto7_body |> List.rev)
                let head = atariList loto (Atari loto7_head |> List.rev)
                ( foot @ body @ head )
                |> List.rev
                |> List.take short
                |> List.rev
                |> register db


    

