// ===========================================================================
//  FILE    : mizuho.fsx
//  AUTHOR  : callmekohei <callmekohei at gmail.com>
//  License : MIT license
// ===========================================================================

/// みずほ銀行のホームページから当選番号を取得するスクリプト

namespace mizuho

#load "../.paket/load/net471/main.group.fsx"
open FSharp.Data
open OpenQA.Selenium
open OpenQA.Selenium.Firefox
open OpenQA.Selenium.Support.UI

open System
open System.Net
open System.Text.RegularExpressions


module public Mizuho =

    type public Loto = {
        kinds         : string ;
        baseURL       : string ;
        URL           : string ;
        backnumberURL : string ;
        tableCss      : string ;
        kaisuu        : string ;
        kaisaibi      : string ;
        hitNumber     : string ;
        chunkSize     : int ;
        truncateSize  : int
    }

    let public loto6_head:Loto = {
        kinds          = "Six"
        baseURL        = @"https://www.mizuhobank.co.jp/retail/index.html"
        URL            = @"https://www.mizuhobank.co.jp/retail/takarakuji/loto/loto6/index.html"
        backnumberURL  = @"https://www.mizuhobank.co.jp/retail/takarakuji/loto/backnumber/index.html"
        tableCss       = ""
        kaisuu         = @"table.typeTK > thead > tr > th.alnCenter.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td[colspan='6'].alnCenter"
        hitNumber      = @"table.typeTK > tbody > tr > td.alnCenter.extension > strong"
        chunkSize      = 7
        truncateSize   = 6
    }

    let public loto6_body:Loto = {
        kinds          = "SixChartA"
        baseURL        = @"https://www.mizuhobank.co.jp/retail/index.html"
        URL            = @""
        backnumberURL  = @"https://www.mizuhobank.co.jp/retail/takarakuji/loto/backnumber/index.html"
        tableCss       = @"table.typeTK > tbody > tr.js-backnumber-temp-a > td > a "
        kaisuu         = @"table.typeTK > thead > tr > th.alnCenter.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td[colspan='6'].alnCenter"
        hitNumber      = @"table.typeTK > tbody > tr > td.alnCenter.extension"
        chunkSize      = 7
        truncateSize   = 6
    }

    let public loto6_foot_new:Loto = {
        kinds          = "SixChartB_new"
        baseURL        = @"https://www.mizuhobank.co.jp/retail/index.html"
        URL            = @""
        backnumberURL  = @"https://www.mizuhobank.co.jp/retail/takarakuji/loto/backnumber/index.html"
        tableCss       = @"table.typeTK.js-backnumber-b > tbody > tr.js-backnumber-temp-b > td > a "
        kaisuu         = @"div.spTableScroll > table.typeTK > tbody > tr > th.bgf7f7f7"
        kaisaibi       = @"div.spTableScroll > table.typeTK > tbody > tr > td.alnRight"
        hitNumber      = @"div.spTableScroll > table.typeTK > tbody > tr > td[class='']"
        chunkSize      = 6
        truncateSize   = 6
    }

    let public loto6_foot_old:Loto = {
        kinds          = "SixChartB_old"
        baseURL        = @"https://www.mizuhobank.co.jp/retail/index.html"
        URL            = @""
        backnumberURL  = @"https://www.mizuhobank.co.jp/retail/takarakuji/loto/backnumber/index.html"
        tableCss       = @"table.typeTK.js-backnumber-b > tbody > tr.js-backnumber-temp-b > td > a "
        kaisuu         = @"div.spTableScroll > table.typeTK > tbody > tr > th.bgf7f7f7"
        kaisaibi       = @"div.spTableScroll > table.typeTK > tbody > tr > td.alnRight"
        hitNumber      = @"div.spTableScroll > table.typeTK > tbody > tr > td:not(.alnRight)"
        chunkSize      = 6
        truncateSize   = 6
    }



    let public loto7_head:Loto = {
        kinds          = "Seven"
        baseURL        = @"https://www.mizuhobank.co.jp/retail/index.html"
        URL            = @"http://www.mizuhobank.co.jp/takarakuji/loto/loto7/index.html"
        backnumberURL  = @"https://www.mizuhobank.co.jp/retail/takarakuji/loto/backnumber/index.html"
        tableCss       = @""
        kaisuu         = @"table.typeTK > thead > tr > th.alnCenter.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td[colspan='7'].alnCenter"
        hitNumber      = @"table.typeTK > tbody > tr > td.extension.alnCenter > strong"
        chunkSize      = 9
        truncateSize   = 7
    }

    let public loto7_body:Loto = {
        kinds          = "SevenChartA"
        baseURL        = @"https://www.mizuhobank.co.jp/retail/index.html"
        URL            = @""
        backnumberURL  = @"https://www.mizuhobank.co.jp/retail/takarakuji/loto/backnumber/index.html"
        tableCss       = @"table.typeTK > tbody > tr.js-backnumber-temp-a > td > a "
        kaisuu         = @"table.typeTK > thead > tr > th.alnCenter.bgf7f7f7"
        kaisaibi       = @"table.typeTK > tbody > tr > td[colspan='7'].alnCenter"
        hitNumber      = @"table.typeTK > tbody > tr > td.extension.alnCenter > strong"
        chunkSize      = 9
        truncateSize   = 7
    }


    let public loto7_foot:Loto = {
        kinds          = "SevenChartB"
        baseURL        = @"https://www.mizuhobank.co.jp/retail/index.html"
        URL            = @""
        backnumberURL  = @"https://www.mizuhobank.co.jp/retail/takarakuji/loto/backnumber/index.html"
        tableCss       = @"table.typeTK.js-backnumber-b > tbody > tr.js-backnumber-temp-b > td > a "
        kaisuu         = @"div.spTableScroll > table.typeTK > tbody > tr > th.bgf7f7f7"
        kaisaibi       = @"div.spTableScroll > table.typeTK > tbody > tr > td[class='alnRight js-lottery-date']"
        hitNumber      = @"div.spTableScroll > table.typeTK > tbody > tr > td[class='']"
        chunkSize      = 7
        truncateSize   = 7
    }


    type Fox () =

        let opt = new FirefoxOptions()
        do  opt.AddArgument("--headless")
        let driver = new FirefoxDriver( opt )
        let wait = WebDriverWait(driver, TimeSpan.FromSeconds(10.))

        member this.Html(url:string) =
            driver.Url <- url
            driver.PageSource

        member this.HtmlWithJS(url:string, loto:Loto) =
            try
                driver.Url <- url

                wait.Until( fun (driver:IWebDriver) ->

                    let isInnerText (d:IWebDriver): bool =
                        [
                            d.FindElements( By.CssSelector( loto.kaisuu ))
                            d.FindElements( By.CssSelector( loto.kaisaibi ))
                            d.FindElements( By.CssSelector( loto.hitNumber ))
                        ]
                        |> Seq.concat
                        |> Seq.forall ( fun (x:IWebElement) -> x.Text <> String.Empty )

                    try
                        isInnerText driver
                    with e ->
                        // Most errors are "Stale Element Reference Exception", maybe.
                        // See also: https://docs.seleniumhq.org/exceptions/stale_element_reference.jsp
                        let flg = isInnerText driver
                        if flg = false then
                            printfn "%A" e.Message
                        flg

                ) |> ignore

                driver.PageSource

            with e ->
                printfn "foo bar baz %A" e.Message
                driver.Quit()
                String.Empty

        member this.Quit() =
            driver.Quit()


    let swapRowColumn lst =
        lst
        |> List.collect List.indexed
        |> List.groupBy fst
        |> List.map snd
        |> List.map (List.map snd)


    let private listOfUrl doc (loto:Loto) =

        let absURL (bURL:string) ( url:string) = ( new Uri( ( new Uri (bURL) ) , url ) ).AbsoluteUri

        doc
        |> HtmlDocument.Parse
        |> fun doc ->
            doc.CssSelect( loto.tableCss )
            |> List.choose ( fun x -> x.TryGetAttribute("href") |> Option.map (fun a -> a.Value() |> fun url -> absURL loto.baseURL url ) )
            |> List.chunkBySize 3
            |> swapRowColumn


    let private atariImpl (loto:Loto) (doc:string) = async {

        let css =
            if loto.kinds = "SixChartB_old" then
                @"div.spTableScroll > table.typeTK > tbody > tr > td.''"
            else
                loto.hitNumber

        return
            doc
            |> HtmlDocument.Parse
            |> fun doc ->
                let index   = doc.CssSelect( loto.kaisuu )    |> List.map ( fun n -> n.InnerText() |> fun s -> String.filter Char.IsDigit s )
                let date    = doc.CssSelect( loto.kaisaibi )  |> List.map ( fun n -> n.InnerText() |> fun s -> s.Replace("年","/").Replace("月","/").Replace("日",""))
                let numbers = doc.CssSelect( css )            |> List.map ( fun n -> n.InnerText() ) |> List.chunkBySize loto.chunkSize |> List.map ( List.truncate loto.truncateSize )
                (index, date, numbers)
            |||> List.map3 ( fun a b c -> [a] @ [b] @ c )
    }


    let public Atari (loto:Loto) =

        ServicePointManager.DefaultConnectionLimit <- 100

        let f = Fox()

        let tmp =
            match loto.kinds with
            | "Six" | "Seven" ->
                atariImpl loto ( f.HtmlWithJS(loto.URL, loto) ) |> Async.RunSynchronously
            | "SixChartA" ->
                listOfUrl ( f.Html loto.backnumberURL ) loto
                |> fun l -> l.[1]
                |> List.filter( fun url -> url.Contains("loto6") )
                |> List.map( fun url -> atariImpl loto (f.HtmlWithJS(url,loto)) )
                |> Async.Parallel
                |> Async.RunSynchronously
                |> List.concat
            | "SixChartB_new" ->
                listOfUrl ( f.Html loto.backnumberURL ) loto
                |> fun l -> l.[1]
                // 空白セルよけ
                |> List.filter( fun url -> url.Contains("loto6") )
                |> List.partition( fun url -> url.Contains("detail") )
                |> fst
                |> List.map( fun url -> atariImpl loto ( f.HtmlWithJS(url,loto) ) )
                |> Async.Parallel
                |> Async.RunSynchronously
                |> List.concat
            | "SixChartB_old" ->
                listOfUrl ( f.Html loto.backnumberURL ) loto
                |> fun l -> l.[1]
                // 空白セルよけ
                |> List.filter( fun url -> url.Contains("loto6") )
                |> List.partition( fun url -> url.Contains("detail") )
                |> snd
                |> List.map( fun url -> atariImpl loto ( f.HtmlWithJS(url,loto) ) )
                |> Async.Parallel
                |> Async.RunSynchronously
                |> List.concat
            | "SevenChartA" | "SevenChartB" ->
                listOfUrl ( f.Html loto.backnumberURL ) loto
                |> fun l -> l.[2]
                // 空白セルよけ
                |> List.filter( fun s -> s.Contains("loto7") )
                |> List.map( fun url -> atariImpl loto ( f.HtmlWithJS(url,loto) ) )
                |> Async.Parallel
                |> Async.RunSynchronously
                |> List.concat
            | _ ->
                f.Quit()
                failwith "foo"

        f.Quit()
        tmp



(*
    jikken
*)

    // Atari loto6_body

    // Atari loto6_head |> printfn "%A"
    // Atari loto6_body |> printfn "%A"
    // Atari loto6_foot_new |> printfn "%A"
    // Atari loto6_foot_old |> printfn "%A"

    // Atari loto7_head |> printfn "%A"
    // Atari loto7_body |> printfn "%A"
    // Atari loto7_foot |> printfn "%A"
