namespace TimestampedJsonLogFilter

open System
open Newtonsoft.Json.Linq

module Types =

  type LogLine = {
    Time : TimeSpan
    Data : JToken
  }

  type LogFile = {
    Filename : string
    Lines : LogLine list
  }

  type Log = {
    Files : LogFile list
  }

  type QueryTime =
    | Before of TimeSpan
    | After of TimeSpan
    | Between of TimeSpan * TimeSpan

  type QueryCondition = JToken -> bool

  type LogMap = JToken -> JToken

  type QueryWhere = {
    path : string
    condition : QueryCondition
  }

  type Query = {
    Where : QueryWhere list
    When : QueryTime option
    From : string option
  }
