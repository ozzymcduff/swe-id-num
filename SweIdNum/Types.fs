namespace SweIdNum
open System
type OrganizationalIdentityNumber={OIN:string}
    with
    override this.ToString() = this.OIN

type PersonalIdentityNumber={PIN:string}
    with
    override this.ToString() = this.PIN

exception DoesNotMatchFormatException
exception InvalidChecksumException of string

type ParseMessage=
    | DoesNotMatchFormat
    | InvalidChecksum of expected:int * actual:int
    with
        static member toException msg =
            match msg with
            | DoesNotMatchFormat -> DoesNotMatchFormatException
            | InvalidChecksum (expected,actual) -> InvalidChecksumException (sprintf "Expected %i but was %i" expected actual)
        override this.ToString() =
            match this with
            | DoesNotMatchFormat -> "Does not match format"
            | InvalidChecksum (expected,actual) -> sprintf "Invalid checksum: Expected %i but was %i" expected actual