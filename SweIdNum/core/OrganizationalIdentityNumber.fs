namespace SweIdNum.Core
open System.Text.RegularExpressions
open SweIdNum
open System.Runtime.CompilerServices

[<Extension>]
module OrganizationalIdentityNumbers=
    let private formats = 
        //format 1: "NNNNNN-NNNC"
        //format 2: "NNNNNNNNNC"
        Regex("^[0-9]{2}[2-9]{1}[0-9]{3}-?[0-9]{4}$")
    type ParseMessage=
        | DoesNotMatchFormat

    let private minus = Regex("[-]")

    [<CompiledName("TryParse")>]
    let tryParse (pin:string) : Choice<OrganizationalIdentityNumber, ParseMessage>=
        let replaceMinus v=
            minus.Replace(v, "")

        let m = formats.Match pin

        if m.Success then

            Choice1Of2 {OIN= replaceMinus m.Value}
        else
            Choice2Of2 DoesNotMatchFormat

    [<CompiledName("IsValid")>]
    let isValid (pin:string)= 
        match tryParse pin with
        | Choice1Of2 _-> true
        | _ -> false

    [<CompiledName("ToString")>]
    let toString (pin:OrganizationalIdentityNumber)= 
        pin.ToString()
