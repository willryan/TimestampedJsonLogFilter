namespace TimestampedJsonLogFilter.Tests

open TimestampedJsonLogFilter.QueryConditions
open NUnit.Framework
open FsUnit
open System
open Newtonsoft.Json.Linq

module QueryConditionsTests =

  let jObj = JObject.Parse("
  {
    'a': 1,
    'b': 'bar',
    'd': 10
  }
  ")

  [<Test>]
  let ``Exists filters properly`` () =
    let tok = jObj.SelectToken("a")
    Exists tok
    |> should equal true
    let tok2 = jObj.SelectToken("c")
    Exists tok2
    |> should equal false

  [<Test>]
  let ``MathEquals filters properly`` () =
    let tok = jObj.SelectToken("d")
    MathEquals 10m tok
    |> should equal true
    MathEquals 10.1m tok 
    |> should equal false
