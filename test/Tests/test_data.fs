module TestData
open FSharp.Data
type Pin={Value:string}
type FakePin={Pin:Pin;Name:string}
type CsvPins = CsvProvider<"./TestData/rccvast_persnr_fejk.csv", Separators=";">
let rccvast_persnr_fejk = 
    CsvPins.Load("./TestData/rccvast_persnr_fejk.csv").Rows 
    |> Seq.map (fun r->{Pin={Value=r.Pin}; Name=r.Name})

open System.IO

let test_pin_skatteverket = 
    File.ReadLines("./TestData/test_pin_skatteverket.csv") 
    |> Seq.map (fun r->{Value=r})
