namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("TimestampedJsonLogFilter")>]
[<assembly: AssemblyProductAttribute("TimestampedJsonLogFilter")>]
[<assembly: AssemblyDescriptionAttribute("filters timestamped json logs")>]
[<assembly: AssemblyVersionAttribute("0.5")>]
[<assembly: AssemblyFileVersionAttribute("0.5")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.5"
