# ===========================================================================
#  FILE    : build.bash
#  AUTHOR  : callmekohei <callmekohei at gmail.com>
#  License : MIT license
# ===========================================================================

#!/bin/bash

# fit your file path
FSX_PATH=./src/main_binary.fsx
Lib_PATH=./.paket/load/net471/main.group.fsx


create_dylib() (
    if [ ! -f "./bin/libSQLite.Interop.dylib" ] ; then
        wget https://system.data.sqlite.org/blobs/1.0.108.0/sqlite-netFx-full-source-1.0.108.0.zip
        mkdir ./foo/
        unzip sqlite-netFx-full-source-1.0.108.0.zip -d ./foo/
        bash foo/Setup/compile-interop-assembly-release.sh
        cp -f foo/bin/2013/Release/bin/libSQLite.Interop.dylib ./bin/
        rm -rf ./foo/
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
        | xargs wget -P ./.paket/

    mv .paket/paket.bootstrapper.exe .paket/paket.exe
}

install_lib() (

    local foo="
        generate_load_scripts: true
        source https://www.nuget.org/api/v2
        nuget System.Data.SQLite
        nuget fsharp.data == 3.0.0-beta3
        nuget Selenium.webdriver
        nuget Selenium.Support
    "

    if ! type paket >/dev/null 2>&1 ; then
        download_paket_bootstrapper
        mono ./.paket/paket.exe init
        echo "${foo}" > ./paket.dependencies
        mono ./.paket/paket.exe install
    else
        if [ ! -f ./packages/ ] ; then
            paket init
            echo "${foo}" > ./paket.dependencies
            paket install
        fi
    fi
)

create_db() (
    local foo='create table loto6 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int )'
    local bar='create table loto7 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int, n7 int)'

    touch ./loto.sqlite3
    cp -f ./loto.sqlite3 ./bin/
    rm ./loto.sqlite3

    echo "${foo}" | sqlite3 ./bin/loto.sqlite3
    echo "${bar}" | sqlite3 ./bin/loto.sqlite3
)

create_exe_file() (
    declare -a local arr=(
        "${FSX_PATH}"
        --nologo
        --simpleresolution
        --out:./bin/$(basename "${FSX_PATH}" .fsx).exe
    )
    fsharpc "${arr[@]}"
)

arrange_text() {
    local line
    while read -r line
    do
        echo "${line}" \
        | sed -e 's/#r //g' \
              -e 's/"//g'   \
        | grep --color=never -e "^\." \
        | sed -e 's|^.*packages|\./packages|g'
    done
}

copy_dll_to_bin_folder() {
    local line
    while read -r line
    do
        cp "${line}" ./bin/
    done
}


if [ -e ./bin/ ] ; then
    echo 'do nothing!'
else
    mkdir ./bin/
    create_dylib
    install_lib
    if [ "$?" = 0 ]; then
        create_db
        create_exe_file
        cat "${Lib_PATH}" | arrange_text | copy_dll_to_bin_folder
    fi
fi
