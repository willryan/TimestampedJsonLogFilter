// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Types.fs"
#load "Parsing.fs"
open TimestampedJsonLogFilter

let log = Parse.FromDirectory "."
printfn "log %A" log
