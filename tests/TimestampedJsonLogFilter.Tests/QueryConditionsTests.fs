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
    Exists tok |> should equal true
    let tok2 = jObj.SelectToken("c")
    Exists tok2 |> should equal false

  [<Test>]
  let ``MathEquals filters properly`` () =
    let tok = jObj.SelectToken("d")
    MathEquals 10m tok |> should equal true
    MathEquals 10.1m tok |> should equal false

  [<Test>]
  let ``Gt filters properly`` () =
    let tok = jObj.SelectToken("a")
    Gt 5m tok |> should equal false
    let tok2 = jObj.SelectToken("d")
    Gt 5m tok2 |> should equal true

  [<Test>]
  let ``Lt filters properly`` () =
    let tok = jObj.SelectToken("a")
    Lt 5m tok |> should equal true
    let tok2 = jObj.SelectToken("d")
    Lt 5m tok2 |> should equal false

  [<Test>]
  let ``StringEquals filters properly`` () =
    let tok = jObj.SelectToken("a")
    StringEquals "1" tok |> should equal true
    let tok2 = jObj.SelectToken("b")
    StringEquals "bar" tok2 |> should equal true
    StringEquals "foobar" tok2 |> should equal false
    StringEquals "barfoo" tok2 |> should equal false

  [<Test>]
  let ``ContainsString filters properly`` () =
    let tok = jObj.SelectToken("b")
    ContainsString "bar" tok |> should equal true
    ContainsString "ba" tok |> should equal true
    ContainsString "ar" tok |> should equal true
    ContainsString "barf" tok |> should equal false

  [<Test>]
  let ``Contains filters properly`` () =
    let tok = jObj.SelectToken("e")
    ArrayContains 2 tok |> should equal true
    ArrayContains 3 tok |> should equal false
    ArrayContains 8 tok |> should equal true

  [<Test>]
  let ``boolean algebra`` () =
    let tok = jObj.SelectToken("e")
    (And (ArrayContains 4) (ArrayContains 8)) tok |> should equal true
    (And (ArrayContains 5) (ArrayContains 8)) tok |> should equal false
    (Or (ArrayContains 5) (ArrayContains 8)) tok |> should equal true
    (Not (Or (ArrayContains 5) (ArrayContains 8))) tok |> should equal false
    (Not (And (ArrayContains 5) (ArrayContains 8))) tok |> should equal true
    (And (ArrayContains 4) (Not (ArrayContains 5))) tok |> should equal true

  
