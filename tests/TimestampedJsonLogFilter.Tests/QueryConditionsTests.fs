namespace TimestampedJsonLogFilter.Tests

open Xunit
open FsUnit.Xunit
open System
open Newtonsoft.Json.Linq
open TimestampedJsonLogFilter.Types
open TimestampedJsonLogFilter.QueryConditions

module QueryConditionsTests =

  let jObj = JObject.Parse("
  {
    'a': 1,
    'b': 'bar',
    'd': 10,
    'e': [2, 4, 8],
    'f': ['two', 'four', 'eight'],
    'g': {
      'subA' : { 'sub1' : 1, 'sub2' : 2},
      'subB' : { 'sub3' : 3, 'sub4' : 4},
      'subC' : { 'sub2' : 5 }
    }
  }
  ")

  [<Fact>]
  let ``Exists filters properly`` () =
    let tok = jObj.SelectToken("a")
    exists tok |> should equal true
    let tok2 = jObj.SelectToken("c")
    exists tok2 |> should equal false

  [<Fact>]
  let ``MathEquals filters properly`` () =
    let tok = jObj.SelectToken("d")
    mathEquals 10 tok |> should equal true
    mathEquals 10.1m tok |> should equal false

  [<Fact>]
  let ``Gt filters properly`` () =
    let tok = jObj.SelectToken("a")
    gt 5m tok |> should equal false
    let tok2 = jObj.SelectToken("d")
    gt 5m tok2 |> should equal true

  [<Fact>]
  let ``Lt filters properly`` () =
    let tok = jObj.SelectToken("a")
    lt 5m tok |> should equal true
    let tok2 = jObj.SelectToken("d")
    lt 5m tok2 |> should equal false

  [<Fact>]
  let ``StringEquals filters properly`` () =
    let tok = jObj.SelectToken("a")
    stringEquals "1" tok |> should equal true
    let tok2 = jObj.SelectToken("b")
    stringEquals "bar" tok2 |> should equal true
    stringEquals "foobar" tok2 |> should equal false
    stringEquals "barfoo" tok2 |> should equal false

  [<Fact>]
  let ``ContainsString filters properly`` () =
    let tok = jObj.SelectToken("b")
    stringContains "bar" tok |> should equal true
    stringContains "ba" tok |> should equal true
    stringContains "ar" tok |> should equal true
    stringContains "barf" tok |> should equal false

  [<Fact>]
  let ``arrayContains filters properly`` () =
    let tok = jObj.SelectToken("e")
    mathArrayContains 2 tok |> should equal true
    mathArrayContains 3 tok |> should equal false
    mathArrayContains 8 tok |> should equal true
    let tok2 = jObj.SelectToken("f")
    stringArrayContains "two" tok2 |> should equal true
    stringArrayContains "three" tok2 |> should equal false
    stringArrayContains "eight" tok2 |> should equal true

  [<Fact>]
  let ``boolean algebra`` () =
    let tok = jObj.SelectToken("e")
    (qAnd (mathArrayContains 4) (mathArrayContains 8)) tok |> should equal true
    (qAnd (mathArrayContains 5) (mathArrayContains 8)) tok |> should equal false
    (qOr (mathArrayContains 5) (mathArrayContains 8)) tok |> should equal true
    (qNot (qOr (mathArrayContains 5) (mathArrayContains 8))) tok |> should equal false
    (qNot (qAnd (mathArrayContains 5) (mathArrayContains 8))) tok |> should equal true
    (qAnd (mathArrayContains 4) (qNot (mathArrayContains 5))) tok |> should equal true

  [<Fact>]
  let ``qWhere lets you nest querys`` ()=
    let tok = jObj.SelectToken("g")
    tok
    |> qAnd (qWhere { Path = "subA.sub2" ; Condition = exists })
           (qWhere { Path = "subC.sub2" ; Condition = exists })
    |> should equal true

    tok
    |> qAnd (qWhere { Path = "subB.sub2" ; Condition = exists })
           (qWhere { Path = "subC.sub2" ; Condition = exists })
    |> should equal false
