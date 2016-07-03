namespace SweIdNum
open System.Text.RegularExpressions
type OrganizationalIdentityNumber={OIN:string}

module OrganizationalIdentityNumbers=
    let formats = 
        //format 1: "NNNNNN-NNNC"
        //format 2: "NNNNNNNNNC"
        Regex("^([0-9]{2}[2-9]{1}[0-9]{3})-?([0-9]{4})$")
    type ParseMessage=
        | DoesNotMatchFormat

    [<CompiledName("Parse")>]
    let parse (pin:string) : Choice<OrganizationalIdentityNumber, ParseMessage>=
        let m = formats.Match(pin)
        let getCaptured (captures:CaptureCollection)=
            seq{
                for c in captures do
                    yield c.Value
            }
            |> System.String.Concat

        if m.Success then
            Choice1Of2 {OIN=getCaptured( m.Captures ) }
        else
            Choice2Of2 DoesNotMatchFormat

    [<CompiledName("IsValid")>]
    let isValid (pin:string)= 
        match parse pin with
        | Choice1Of2 _-> true
        | _ -> false

    [<CompiledName("Format")>]
    let format (pin:OrganizationalIdentityNumber)= ""
