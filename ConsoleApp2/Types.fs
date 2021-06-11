module Types

open Microsoft.Xna.Framework

type LoopState =
    | Update of GameTime
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

type M = Result<Move, Fail>
