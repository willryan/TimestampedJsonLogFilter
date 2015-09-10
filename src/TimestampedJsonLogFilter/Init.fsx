#I "bin/Release"
#r "TimestampedJsonLogFilter.dll"

open TimestampedJsonLogFilter
open TimestampedJsonLogFilter.Types
open TimestampedJsonLogFilter.QueryConditions

let dir = fsi.CommandLineArgs.[1]
let log = Log.fromDirectory dir

