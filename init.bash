# ===========================================================================
#  FILE    : init.bash
#  AUTHOR  : callmekohei <callmekohei at gmail.com>
#  License : MIT license
# ===========================================================================

# create libSQLite.Interop.dylib
if [ ! -f "./src/libSQLite.Interop.dylib" ] ; then
    wget https://system.data.sqlite.org/blobs/1.0.108.0/sqlite-netFx-full-source-1.0.108.0.zip
    mkdir foo
    unzip sqlite-netFx-full-source-1.0.108.0.zip -d ./foo/
    bash foo/Setup/compile-interop-assembly-release.sh
    cp foo/bin/2013/Release/bin/libSQLite.Interop.dylib ./src/
    rm -rf foo
    rm ./sqlite-netFx-full-source-1.0.108.0.zip
fi

# initial install library
if [ ! -e ./packages ] ; then

    foo="
        source https://www.nuget.org/api/v2
        generate_load_scripts: true
        nuget System.Data.SQLite
        nuget fsharp.data == 3.0.0-beta3
        nuget Selenium.webdriver
        nuget Selenium.Support
    "

    paket init
    echo "$foo" > paket.dependencies
    paket install

fi

# create database file ( sqlite3 file )
if [ ! -f ./src/loto.sqlite3 ] ; then

    foo='create table loto6 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int )'
    bar='create table loto7 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int, n7 int)'
    touch ./src/loto.sqlite3
    echo $foo | sqlite3 ./src/loto.sqlite3
    echo $bar | sqlite3 ./src/loto.sqlite3

fi
