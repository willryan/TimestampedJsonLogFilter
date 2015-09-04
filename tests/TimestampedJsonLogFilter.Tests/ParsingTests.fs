module TimestampedJsonLogFilter.Tests

open TimestampedJsonLogFilter.Parse
open NUnit.Framework
open FsUnit
open System
open Newtonsoft.Json.Linq

module ParsingTests =
  let mutable retDate = DateTime.Now
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
    JObjectParser = (fun s -> new JObject())
  }

  [<Test>]
  let ``filesInDir filters properly`` () =
    let files = Internal.filesInDir "foo"
    files |> should equal ["foo1.log" ; "foo2.log" ; "foo3.log"]

  [<Test>]
  let ``lineToTimeData splits and parses`` () =
    Internal.lineToTimeData "date\tdata"
    |> should equal (retDate, "data")
    let files = Internal.filesInDir "foo"
    files |> should equal ["foo1.log" ; "foo2.log" ; "foo3.log"]
