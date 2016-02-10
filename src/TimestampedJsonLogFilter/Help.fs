namespace TimestampedJsonLogFilter

// NOTE: I am fully aware there are better ways of doing documentation,
// but I'm short on time and this will work okay in FSI.
module Help =

  [<StructuredFormatDisplay("{Format}")>]
  type Documentation = {
    Definition : string
    Example: string
  } with
    member x.Format = sprintf "\nDefinition:\n%s\n\nExample:\n%s" x.Definition x.Example

  let fromSources = {
    Definition = "filter log files by regex on filename"
    Example = "log |> Log.fromSources \"Event\""
  }

  let fromTime = {
    Definition = "underlying function used by after, before, and between"
    Example = "log |> Log.fromTime (Before 10.0) // must be double"
  }

  let after = {
    Definition = "include only messages after a certain time (in seconds)"
    Example = "log |> Log.after 12 // any numeric okay"
  }

  let before = {
    Definition = "include only messages before a certain time (in seconds)"
    Example = "log |> Log.before 5 // any numeric okay"
  }

  let between = {
    Definition = "include only messages between two points in time (in seconds)"
    Example = "log |> Log.between 3 12 // any numeric okay"
  }

  let whereQ = {
    Definition = "underlying function used by where"
    Example = "log |> Log.whereQ { Path = \"foo.bar\", Condition = QueryConditions.exists }"
  }

  let where = {
    Definition = "filter a log by some condition, takes a JSON path (e.g. foo.bar) and a condition to run on the data at that path (e.g. exists, gt 5)"
    Example = "log |> Log.where \"foo.bar\" (lt 10)"
  }

  let whereNotQ = {
    Definition = "underlying function used by whereNot"
    Example = "log |> Log.whereNotQ { Path = \"foo.bar\", Condition = QueryCondition.stringEquals \"hello\" }"
  }

  let whereNot = {
    Definition = "filter a log by some condition, where a path does not exist OR a condition is not true"
    Example = "log |> Log.whereNot \"foo.bar\" (mathEquals 5.0)"
  }

  let grep = {
    Definition = "filter a log by a simple search of the contents of log messages"
    Example = "log |> Log.grep \"banana\""
  }

  let select = {
    Definition = "underlying helper used by selectPath(s), takes a list of mappers and returns first match"
    Example = "log |> Log.select [JTokenMapFunc1 ; JTokenMapFunc2; (fun tok -> tok.SelectToken(\"hello.goodbye\")]"
  }

  let selectPaths = {
    Definition = "like Log.selectPath, only uses multiple paths, choosing first match"
    Example = "log |> Log.selectPaths [\"foo.bar.baz\" ; \"x.y.0\"]"
  }

  let selectPath = {
    Definition = "uses JSON path to select sub-tree of JSON log message; if the path is not present the line will be excluded"
    Example = "log |> Log.selectPath [\"first.second\"]"
  }

  let fromDirectory = {
    Definition = "loads all log files from the input directory and returns a Log record"
    Example = "let log = Log.fromDirectory \"your/path/here\""
  }

  let toDirectory = {
    Definition = "saves a log structure to an output directory. Useful after you have winnowed down a lot to a subset that identifies a problem"
    Example = "Log.toDirectory \"your/path/here\" log"
  }

  let combine = {
    Definition = "combines two logs by interleaving files with the same name. Useful if you have split a log into two separate useful sections and want to recombine"
    Example = "let combinedLog = Log.combine log1 log2"
  }
  
  let mergeSingleton = {
    Definition = "combines the first (presumed to be the only) log file in one Log into ALL files in another log. Useful if you have a global \"event\" log you wish to intersperse with other logs"
    Example = "let mergedLog = Log.mergeSingleton eventLog allLog"
  }

  let chaining = {
    Definition = "a few possible ways of combining multiple commands"
    Example = """log |> Log.fromSources "Event" |> Log.where "foo.bar" (gt 10) |> Log.selectPath "foo.baz"
log |> Log.after 30 |> Log.whereNot "bar.baz" (stringContains "Error") |> Log.toDirectory "./No_Errors"
"""
  }

  let LIST = {
    Definition = """List of help topics.
Type "Log.Help._topic_;;" to see more
For example, "Log.Help.after;;"
Type "Log.Help.LIST" to see this list
"""
    Example = """fromSources
fromTime
after
before
between
whereQ
where
whereNotQ
whereNot
grep
select
selectPaths
selectPath
fromDirectory
toDirectory
combine
mergeSingleton
chaining
"""
  }

