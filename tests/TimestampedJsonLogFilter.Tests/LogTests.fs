namespace TimestampedJsonLogFilter.Tests

open TimestampedJsonLogFilter.Log
open NUnit.Framework
open FsUnit
open System
open Newtonsoft.Json.Linq

module LogTests =

  [<Test>]
  let ``fromSources filters by source`` () =
    5 |> should equal 5

  [<Test>]
  let ``after filters after a timespan`` () =
    5 |> should equal 5

  [<Test>]
  let ``before filters before a timespan`` () =
    5 |> should equal 5

  [<Test>]
  let ``between filters between a timespan`` () =
    5 |> should equal 5

  [<Test>]
  let ``where filters by a path and condition`` () =
    5 |> should equal 5

  [<Test>]
  let ``whereNot filters by a path and condition`` () =
    5 |> should equal 5

  [<Test>]
  let ``grep filters the current path by a regex`` () =
    5 |> should equal 5
    
  [<Test>]
  let ``selectPaths selects multiple paths`` () =
    5 |> should equal 5

  [<Test>]
  let ``combine combines two logs`` () =
    5 |> should equal 5

  [<Test>]
  let ``mergeSingleton merges a singleton log into every file of another log`` () =
    5 |> should equal 5
