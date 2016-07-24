namespace SweIdNum.Core
open System.Text.RegularExpressions
open SweIdNum

module OrganizationalIdentityNumber=
    let formats = 
        //format 1: "NNNNNN-NNNC"
        //format 2: "NNNNNNNNNC"
        Regex("^[0-9]{2}[2-9]{1}[0-9]{3}-?[0-9]{4}$")
    type ParseMessage=
        | DoesNotMatchFormat

    [<CompiledName("Parse")>]
    let parse (pin:string) : Choice<OrganizationalIdentityNumber, ParseMessage>=
        let m = formats.Match(pin)
        if m.Success then
            Choice1Of2 {OIN= m.Value}
        else
            Choice2Of2 DoesNotMatchFormat

    [<CompiledName("IsValid")>]
    let isValid (pin:string)= 
        match parse pin with
        | Choice1Of2 _-> true
        | _ -> false

    [<CompiledName("Format")>]
    let format (pin:OrganizationalIdentityNumber)= ""
