// ===========================================================================
//  FILE    : loto.fsx
//  AUTHOR  : callmekohei <callmekohei at gmail.com>
//  License : MIT license
// ===========================================================================

/// 当たるも八卦当たらぬも八卦

#r @"./packages/Selenium.WebDriver/lib/net45/WebDriver.dll"

#load @"./sqlite.fsx"
open sqlite.Database

#load @"./register.fsx"
open register

#r @"./packages/System.Data.SQLite.Core/lib/net46/System.Data.SQLite.dll"
open System.Data.SQLite

#r @"./packages/FSharp.Data/lib/net45/FSharp.Data.dll"

open System.Collections.Concurrent

module Util =

    type LOTO  = Loto6 | Loto7
    type Loto  = { number: int ; lst:int list ; kind: LOTO }
    let  loto6 = { number = 6 ; lst = [1..43] ; kind = Loto6 }
    let  loto7 = { number = 7 ; lst = [1..37] ; kind = Loto7 }

    let seqCombiUniq (lb:int) (ub:int) (n:int) : int list seq =
        System.Random()
        |> Seq.unfold ( fun rnd ->
            Some ( List.init n ( fun _ -> rnd.Next( lb, ub )), rnd ))
        |> Seq.filter ( fun l -> ( List.distinct l |> List.length  ) = n )

    let pickOne (lst: int list) : int =
        let rnd = System.Random()
        lst |> List.item (rnd.Next lst.Length)

    let createZone (loto:Loto) : int list =
        seqCombiUniq 1 (loto.number - 1) 3 |> Seq.find ( List.sum >> (=) loto.number )

    let toZone (loto:Loto) lst  =
        match loto.kind with
        | Loto6 ->
            let first = lst |> List.filter (fun elm -> elm < 16) |> List.length
            let third = lst |> List.filter (fun elm -> elm > 30) |> List.length
            let second = loto.number - ( first + third )
            [first; second; third]
        | Loto7 ->
            let first = lst |> List.filter (fun elm -> elm < 14) |> List.length
            let third = lst |> List.filter (fun elm -> elm > 26) |> List.length
            let second = loto.number - ( first + third )
            [first; second; third]

    let swapRowColumn lst =
        lst
        |> List.collect List.indexed
        |> List.groupBy fst
        |> List.map snd
        |> List.map (List.map snd)

    let createAscendantRandomList lst  : int list =
        lst
        |> List.scan (fun last -> List.filter (fun n -> last < n ) >> pickOne) 0
        |> List.tail

    let prettyPrint (separater: string) (lst: int list) : string =
        lst
        |> List.map (sprintf "%02d")
        |> String.concat separater




module Main =
    open Util

    let idea04 (loto:Loto) (n:int) =

        /// (STEP0) データーベースに当選番号情報を登録する
        register.Register.doRegister( match loto.kind with Loto6 -> Register.loto6 | Loto7 -> Register.loto7 )

        /// (STEP1) 各スロットごとの数字をリストにまとめる
        let data (loto:Loto) =

            let col = match loto.kind with Loto6 -> [2..7] | Loto7 -> [2..8]
            let s   = match loto.kind with Loto6 -> @"SELECT * FROM loto6" | Loto7 -> @"SELECT * FROM loto7"

            let cq = new ConcurrentQueue<int list>()

            let f col = fun (r:SQLiteDataReader) ->
                col
                |> List.map( fun n -> r.GetInt32(n) )
                |> fun l -> cq.Enqueue l
                |> ignore

            let db = register.Register.db
            db.sqlite_open
            db.sqlite_select s (f col)
            db.sqlite_close
            cq.ToArray() |> Array.toList |> swapRowColumn

        /// (STEP2) 各ゾーンの個数にもとづいて予測する
        let zone = createZone loto

        Seq.initInfinite (fun _ -> createAscendantRandomList (data loto ) )
        |> Seq.filter (fun l -> toZone loto l = zone )
        |> Seq.distinct
        |> Seq.take n


    [<EntryPointAttribute>]
    let main argv =
        let v:Loto = if    Array.isEmpty argv || argv.[0] <> "7"
                     then  loto6
                     else  loto7

        idea04 v 5
        |> Seq.fold ( fun acc l -> prettyPrint " " l + "\n" + acc ) ""
        |> stdout.WriteLine

        0
