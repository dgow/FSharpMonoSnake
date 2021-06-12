module Types

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

type GameLoopData = {
    time:GameTime
    keyboard:KeyboardState
}

type LoopState =
    | Update of GameLoopData
    | Draw of GameTime

type Direction = Point

type Move =
    | Normal of Direction
    | Eat of Direction

type Body =
    | Hungry of Point list
    | FedUp of Point list

type Fail =
    | NoInput
    | Outside
    | HitBody
