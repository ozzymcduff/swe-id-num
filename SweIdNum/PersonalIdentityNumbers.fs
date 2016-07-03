namespace SweIdNum

type PersonalIdentityNumber(value:string)=
    let x = ""

module PersonalIdentityNumbers=
    [<CompiledName("IsValid")>]
    let isValid (pin:PersonalIdentityNumber)= false
    [<CompiledName("Format")>]
    let format (format:string) (pin:PersonalIdentityNumber) = ""