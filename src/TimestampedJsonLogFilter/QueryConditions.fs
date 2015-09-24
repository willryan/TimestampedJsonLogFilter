namespace TimestampedJsonLogFilter

open System
open System.Diagnostics
open Newtonsoft.Json.Linq
open TimestampedJsonLogFilter.Util
open TimestampedJsonLogFilter.Types
open System.Text.RegularExpressions

module QueryConditions =

  module Internal =
    let JTokenToType<'T when 'T :> Object> (o:JToken) =
      let asObj = o :> Object
      try
        asObj :?> 'T
      with
      _ ->
        Convert.ChangeType(asObj, typeof<'T>) :?> 'T

  let exists (o:JToken) =
    o <> null

  let mathCast<'T when 'T :> Object> (f:'T -> bool) (o:JToken) =
    f (Internal.JTokenToType<'T> o)

  let dblCast = mathCast<double>

  let mathEquals value = dblCast (fun v -> v = toDbl value)

  let gt value = dblCast (fun v -> v > toDbl value)

  let lt value = dblCast (fun v -> v < toDbl value)

  let stringCast f (o:JToken) = f (string o)

  let stringEquals value = stringCast (fun v -> v = value)

  let stringContains value = stringCast (fun v -> v.Contains(value))

  let grep value = stringCast (fun v -> Regex.Match(v, value).Success)

  let arrayCast f (o:JToken) = f (o :?> JArray)

  let stringArrayContains (value:string) = arrayCast (fun v ->
    v.Children()
    |> Seq.tryFind (fun av ->
      (string)av = value
    )
    |> Option.isSome
  )

  let mathArrayContains value = arrayCast (fun v ->
    let valueD = toDbl value

    v.Children()
    |> Seq.tryFind (fun av ->
      (double)av = valueD
    )
    |> Option.isSome
  )

  let qNot f o =
    not <| f o

  let qAnd c1 c2 o =
    (c1 o) && (c2 o)

  let qOr c1 c2 o =
    (c1 o) || (c2 o)

  let qWhere q (dLog:JToken) =
    let tok = dLog.SelectToken(q.Path)
    if tok <> null then
      q.Condition tok
    else
      false
