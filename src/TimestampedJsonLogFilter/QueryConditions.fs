namespace TimestampedJsonLogFilter

open System
open System.Diagnostics
open Newtonsoft.Json.Linq
open TimestampedJsonLogFilter.Types

module QueryConditions =

  module Internal =
    let JTokenToType<'T when 'T :> Object> (o:JToken) =
      let asObj = o :> Object
      Convert.ChangeType(asObj, typeof<'T>) :?> 'T

  let exists (o:JToken) =
    o <> null

  let cast<'T when 'T :> Object> (f:'T -> bool) (o:JToken) =
    f (Internal.JTokenToType<'T> o)

  let mathCast = cast<decimal>

  let mathEquals value = mathCast (fun v -> v = value)

  let gt value = mathCast (fun v -> v > value)

  let lt value = mathCast (fun v -> v < value)

  let stringCast = cast<string>

  let stringEquals value = stringCast (fun v -> v = value)

  let containsString value = stringCast (fun v -> v.Contains(value))

  let arrayCast = cast<JArray>

  let arrayContains<'T when 'T : equality> (value:'T) = arrayCast (fun v ->
    v.Children()
    |> Seq.tryFind (fun o ->
      let converted = Internal.JTokenToType o
      converted = value
      )
    |> Option.isSome
    )

  let qNot f o =
    not <| f o

  let qAnd c1 c2 o =
    (c1 o) && (c2 o)

  let qOr c1 c2 o =
    (c1 o) || (c2 o)

  let matchWhere q (dLog:JToken) =
    let tok = dLog.SelectToken(q.Path)
    if tok <> null then
      q.Condition tok
    else
      false

