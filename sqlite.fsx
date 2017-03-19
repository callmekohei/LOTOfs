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

        let cn = connection

        member this.sqlite_open : unit =
            cn.Open()
        
        member this.sqlite_close : unit =
            cn.Close()

        member this.sqlite_createTable sql  =
            ( new SQLiteCommand(sql, cn)).ExecuteNonQuery() |> ignore

        member this.sqlite_insert  sql =
            ( new SQLiteCommand(sql, cn)).ExecuteNonQuery() |> ignore

        member this.sqlite_select  sql f =
            let reader = ( new SQLiteCommand(sql, cn )).ExecuteReader()
            while ( reader.Read() ) do
               f reader 
