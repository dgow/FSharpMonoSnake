module Types

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

let rectSize = 50
let screenWidth = 12
let screenHeight = 8
let margin = 2

let field = Rectangle(0,0, screenWidth, screenHeight)
type Direction = Point

type GameLoopData = {
    time:GameTime
    dir:Direction
}


type LoopState =
    | Update of GameLoopData
    | Draw of GameTime
    | Input of Direction

type Move =
    | Normal of Point
    | Eat of Point

type Body =
    | Hungry of Point list
    | FedUp of Point list

type Fail =
    | NoInput
    | Outside
    | HitBody
