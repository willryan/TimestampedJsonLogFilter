namespace TimestampedJsonLogFilter

open System

module Util =

  let toDbl v =
      Convert.ChangeType(v, typeof<double>) :?> double

  let tryChoose f =
    try
      let r = f()
      Choice.result r
    with
      | e -> Choice.error e
