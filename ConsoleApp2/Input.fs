module Input

//#r "C:\\Users\\Johann\\.nuget\\packages\\monogame.framework.desktopgl\\3.8.0.1641\\lib\\net452\\MonoGame.Framework.dll"



open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

open Types

let rmOneTail = List.rev >> List.tail >> List.rev

let keyToVec =
    [ Keys.Right, Point(1, 0)
      Keys.Left, Point(-1, 0)
      Keys.Up, Point(0, -1)
      Keys.Down, Point(0, 1) ]
    |> Map.ofList

let DirectionFromKeyboard (keyState: KeyboardState) (lastState: KeyboardState) =
    keyToVec
    |> Seq.filter (fun x -> keyState.IsKeyDown(x.Key))
    |> Seq.filter (fun x -> lastState.IsKeyUp(x.Key))
    |> Seq.sumBy (fun x -> x.Value)

let ValidateDir (dir:Point) body : Move =
    let playerPos = List.head body
    let valid =
        Drawer.field.Contains(playerPos + dir)
    match valid with
    | true -> Normal dir
    | false -> Outside
//    

let MakeMove body itemPos dir  =
    let buttonPressed = dir <> Point.Zero
    let itemEaten = List.head body + dir = itemPos

    match buttonPressed, itemEaten with
    | true, true -> Eat dir
    | true, false -> Normal dir
    | false, _ -> Still

let ApplyMove body move =
    let playerPos = List.head body

    match move with
    | Normal dir -> Hungry(playerPos + dir :: rmOneTail body)
    | Eat dir -> FedUp(playerPos + dir :: body)
    | Still -> Hungry body
