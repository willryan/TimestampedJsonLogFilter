namespace TimestampedJsonLogFilter

open System

module Util =

  let toDbl v =
      Convert.ChangeType(v, typeof<double>) :?> double
