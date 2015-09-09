namespace TimestampedJsonLogFilter

open TimestampedJsonLogFilter.Types
open System.IO
open System
open System.Text.RegularExpressions
open Newtonsoft.Json.Linq

module Log =

  module Internal =
    let iter (f:'a -> 'a option) (aList:'a list) : 'a list =
      let rec itera f al =
        match al with
        | a :: l ->
          match (f a) with
          | Some b -> b :: itera f l
          | None -> itera f l
        | [] -> []
      itera f aList

    let boolToOption f v =
      if (f v) then Some v else None

    let objectToOption<'o when 'o : null and 'o : equality>(o:'o) =
      if o <> null then Some o else None

    let iterb (f:'a -> bool) (aList:'a list) : 'a list =
      iter (fun a -> boolToOption f a) aList

    let itero<'a when 'a : null and 'a : equality> (f:'a -> 'a) (aList:'a list) : 'a list =
      iter (fun a -> objectToOption (f a)) aList

    let rec matchTime q tLog =
      match q with
      | Before t -> tLog < t
      | After t -> tLog > t
      | Between (t1, t2) -> (matchTime (After t1) tLog) && (matchTime (Before t2) tLog)

    let matchWhere q (dLog:JToken) =
      let tok = dLog.SelectToken(q.Path)
      if tok <> null then
        q.Condition tok
      else
        false

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
                  |> iter f
              }
            )
            |> iterb (fun file -> not file.Lines.IsEmpty)
      }


  let fromSources s (l:Log) : Log =
    {
      l with
        Files =
          l.Files
          |> Internal.iterb (fun file -> Regex.Match(file.Filename, s).Success)
    }

  let fromTime (q:QueryTime)  =
    Internal.fileFilter (Internal.boolToOption (fun ln -> Internal.matchTime q ln.Time))

  let where (q:QueryWhere) =
    Internal.fileFilter (Internal.boolToOption (fun ln -> Internal.matchWhere q ln.Data))

  let select (f:LogMap) =
    Internal.fileFilter (fun ln ->
      let token = f ln.Data
      if token <> null then
        Some { ln with Data = token }
      else
        None
    )

  let selectPath (path:string) =
    select (fun t -> t.SelectToken(path))

  let fromDirectory (path:string) =
    Parse.fromDirectory path
  //let combine (l1:Log) (l2:Log) : Log =
  //  if l1.Time <> l2.Time then
  //    raise "Logs with different start times cannot be combined"
  //  else
  //  {
  //    l1 with
  //      Files =


