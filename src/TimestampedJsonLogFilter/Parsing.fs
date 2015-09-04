namespace TimestampedJsonLogFilter

open TimestampedJsonLogFilter.Types
open System.IO
open System
open Newtonsoft.Json.Linq

module Parse =

  module Internal =
    type Externals = {
      DirFileFinder : string -> string list
      LineReader : string -> string list
      DateTimeParser : string -> DateTime
      JObjectParser : string -> JObject
    }

    let mutable externals = {
      DirFileFinder =
        (fun (dir:string) ->
          (new DirectoryInfo(dir)).GetFiles()
          |> Array.map (fun fi -> fi.FullName)
          |> Array.toList
        )
      LineReader =
        (fun fn ->
          File.ReadAllLines fn
          |> Array.toList
        )
      DateTimeParser = DateTime.Parse
      JObjectParser = JObject.Parse
    }

    let filesInDir dir =
      externals.DirFileFinder dir
      |> List.filter (fun fn -> fn.EndsWith(".log"))

    let lineToTimeData (line:string) =
      match line.Split('\t') with
        | [| time ; payload |] ->
          externals.DateTimeParser(time) , externals.JObjectParser(payload)
        | _ -> raise (new Exception(sprintf "Invalid line %s" line))

    let fileToLines filename =
      let lines =
        filename
        |> externals.LineReader
        |> List.map lineToTimeData
      lines, filename

    let filesToLines files =
      List.map fileToLines files

    let linesToFileObject earliest (lines, filename) =
       {
          Filename = filename
          Lines =
            lines
            |> List.map (fun (time, data) ->
              {
                Time = time - earliest
                Data = data
              }
            )
       }

    let linesToFileObjects filesLines =
      let (earliestLines, _) =
        filesLines
        |> List.minBy (fun (lines, _) ->
          fst (List.minBy fst lines)
        )
      let earliest = fst (List.head earliestLines)
      List.map (linesToFileObject earliest) filesLines

  let FromDirectory (directory:string) : Log =
    {
      Files =
        directory
        |> Internal.filesInDir
        |> Internal.filesToLines
        |> Internal.linesToFileObjects
    }
