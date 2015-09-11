namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("TimestampedJsonLogFilter")>]
[<assembly: AssemblyProductAttribute("TimestampedJsonLogFilter")>]
[<assembly: AssemblyDescriptionAttribute("filters timestamped json logs")>]
[<assembly: AssemblyVersionAttribute("0.1")>]
[<assembly: AssemblyFileVersionAttribute("0.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.1"
