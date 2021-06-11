module GameLogic

open Microsoft.Xna.Framework

let MakeMove itemPos body direction =
    Input.CheckInputExists direction
    |> Option.bind (Input.CheckInsideField body)
    |> Option.bind (Input.CreateMove body itemPos)
    |> Option.bind (Input.ApplyMove body)
    