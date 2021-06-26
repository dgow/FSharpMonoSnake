module GameLogic

open Microsoft.Xna.Framework
open Types

let (>>=) m f = Result.bind f m

let MakeMove itemPos body direction = 
    Input.CheckInputExists direction
    >>= Input.CalculateNextPos body
    >>= Input.CheckInsideField
    >>= Input.CheckHitOwnBody body
    >>= Input.CreateMove itemPos
    >>= Input.ApplyMove body
