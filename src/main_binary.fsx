// ===========================================================================
//  FILE    : main_binary.fs
//  AUTHOR  : callmekohei <callmekohei at gmail.com>
//  License : MIT license
// ===========================================================================

#load "./loto.fsx"
open Loto.Util

[<EntryPointAttribute>]
let main argv =
    let v:Loto = if    Array.isEmpty argv || argv.[0] <> "7"
                 then  loto6
                 else  loto7

    idea04 v 5
    |> Seq.fold ( fun acc l -> prettyPrint " " l + "\n" + acc ) ""
    |> stdout.WriteLine

    0
