module GameLogic

open Microsoft.Xna.Framework
open MonoSnake

let (>>=) m f = Result.bind f m

let MakeMove itemPos body direction = 
    Input.CalculateNextPos body direction
    >>= Input.CheckInsideField
    >>= Input.CheckHitOwnBody body
    >>= Input.CreateMove itemPos
    >>= Input.ApplyMove body
    
