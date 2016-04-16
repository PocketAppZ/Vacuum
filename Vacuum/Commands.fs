﻿module Vacuum.Commands

open CommandLine

[<Verb("clean", HelpText = "Clean the temporary directory.")>]
type Clean =
    { [<Option('d',
               "directory",
               HelpText = "Temporary directory path. Will be calculated if not defined.")>]
      Directory : string option

      [<Option('p',
               "period",
               HelpText = "Number of days for item to stay untouched before being cleaned up. Default period is 30 days.")>]
      Period : int option

      [<Option('s',
               "space",
               HelpText = "Amount of space to be freed disregard the dates. Off by default. Supports k and m postfix. For example, 10k = 10 kibibytes, 10m = 10 mebibytes")>]
      Space : string option }

    member this.BytesToFree =
        let allButLast (s : string) = s.Substring (0, s.Length - 1)
        match this.Space with
        | Some space when space.ToLowerInvariant().EndsWith("k") -> Some (1024L * (int64 <| allButLast space))
        | Some space when space.ToLowerInvariant().EndsWith("m") -> Some (1024L * 1024L * (int64 <| allButLast space))
        | Some space -> Some (int64 space)
        | None -> None