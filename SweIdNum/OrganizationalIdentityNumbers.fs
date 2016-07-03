namespace SweIdNum

type OrganizationalIdentityNumber()=
    let x = ""

module OrganizationalIdentityNumbers=
    [<CompiledName("IsValid")>]
    let isValid (pin:OrganizationalIdentityNumber)= false
    [<CompiledName("Format")>]
    let format (pin:OrganizationalIdentityNumber)= ""
