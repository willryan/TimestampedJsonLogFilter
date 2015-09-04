namespace TimestampedJsonLogFilter

open System
open Newtonsoft.Json.Linq

module Types =

  type LogLine = {
    Time : TimeSpan
    Data : JObject
  }

  type NodeIdentifier = {
    Module : string
    CsuGroup : string
    CsuNodeId : string
  }

  type LogFilename =
    | Initial
    | Events
    | Node of NodeIdentifier

  type LogFile = {
    Filename : LogFilename
    Lines : LogLine list
  }

  type Log = {
    Files : LogFile list
  }

  type QueryTime =
    | Before of TimeSpan
    | After of TimeSpan
    | Between of TimeSpan * TimeSpan

  type QueryWhereOperator =
    | Is of string
    | IsNot of string
    | Contains of string
    | DoesNotContain of string
    | GreaterThan of double
    | LessThan of double

  type QueryWhere = {
    path : string
    condition : QueryWhereOperator option
  }

  type Query = {
    Where : QueryWhere list
    When : QueryTime option
    From : LogFilename option
  }
