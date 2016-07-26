namespace SweIdNum.Core
open System.Text.RegularExpressions
open SweIdNum
open System.Runtime.CompilerServices
open System

[<Extension>]
module OrganizationalIdentityNumbers=
    let private formats = 
        //format 1: "NNNNNN-NNNC"
        //format 2: "NNNNNNNNNC"
        Regex("^([0-9]{2}[2-9]{1}[0-9]{3})-?([0-9]{4})$")

    let private minus = Regex("[-]")
    let private replaceMinus v= minus.Replace(v, "")

    [<CompiledName("FSharpTryParse")>]
    let tryParse (pin:string)=

        let m = formats.Match pin
        if m.Success then
            Choice1Of2 {OIN= String.Join("-", [| m.Groups.[1].Value; m.Groups.[2].Value |])}
        else
            Choice2Of2 DoesNotMatchFormat

    [<CompiledName("TryParse")>]
    let chsarpTryParse (pin:string, [<System.Runtime.InteropServices.Out>]value:OrganizationalIdentityNumber byref) : bool=
        match tryParse pin with
        | Choice1Of2 pin -> 
            value<-pin
            true
        | Choice2Of2 err -> 
            false

    [<CompiledName("Parse")>]
    let parse (pin:string) =
        match tryParse pin with
        | Choice1Of2 pin->pin
        | Choice2Of2 err->raise (ParseException err)

    [<CompiledName("IsValid")>]
    let isValid (pin:string)= 
        match tryParse pin with
        | Choice1Of2 _-> true
        | _ -> false

    [<Extension>]
    [<CompiledName("Control")>]
    let control (pin:OrganizationalIdentityNumber)=
        let valid_luhn = Luhn.is_luhn_valid(Int64.Parse(replaceMinus pin.OIN))
        valid_luhn
