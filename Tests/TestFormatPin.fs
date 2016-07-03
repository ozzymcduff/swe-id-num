module TestFormatPin

open Xunit.Sdk
type XData()=
    inherit DataAttribute()
    override this.GetData (testMethod)=
        [
            "191212121212"; "201212121212"; "191212121212"; "201212121212"; 
                        "199106252523"; "189611070798"; "190011121298"; "200809050523";
                        "189101252598"; "201401232323"
        ] |> Seq.map (fun v-> [| (v :> obj) |] )

open Xunit
open FsUnit.Xunit
open SweIdNum
        
[<Theory>]
[<XData()>]
let FormatPin value =
    let maybePin = PersonalIdentityNumbers.parse(value)
    match maybePin with 
    | Choice1Of2 pin->
        pin |> PersonalIdentityNumbers.format "%N" |> should equal "1212"
        pin |> PersonalIdentityNumbers.format "%P" |> should equal "(19) 12-12-12 - 1212"
        pin |> PersonalIdentityNumbers.format "%Y-%m-%d-%N" |> should equal "1912-12-12-1212"
    | Choice2Of2 e->
        failwithf "%A" e