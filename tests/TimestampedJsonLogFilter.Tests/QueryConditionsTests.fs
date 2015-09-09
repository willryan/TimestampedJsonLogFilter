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
    'd': 10,
    'e': [2, 4, 8]
  }
  ")

  [<Test>]
  let ``Exists filters properly`` () =
    let tok = jObj.SelectToken("a")
    exists tok |> should equal true
    let tok2 = jObj.SelectToken("c")
    exists tok2 |> should equal false

  [<Test>]
  let ``MathEquals filters properly`` () =
    let tok = jObj.SelectToken("d")
    mathEquals 10m tok |> should equal true
    mathEquals 10.1m tok |> should equal false

  [<Test>]
  let ``Gt filters properly`` () =
    let tok = jObj.SelectToken("a")
    gt 5m tok |> should equal false
    let tok2 = jObj.SelectToken("d")
    gt 5m tok2 |> should equal true

  [<Test>]
  let ``Lt filters properly`` () =
    let tok = jObj.SelectToken("a")
    lt 5m tok |> should equal true
    let tok2 = jObj.SelectToken("d")
    lt 5m tok2 |> should equal false

  [<Test>]
  let ``StringEquals filters properly`` () =
    let tok = jObj.SelectToken("a")
    stringEquals "1" tok |> should equal true
    let tok2 = jObj.SelectToken("b")
    stringEquals "bar" tok2 |> should equal true
    stringEquals "foobar" tok2 |> should equal false
    stringEquals "barfoo" tok2 |> should equal false

  [<Test>]
  let ``ContainsString filters properly`` () =
    let tok = jObj.SelectToken("b")
    containsString "bar" tok |> should equal true
    containsString "ba" tok |> should equal true
    containsString "ar" tok |> should equal true
    containsString "barf" tok |> should equal false

  [<Test>]
  let ``Contains filters properly`` () =
    let tok = jObj.SelectToken("e")
    arrayContains 2 tok |> should equal true
    arrayContains 3 tok |> should equal false
    arrayContains 8 tok |> should equal true

  [<Test>]
  let ``boolean algebra`` () =
    let tok = jObj.SelectToken("e")
    (qAnd (arrayContains 4) (arrayContains 8)) tok |> should equal true
    (qAnd (arrayContains 5) (arrayContains 8)) tok |> should equal false
    (qOr (arrayContains 5) (arrayContains 8)) tok |> should equal true
    (qNot (qOr (arrayContains 5) (arrayContains 8))) tok |> should equal false
    (qNot (qAnd (arrayContains 5) (arrayContains 8))) tok |> should equal true
    (qAnd (arrayContains 4) (qNot (arrayContains 5))) tok |> should equal true


