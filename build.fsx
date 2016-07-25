#I "packages/FAKE/tools/"
#r "FakeLib.dll"

open System
open System.IO
open System.Text.RegularExpressions
open Fake
open Fake.Testing
let libDir  = "./SweIdNum/bin/Debug/"
let testDir   = "./Tests/bin/Debug/"

let solutionFile  = "SweIdNum.sln"

Target "build" (fun _ ->
    !! solutionFile
    |> MSBuildDebug "" "Rebuild"
    |> ignore
)

Target "build_release" (fun _ ->
    !! solutionFile
    |> MSBuildRelease "" "Rebuild"
    |> ignore
)

let test ()=
    !! (Path.Combine(testDir, "*Tests*.dll"))
        |> xUnit2 (fun p -> 
            {p with
                ShadowCopy = false; 
                HtmlOutputPath = Some(testDir + "TestResults.xml")
            })


Target "test_only" (fun _ ->  test() )

Target "pack" (fun _ ->
    Paket.Pack(fun p ->
        { p with
            OutputPath = libDir})
)

Target "clean" (fun _ ->
    CleanDirs [libDir; testDir]
)

Target "push" (fun _ ->
    Paket.Push(fun p ->
        { p with
            WorkingDir = libDir })
)

Target "install" DoNothing
Target "test" (fun _ ->  test() )

"build_release"
    ==> "pack"

"build"
    ==> "test"

RunTargetOrDefault "build"