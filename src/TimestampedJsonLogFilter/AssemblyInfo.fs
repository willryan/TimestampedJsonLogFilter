namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("TimestampedJsonLogFilter")>]
[<assembly: AssemblyProductAttribute("TimestampedJsonLogFilter")>]
[<assembly: AssemblyDescriptionAttribute("filters timestamped json logs")>]
[<assembly: AssemblyVersionAttribute("0.8")>]
[<assembly: AssemblyFileVersionAttribute("0.8")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.8"
