// ===========================================================================
//  FILE    : sqlite.fsx
//  AUTHOR  : callmekohei <callmekohei at gmail.com>
//  License : MIT license
// ===========================================================================

namespace sqlite
#r @"./packages/System.Data.SQLite.Core/lib/net46/System.Data.SQLite.dll"
open System.Data.SQLite
open System.Runtime.InteropServices

module Database =

    /// Path to libSQLite.Interop.dylib
    [<DllImport(@"./libSQLite.Interop.dylib" , CallingConvention = CallingConvention.Cdecl)>]
    let sqlite_connection : System.Data.SQLite.SQLiteConnection  =
        /// Path to sqlite3
        ( new SQLiteConnection( @"Data Source=./loto.sqlite3;Version=3;foreign keys=true" ))

    type SQ3 ( connection : System.Data.SQLite.SQLiteConnection ) =

        member this.sqlite_open : unit =
            connection.Open()

        member this.sqlite_close : unit =
            connection.Close()

        member this.sqlite_createTable sql  =
            ( new SQLiteCommand(sql, connection)).ExecuteNonQuery() |> ignore

        member this.sqlite_insert  sql =
            ( new SQLiteCommand(sql, connection)).ExecuteNonQuery() |> ignore

        member this.sqlite_select  sql f =
            let reader = ( new SQLiteCommand(sql, connection )).ExecuteReader()
            while ( reader.Read() ) do
               f reader
