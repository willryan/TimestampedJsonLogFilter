namespace TimestampedJsonLogFilter

open System
open System.Diagnostics
open Newtonsoft.Json.Linq

module QueryConditions =

  let Exists (o:JToken) =
    o <> null

  let Cast<'T when 'T :> Object> (f:'T -> bool) (o:JToken) =
    let asObj = o :> Object
    let v = Convert.ChangeType(asObj, typeof<'T>) :?> 'T
    f v

  let Math = Cast<decimal>

  let MathEquals value = Math (fun v -> v = value)

  let Gt value = Math (fun v -> v > value)

  let Lt value = Math (fun v -> v < value)

  let String = Cast<string>

  let StringEquals value = String (fun v -> v = value)

  let ContainsString value = String (fun v -> v.Contains(value))

  let Array = Cast<JArray>

  let Not f o =
    not <| f o

  let And c1 c2 o =
    (c1 o) && (c2 o)

  let Or c1 c2 o =
    (c1 o) || (c2 o)
