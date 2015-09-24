namespace TimestampedJsonLogFilter

open TimestampedJsonLogFilter.Util
open TimestampedJsonLogFilter.Types
open TimestampedJsonLogFilter.QueryConditions
open System.IO
open System
open System.Text.RegularExpressions
open Newtonsoft.Json.Linq

module Log =

  module Internal =

    let boolToOption f v =
      if (f v) then Some v else None

    let rec matchTime q tLog =
      match q with
      | Before t -> tLog < TimeSpan.FromSeconds(t)
      | After t -> tLog > TimeSpan.FromSeconds(t)
      | Between (t1, t2) -> (matchTime (After t1) tLog) && (matchTime (Before t2) tLog)

    let fileFilter (f:LogLine -> LogLine option) (l:Log) : Log =
      {
        l with
          Files =
            l.Files
            |> List.map (fun file ->
              {
                Filename = file.Filename
                Lines =
                  file.Lines
                  |> List.choose f
              }
            )
            |> List.filter (fun file -> not file.Lines.IsEmpty)
      }

    let mergeFile (lf1:LogFile) (lf2:LogFile) : LogFile =
      {
        lf1 with
          Lines = (List.append lf1.Lines lf2.Lines)
            |> List.sortBy (fun ln -> ln.Time)
      }

  let fromSources s (l:Log) : Log =
    {
      l with
        Files =
          l.Files
          |> List.filter (fun file -> Regex.Match(file.Filename, s).Success)
    }

  let fromTime (q:QueryTime)  =
    Internal.fileFilter (Internal.boolToOption (fun ln -> Internal.matchTime q ln.Time))

  let after v =
    fromTime <| After (toDbl v)

  let before v =
    fromTime <| Before (toDbl v)

  let between v1 v2 =
    fromTime <| Between ((toDbl v1), (toDbl v2))

  let whereQ (q:QueryWhere) =
    Internal.fileFilter (Internal.boolToOption (fun ln -> qWhere q ln.Data))

  let where path cond =
    whereQ { Path = path ; Condition = cond }

  let whereNotQ (q:QueryWhere) =
    Internal.fileFilter (Internal.boolToOption (fun ln -> not (qWhere q ln.Data)))

  let whereNot path cond =
    whereNotQ { Path = path ; Condition = cond }

  let grep str =
    where "$" <| QueryConditions.grep str

  let select (fs:LogMap list) =
    Internal.fileFilter (fun ln ->
      fs
      |> List.tryPick (fun f ->
        let token = f ln.Data
        if token <> null then
          Some { ln with Data = token }
        else
          None
      )
    )

  let selectPaths (paths:string list) =
    select (List.map (fun path -> (fun t -> t.SelectToken(path))) paths)

  let selectPath (path:string) =
    selectPaths [path]

  let fromDirectory = Parse.fromDirectory

  let toDirectory = Parse.toDirectory

  let combine (l1:Log) (l2:Log) : Log =
    if l1.Time <> l2.Time then
      raise (new Exception("Logs with different start times cannot be combined"))
    else
      let empty = Map.empty<string, LogFile>
      {
        l1 with
          Files =
            List.fold (fun map (lf:LogFile) ->
              if Map.containsKey lf.Filename map then
                Map.add lf.Filename (Internal.mergeFile lf (Map.find lf.Filename map)) map
              else
                Map.add lf.Filename lf map
            ) empty (List.append l1.Files l2.Files)
            |> Map.toList
            |> List.map snd
      }

  let mergeSingleton singleLog fullLog =
    let singleFile = List.head singleLog.Files
    let singleInFull = {
      fullLog with
        Files =
          fullLog.Files
          |> List.map (fun file ->
            {
              Filename = file.Filename
              Lines = singleFile.Lines
            }
          )
    }
    combine fullLog singleInFull
