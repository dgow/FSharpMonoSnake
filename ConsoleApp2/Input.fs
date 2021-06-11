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

let ApplyMove body move =
    let playerPos = List.head body
    let move = match move with
                | Normal dir -> Hungry(playerPos + dir :: rmOneTail body)
                | Eat dir -> FedUp(playerPos + dir :: body)
    Some move

let CheckInputExists (dir: Point) =
    match dir with
    | x when x = Point.Zero -> None
    | _ -> Some dir

let CheckInsideField body (dir:Point) =
    let playerPos = List.head body
    let isInsideField = Drawer.field.Contains(playerPos + dir)
    
    match isInsideField with
    | false -> None
    | true -> Some dir

let CreateMove body itemPos dir =
    let itemEaten = List.head body + dir = itemPos

    match itemEaten with
    | false -> Some(Normal dir)
    | true -> Some(Eat dir)
