# ===========================================================================
#  FILE    : build.bash
#  AUTHOR  : callmekohei <callmekohei at gmail.com>
#  License : MIT license
# ===========================================================================

SCRIPT_PATH='./src/loto.fsx'
SCRIPT_DIR=$(cd $(dirname $0);pwd)

# check ./libSQLite.Interop.dylib
if [ ! -f "./libSQLite.Interop.dylib" ] ; then
    echo 'not exist libSQLite.Interop.dylib'
    exit
fi


# initial install library
foo="
    source https://www.nuget.org/api/v2
    nuget System.Data.SQLite
    nuget fsharp.data == 3.0.0-beta3
    nuget Selenium.webdriver
    nuget Selenium.Support
"

if [ ! -e ./packages ] ; then
    paket init
    echo "$foo" > paket.dependencies
    paket install
fi


# Create bin folder
if [ -e ./bin_lotofs ] ; then
    rm -rf ./bin_lotofs
fi

mkdir ./bin_lotofs


# Create EXE file
declare -a arr=(
   '--nologo'
   '--simpleresolution'
   # '-g'
   # '--optimize-'
    $SCRIPT_PATH
    --out:./bin_lotofs/loto.exe
)

fsharpc ${arr[@]}


# create database file ( sqlite3 file )
foo='create table loto6 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int )'
bar='create table loto7 ( id int primary key, date text , n1 int, n2 int, n3 int, n4 int, n5 int, n6 int, n7 int)'
touch ./bin_lotofs/loto.sqlite3
echo $foo | sqlite3 ./bin_lotofs/loto.sqlite3
echo $bar | sqlite3 ./bin_lotofs/loto.sqlite3


# Get path of DLL from FSharp script
cat $SCRIPT_PATH \
    | grep --color=never -e '^#r' \
    | sed  -e 's/^.*@"//g' \
           -e 's/"$//g' \
           -e 's%^\.%'$SCRIPT_DIR'%g' \
    > /tmp/std.out.$$ 2>/tmp/std.err.$$

std_out=`cat /tmp/std.out.$$`
std_err=`cat /tmp/std.err.$$`
rm -f /tmp/std.out.$$
rm -f /tmp/std.err.$$


# DLL files -> ./bin folder (copy)
if [ -n "$std_out" ] ; then

    cp $SCRIPT_DIR'/packages/FSharp.Core/lib/net45/FSharp.Core.dll' ./bin_lotofs/

    Arr_DLL_PATH=($(echo $std_out))
    len=$(( ${#Arr_DLL_PATH[@]} - 1 ))

    for i in $(seq 0 $len) ; do
        cp ${Arr_DLL_PATH[$i]} ./bin_lotofs/
    done

fi

# move other files to bin folder
cp ./libSQLite.Interop.dylib ./bin_lotofs/
