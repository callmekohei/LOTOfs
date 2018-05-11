# ===========================================================================
#  FILE    : init.bash
#  AUTHOR  : callmekohei <callmekohei at gmail.com>
#  License : MIT license
# ===========================================================================

create_dylib() (
    if [ ! -f "./src/libSQLite.Interop.dylib" ] ; then
        wget https://system.data.sqlite.org/blobs/1.0.108.0/sqlite-netFx-full-source-1.0.108.0.zip
        mkdir foo/
        unzip sqlite-netFx-full-source-1.0.108.0.zip -d ./foo/
        bash foo/Setup/compile-interop-assembly-release.sh
        cp foo/bin/2013/Release/bin/libSQLite.Interop.dylib ./src/
        rm -rf foo/
        rm ./sqlite-netFx-full-source-1.0.108.0.zip
    fi
)

# see also
# Getting Started with Paket > Manual setup
# https://fsprojects.github.io/Paket/getting-started.html#Manual-setup
function download_paket_bootstrapper(){

    if ! type jq >/dev/null 2>&1 ; then
        echo 'Please install jq'
        return -1
        exit
    fi

    curl -i "https://api.github.com/repos/fsprojects/Paket/releases" \
        | jq '.[]' \
        | jq '.[0].assets[].browser_download_url' \
        | grep 'paket.bootstrapper.exe' \
        | xargs wget -P .paket

    mv .paket/paket.bootstrapper.exe .paket/paket.exe
}

install_lib() (

    local foo="
        source https://www.nuget.org/api/v2
        generate_load_scripts: true
        nuget System.Data.SQLite
        nuget fsharp.data == 3.0.0-beta3
        nuget Selenium.webdriver
        nuget Selenium.Support
    "

    if ! type paket >/dev/null 2>&1 ; then
        download_paket_bootstrapper
        mono ./.paket/paket.exe init
        echo "$foo" > ./paket.dependencies
        mono ./.paket/paket.exe install
    else
        if [ ! -f ./packages/ ] ; then
            paket init
            echo "$foo" > ./paket.dependencies
            paket install
        fi
    fi
)

create_db() (
    if [ ! -f ./src/loto.sqlite3 ] ; then

        local foo='create table loto6 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int )'
        local bar='create table loto7 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int, n7 int)'
        touch ./src/loto.sqlite3
        echo $foo | sqlite3 ./src/loto.sqlite3
        echo $bar | sqlite3 ./src/loto.sqlite3
    fi
)

create_dylib
install_lib
if [ $? = 0 ] ; then
    create_db
fi
