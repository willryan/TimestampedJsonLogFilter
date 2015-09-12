namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("TimestampedJsonLogFilter")>]
[<assembly: AssemblyProductAttribute("TimestampedJsonLogFilter")>]
[<assembly: AssemblyDescriptionAttribute("filters timestamped json logs")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
