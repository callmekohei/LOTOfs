// ===========================================================================
//  FILE    : mizuho.fsx
//  AUTHOR  : callmekohei <callmekohei at gmail.com>
//  License : MIT license
// ===========================================================================


/// みずほ銀行のホームページから当選番号を取得するスクリプト

namespace mizuho
#r @"./packages/FSharp.Data/lib/portable-net45+sl50+netcore45/FSharp.Data.dll"

open FSharp.Data
open System

module public Mizuho =

    type public Loto = {
        baseURL       : string ;
        URL           : string ;
        backnumberURL : string ;
        contains      : string ;
        kaisuu        : string ;
        kaisaibi      : string ;
        hitNumber     : string ;
        chunkSize     : int ;
        truncateSize  : int
    }

    let public loto6_head:Loto = {
        baseURL        = @"http://www.mizuhobank.co.jp/"
        URL            = @"http://www.mizuhobank.co.jp/takarakuji/loto/loto6/index.html"
        backnumberURL  = @"https://www.mizuhobank.co.jp/takarakuji/loto/backnumber/index.html"
        contains       = ""
        kaisuu         = @"table.typeTK > thead > tr > th.alnCenter.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td[colspan='6'].alnCenter"
        hitNumber      = @"table.typeTK > tbody > tr > td.alnCenter.extension > strong"
        chunkSize      = 7
        truncateSize   = 6
    }

    let public loto6_body:Loto = {
        baseURL        = @"http://www.mizuhobank.co.jp/"
        URL            = @""
        backnumberURL  = @"https://www.mizuhobank.co.jp/takarakuji/loto/backnumber/index.html"
        contains       = @"lt6"
        kaisuu         = @"table.typeTK > thead > tr > th.alnCenter.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td[colspan='6'].alnCenter"
        hitNumber      = @"table.typeTK > tbody > tr > td.alnCenter.extension > strong"
        chunkSize      = 7
        truncateSize   = 6
    }

    let public loto6_foot:Loto = {
        baseURL        = @"http://www.mizuhobank.co.jp/"
        URL            = @""
        backnumberURL  = @"https://www.mizuhobank.co.jp/takarakuji/loto/backnumber/index.html"
        contains       = @"loto6"
        kaisuu         = @"table.typeTK > tbody > tr > th.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td.alnRight"
        hitNumber      = @"table.typeTK > tbody > tr > td.''"
        chunkSize      = 6
        truncateSize   = 6
    }

    let public loto7_head:Loto = {
        baseURL        = @"http://www.mizuhobank.co.jp/"
        URL            = @"http://www.mizuhobank.co.jp/takarakuji/loto/loto7/index.html"
        backnumberURL  = @"https://www.mizuhobank.co.jp/takarakuji/loto/backnumber/index.html"
        contains       = @""
        kaisuu         = @"table.typeTK > thead > tr > th.alnCenter.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td[colspan='7'].alnCenter"
        hitNumber      = @"table.typeTK > tbody > tr > td.extension.alnCenter > strong"
        chunkSize      = 9
        truncateSize   = 7
    }

    let public loto7_body:Loto = {
        baseURL        = @"http://www.mizuhobank.co.jp/"
        URL            = @""
        backnumberURL  = @"https://www.mizuhobank.co.jp/takarakuji/loto/backnumber/index.html"
        contains       = @"lt7"
        kaisuu         = @"table.typeTK > thead > tr > th.alnCenter.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td[colspan='7'].alnCenter"
        hitNumber      = @"table.typeTK > tbody > tr > td.extension.alnCenter > strong"
        chunkSize      = 9
        truncateSize   = 7
    }

    let public loto7_foot:Loto = {
        baseURL        = @"http://www.mizuhobank.co.jp/"
        URL            = @""
        backnumberURL  = @"https://www.mizuhobank.co.jp/takarakuji/loto/backnumber/index.html"
        contains       = @"loto7"
        kaisuu         = @"table.typeTK > tbody > tr > th.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td.alnRight"
        hitNumber      = @"table.typeTK > tbody > tr > td.alnCenter"
        chunkSize      = 9
        truncateSize   = 7
    }


    let private absURL (bURL:string) ( url:string) = ( new Uri( ( new Uri (bURL) ) , url ) ).AbsoluteUri

    let private listOfUrl url =
        Http.RequestString( url , responseEncodingOverride = "utf-8")
        |> HtmlDocument.Parse
        |> fun doc -> doc.CssSelect( "td > a" )
        |> List.choose ( fun x -> x.TryGetAttribute("href") |> Option.map (fun a -> a.Value() ) )

    let private atariImpl (loto:Loto) (url:string) =

        let mutable URL = match url with  "" -> loto.URL | _ -> url

        Http.RequestString( URL  , responseEncodingOverride = "utf-8")
        |> HtmlDocument.Parse
        |> fun doc ->
            let index = doc.CssSelect( loto.kaisuu )    |> List.map ( fun n -> n.InnerText()   |> fun s -> String.filter Char.IsDigit s )
            let date  = doc.CssSelect( loto.kaisaibi )  |> List.map ( fun n -> n.InnerText()   |> DateTime.Parse |> fun dt -> dt.ToString "yyyy/MM/dd" )
            let lst   = doc.CssSelect( loto.hitNumber ) |> List.map ( fun n -> n.InnerText() ) |> List.chunkBySize loto.chunkSize |> List.map ( List.truncate loto.truncateSize )
            (index, date, lst)
        |||> List.map3 ( fun a b c -> [a] @ [b] @ c )

    let public Atari (loto:Loto) =

        match loto.URL with
        | "" ->
            listOfUrl loto.backnumberURL
            |> List.filter  ( fun s   -> s.Contains(loto.contains) )
            |> List.map     ( fun s   -> absURL loto.baseURL s )
            |> List.collect ( fun url -> atariImpl loto url )
        | _ ->
            atariImpl loto ""




