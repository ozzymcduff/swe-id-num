namespace SweIdNum


module PersonalIdentityNumber=
    [<CompiledName("IsValid")>]
    let isValid (pin:string)= false
    [<CompiledName("Format")>]
    let format (pin:string)= ""