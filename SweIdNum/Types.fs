namespace SweIdNum
open System
type OrganizationalIdentityNumber={OIN:string}
    with
    override this.ToString() = this.OIN

type PersonalIdentityNumber={PIN:string}
    with
    override this.ToString() = this.PIN


type ParseMessage=
    | DoesNotMatchFormat
type ParseException(msg:ParseMessage)=
    inherit Exception(msg.ToString())

    member this.Message = msg
