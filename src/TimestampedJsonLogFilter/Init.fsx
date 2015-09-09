#r "TimestampedJsonLogFilter.dll"

open TimestampedJsonLogFilter
open TimestampedJsonLogFilter.Types
open TimestampedJsonLogFilter.QueryConditions

let log = Log.fromDirectory fsi.CommandLineArgs.[1]

