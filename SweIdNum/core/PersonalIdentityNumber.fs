namespace SweIdNum.Core
open System.Text.RegularExpressions
open System
open SweIdNum
open System.Runtime.CompilerServices

[<Extension>]
module PersonalIdentityNumbers=
    type private DateFormat=
        | Short
        | Full
    type private PinType=
        | Normal
        | Deceased_in_1947_1967

    let private formats = 
        [
        //format 1: "YYYYMMDDNNNC"
        //format 2: "YYYYMMDD-NNNC"
        (Regex("^(18|19|20)[0-9]{2}(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-]?[0-9]{4}$"), Full, Normal)
        //format 3: "YYMMDD-NNNC"
        //format 4: "YYMMDDNNNC"
        (Regex("^[0-9]{2}(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-+]?[0-9]{4}$"), Short, Normal)
        // Additional formats for old "pins" for people deceased 1947 - 1967 (i.e. ctrl numbr is missing/replaced with A,T or X)
        //format 5: "YYYYMMDDNNNC"
        // format 6: "YYYYMMDD-NNNC"
        (Regex("^(18[0-9]{2}|19([0-5][0-9]|6[0-6]))(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-]?[0-9]{3}[ATX ]$"), Full, Deceased_in_1947_1967)
        //format 7: "YYMMDD-NNNC"
        // format 8: "YYMMDDNNNC"
        (Regex("^([0-5][0-9]|6[0-6])(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-+]?[0-9]{3}[ATX ]$"), Short, Deceased_in_1947_1967)
        ]

    let private minusOrPlus = Regex("[+-]")
    let private centuryOfDate (v:DateTime)=
        (v.Year/100)

    [<CompiledName("FSharpTryParse")>]
    let tryParse (pin:string) =
        let matchFormat (format:Regex*'a*'b)=
            let r,a,b=format
            let m = r.Match(pin)
            if m.Success then
                Some(m,a,b)
            else
                None

        let replaceMinusAndPlus v=
            minusOrPlus.Replace(v, "")

        let maybeM = formats |> List.tryPick matchFormat 

        let date (v:string)=
            let provider =System.Globalization.CultureInfo.InvariantCulture
            DateTime.ParseExact(v.Substring(0,6), "yyMMdd", provider)

        match maybeM with
        | Some (v,df,pt) ->
            let value = replaceMinusAndPlus(v.Value)
            let hasPlus = v.Value.Contains("+")
            let prefix = 
                match df,pt with
                | Short,_ -> 
                    let d = date value
                    let century = 
                        if  d> System.DateTime.Now then 
                            ((centuryOfDate d)-1)
                        else
                            (centuryOfDate d)
                    if hasPlus then 
                        (century - 1).ToString() 
                    else 
                        century.ToString()
                | Full ,_ -> ""

            Choice1Of2 {PIN= prefix+value }
        | None ->
            Choice2Of2 DoesNotMatchFormat

    [<CompiledName("Parse")>]
    let parse (pin:string) =
        match tryParse pin with
        | Choice1Of2 pin->pin
        | Choice2Of2 err->raise (ParseException err)

    [<CompiledName("TryParse")>]
    let chsarpTryParse (pin:string, [<System.Runtime.InteropServices.Out>]value:PersonalIdentityNumber byref) : bool=
        match tryParse pin with
        | Choice1Of2 pin -> 
            value<-pin
            true
        | Choice2Of2 err -> 
            false

    [<CompiledName("FsharpTryParseNumeric")>]
    let parseNumeric (pin:int64)=
        tryParse (pin.ToString("0000000000"))

    [<CompiledName("IsValid")>]
    let isValid (pin:string)= 
        match tryParse pin with
        | Choice1Of2 _-> true
        | _ -> false

    [<Extension>]
    [<CompiledName("GetDate")>]
    let getDate (pin:PersonalIdentityNumber) = 
        let provider =System.Globalization.CultureInfo.InvariantCulture
        DateTime.ParseExact(pin.PIN.Substring(0,8), "yyyyMMdd", provider)

    [<Extension>]
    [<CompiledName("GetControlNumber")>]
    let getControlNumber (pin:PersonalIdentityNumber) = 
        pin.PIN.Substring(8,4)

    [<Extension>]
    [<CompiledName("Control")>]
    let control (pin:PersonalIdentityNumber)=
        let valid_luhn = Luhn.is_luhn_valid(Int64.Parse( pin.PIN.Substring(2,10)))
        let date = getDate pin
        let atx = Regex("*[ATX]$")
        let old_pin_format = date.Year<=1967 && atx.IsMatch(pin.PIN)
        valid_luhn || old_pin_format

    [<CompiledName("Format")>]
    let format (format:string) (pin:PersonalIdentityNumber)= 
        let date = getDate pin
        let replacements = [
            "P", (fun ()->"(cc) yy-mm-dd - NNNC")
            "yyyy", (fun ()->date.ToString("yyyy"))
            "yy", (fun ()->date.ToString("yy") )
            "cc", (fun ()->(centuryOfDate date).ToString())
            "mm", (fun ()->date.ToString("MM"))
            "dd", (fun ()->date.ToString("dd"))
            "NNNC", (fun ()->getControlNumber pin)
        ]
        let mutable result =format
        for replacement in replacements do
            let c = fst replacement
            let r = snd replacement
            let idx= result.IndexOf c
            if idx>=0 then
                result <- result.Replace(c, r())
            else
                ()
        result
    
    let private formatPin = format

    [<Extension>]
    [<CompiledName("ToString")>]
    let toString (pin:PersonalIdentityNumber, format:string) = 
        formatPin format pin
