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

  type QueryTime =
    | Before of TimeSpan
    | After of TimeSpan
    | Between of TimeSpan * TimeSpan

  type QueryCondition = JToken -> bool

  type LogMap = JToken -> JToken

  type QueryWhere = {
    Path : string
    Condition : QueryCondition
  }

