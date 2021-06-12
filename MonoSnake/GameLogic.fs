module GameLogic

open Microsoft.Xna.Framework
open Types

let MakeMove itemPos body direction = 
    Input.CheckInputExists direction
    |> Result.bind (Input.CalculateNextPos body)
    |> Result.bind Input.CheckInsideField
    |> Result.bind (Input.CheckHitOwnBody body)
    |> Result.bind (Input.CreateMove itemPos)
    |> Result.bind (Input.ApplyMove body)
