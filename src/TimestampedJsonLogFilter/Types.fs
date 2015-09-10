namespace TimestampedJsonLogFilter

open System
open Newtonsoft.Json
open Newtonsoft.Json.Linq

module Types =

  [<StructuredFormatDisplay("{Format}")>]
  type LogLine = {
    Time : TimeSpan
    Data : JToken
  } with
    member x.Format = sprintf "%A\t%s" x.Time (x.Data.ToString(Formatting.None))

  type LogFile = {
    Filename : string
    Lines : LogLine list
  }

  type Log = {
    Time : DateTime
    Files : LogFile list
  }

  // in seconds
  type QueryTime =
    | Before of double
    | After of double
    | Between of double * double

  type QueryCondition = JToken -> bool

  type LogMap = JToken -> JToken

  type QueryWhere = {
    Path : string
    Condition : QueryCondition
  }

