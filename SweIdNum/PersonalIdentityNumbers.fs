namespace SweIdNum
open System.Text.RegularExpressions

type PersonalIdentityNumber={PIN:string}

module PersonalIdentityNumbers=
    let formats = 
        [
        //format 1: "YYYYMMDDNNNC"
        //format 2: "YYYYMMDD-NNNC"
        Regex("^(18|19|20)[0-9]{2}(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-]?[0-9]{4}$")
        //format 3: "YYMMDD-NNNC"
        //format 4: "YYMMDDNNNC"
        Regex("^[0-9]{2}(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-+]?[0-9]{4}$")
        // Additional formats for old "pins" for people deceased 1947 - 1967 (i.e. ctrl numbr is missing/replaced with A,T or X)
        //format 5: "YYYYMMDDNNNC"
        // format 6: "YYYYMMDD-NNNC"
        Regex("^(18[0-9]{2}|19([0-5][0-9]|6[0-6]))(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-]?[0-9]{3}[ATX ]$")
        //format 7: "YYMMDD-NNNC"
        // format 8: "YYMMDDNNNC"
        Regex("^([0-5][0-9]|6[0-6])(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-+]?[0-9]{3}[ATX ]$")
        ]
    type ParseMessage=
        | DoesNotMatchFormat

    [<CompiledName("Parse")>]
    let parse (pin:string) : Choice<PersonalIdentityNumber, ParseMessage>=
        let matchFormat (format:Regex)=
            let m = format.Match(pin)
            if m.Success then
                Some(m)
            else
                None

        let maybeM = formats |> List.tryPick matchFormat 

        let getCaptured (captures:CaptureCollection)=
            seq{
                for c in captures do
                    yield c.Value
            }
            |> System.String.Concat
        
        match maybeM with
        | Some m ->
            Choice1Of2 {PIN=getCaptured( m.Captures ) }
        | None ->
            Choice2Of2 DoesNotMatchFormat

    [<CompiledName("IsValid")>]
    let isValid (pin:string)= 
        match parse pin with
        | Choice1Of2 _-> true
        | _ -> false

    [<CompiledName("Format")>]
    let format (format:string) (pin:PersonalIdentityNumber) = ""