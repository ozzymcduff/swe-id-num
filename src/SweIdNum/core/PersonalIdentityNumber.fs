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
        (Regex("^(18|19|20)[0-9]{2}(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-]?[0-9]{4}$",RegexOptions.IgnorePatternWhitespace), Full, Normal)
        //format 3: "YYMMDD-NNNC"
        //format 4: "YYMMDDNNNC"
        (Regex("^[0-9]{2}(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-+]?[0-9]{4}$",RegexOptions.IgnorePatternWhitespace), Short, Normal)
        // Additional formats for old "pins" for people deceased 1947 - 1967 (i.e. ctrl numbr is missing/replaced with A,T or X)
        //format 5: "YYYYMMDDNNNC"
        // format 6: "YYYYMMDD-NNNC"
        (Regex("^(18[0-9]{2}|19([0-5][0-9]|6[0-6]))(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-]?[0-9]{3}[ATX ]$",RegexOptions.IgnorePatternWhitespace), Full, Deceased_in_1947_1967)
        //format 7: "YYMMDD-NNNC"
        // format 8: "YYMMDDNNNC"
        (Regex("^([0-5][0-9]|6[0-6])(0[1-9]|1[0-2])([06][1-9]|[1278][0-9]|[39][0-1])[-+]?[0-9]{3}[ATX ]$",RegexOptions.IgnorePatternWhitespace), Short, Deceased_in_1947_1967)
        ]

    let private minusOrPlus = Regex("[+-]")
    let private centuryOfDate (v:DateTime)=
        (v.Year/100)
    
    [<Extension>]
    [<CompiledName("GetDate")>]
    let getDate (pin:PersonalIdentityNumber) = 
        let provider =System.Globalization.CultureInfo.InvariantCulture
        DateTime.ParseExact(pin.PIN.Substring(0,8), "yyyyMMdd", provider)

    let private atx = Regex(".+[ATX ]$")
    /// Additional formats for old "pins" for people deceased 1947 - 1967 (i.e. ctrl numbr is missing/replaced with A,T, X or space)
    [<Extension>]
    [<CompiledName("OldFormat")>]
    let oldFormat (pin:PersonalIdentityNumber)=
        let date = getDate pin
        date.Year<=1967 && atx.IsMatch(pin.PIN)
        
    let private control (pin:PersonalIdentityNumber)=
        oldFormat pin || Luhn.is_luhn_valid( pin.PIN.Substring(2,10) )

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
            let pin = {PIN= prefix+value }
            if not (control pin) then
                let checksum = Luhn.calculate_luhn ( pin.PIN.Substring(2,9))
                let actual = Int32.Parse (pin.PIN.Substring(pin.PIN.Length-1,1))
                Choice2Of2 (InvalidChecksum (expected=checksum, actual=actual))
            else 
                Choice1Of2 pin
        | None ->
            Choice2Of2 DoesNotMatchFormat

    [<CompiledName("Parse")>]
    let parse (pin:string) =
        match tryParse pin with
        | Choice1Of2 pin->pin
        | Choice2Of2 err->raise (ParseMessage.toException err)

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

    /// According to the pin the legal gender is considered to be female
    [<Extension>]
    [<CompiledName("IsFemale")>]
    let isFemale (pin:PersonalIdentityNumber) = 
        System.Int16.Parse( pin.PIN.Substring(pin.PIN.Length-2,1))% 2s = 0s

    /// According to the pin the legal gender is considered to be male
    [<Extension>]
    [<CompiledName("IsMale")>]
    let isMale (pin:PersonalIdentityNumber) = 
        not (isFemale pin)

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
            "NNNC", (fun ()->pin.PIN.Substring(8,4))
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
