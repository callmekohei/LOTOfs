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
        mkdir foo
        unzip sqlite-netFx-full-source-1.0.108.0.zip -d ./foo/
        bash foo/Setup/compile-interop-assembly-release.sh
        cp foo/bin/2013/Release/bin/libSQLite.Interop.dylib ./bin/
        rm -rf foo
        rm ./sqlite-netFx-full-source-1.0.108.0.zip
    fi
)

install_lib() (
    if [ ! -e ./packages ] ; then
        paket install
    fi
)

create_db() (
    if [ ! -f ./bin/loto.sqlite3 ] ; then
        foo='create table loto6 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int )'
        bar='create table loto7 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int, n7 int)'
        touch ./bin/loto.sqlite3
        echo $foo | sqlite3 ./bin/loto.sqlite3
        echo $bar | sqlite3 ./bin/loto.sqlite3
    fi
)

create_exe_file() (
    declare -a local arr=(
        $FSX_PATH
        --nologo
        --simpleresolution
        --out:./bin/$(basename $FSX_PATH .fsx).exe
    )
    fsharpc ${arr[@]}
)

arrange_text() {
    local line
    while read -r line
    do
        echo "$line" \
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
        cp $line ./bin/
    done
}


if [ -e ./bin ] ; then
    echo 'do nothing!'
else
    mkdir ./bin
    create_dylib
    install_lib
    create_db
    create_exe_file
    cat $Lib_PATH | arrange_text | copy_dll_to_bin_folder
fi





