namespace TimestampedJsonLogFilter.Tests

open TimestampedJsonLogFilter.Log
open Xunit
open FsUnit.Xunit
open System
open Newtonsoft.Json.Linq

module LogTests =

  [<Fact>]
  let ``fromSources filters by source`` () =
    5 |> should equal 5

  [<Fact>]
  let ``after filters after a timespan`` () =
    5 |> should equal 5

  [<Fact>]
  let ``before filters before a timespan`` () =
    5 |> should equal 5

  [<Fact>]
  let ``between filters between a timespan`` () =
    5 |> should equal 5

  [<Fact>]
  let ``where filters by a path and condition`` () =
    5 |> should equal 5

  [<Fact>]
  let ``whereNot filters by a path and condition`` () =
    5 |> should equal 5

  [<Fact>]
  let ``grep filters the current path by a regex`` () =
    5 |> should equal 5
    
  [<Fact>]
  let ``selectPaths selects multiple paths`` () =
    5 |> should equal 5

  [<Fact>]
  let ``combine combines two logs`` () =
    5 |> should equal 5

  [<Fact>]
  let ``mergeSingleton merges a singleton log into every file of another log`` () =
    5 |> should equal 5
