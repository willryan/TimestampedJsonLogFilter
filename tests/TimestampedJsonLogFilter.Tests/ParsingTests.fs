namespace TimestampedJsonLogFilter.Tests

open TimestampedJsonLogFilter.Parse
open Xunit
open FsUnit.Xunit
open System
open Newtonsoft.Json.Linq
open ExtCore.Control

module ParsingTests =
  let mutable retDate = DateTime.Now
  let mutable parsedObject = new JObject()

  let setExternals() =
    Internal.externals <- {
      DirFileFinder =
        (fun d ->
          [
            (sprintf "%s1.log" d)
            (sprintf "%s2.log" d)
            (sprintf "%s2.nope" d)
            (sprintf "%s3.log" d)
            (sprintf "%s4.nope" d)
          ]
        )
      LineReader = (fun fn ->
        match fn with
        | "file1.log" ->
          [
            "time\tdata"
          ]
        | "file2.log" ->
          [
            "time\tdata"
          ]
        | _ -> []
      )
      DateTimeParser = (fun dts -> retDate)
      JObjectParser = (fun s -> parsedObject)
      FileWriter = (fun s l -> true)
    }

  [<Fact>]
  let ``filesInDir filters properly`` () =
    setExternals()
    let files = Internal.filesInDir "foo"
    files |> should equal ["foo1.log" ; "foo2.log" ; "foo3.log"]

  [<Fact>]
  let ``lineToTimeData splits and parses`` () =
    setExternals()
    Internal.lineToTimeData "date\tdata"
    |> should equal (Some (retDate, parsedObject))

  [<Fact>]
  let ``lineToTimeDataRaw split error`` () =
    setExternals()
    let err = Internal.lineToTimeDataRaw "date,data" |> Choice.getError
    err.Message |> should equal "no tab"

  [<Fact>]
  let ``lineToTimeDataRaw date error`` () =
    setExternals()
    Internal.externals <- { Internal.externals with DateTimeParser = DateTime.Parse }
    let err = Internal.lineToTimeDataRaw "date\tdata" |> Choice.getError
    err.Message |> should haveSubstring "string was not recognized as a valid DateTime."

  [<Fact>]
  let ``lineToTimeDataRaw payload error`` () =
    setExternals()
    Internal.externals <- { Internal.externals with JObjectParser = JObject.Parse }
    let err = Internal.lineToTimeDataRaw "date\t{'unended'" |> Choice.getError
    err.Message |> should startWith "Invalid character"
