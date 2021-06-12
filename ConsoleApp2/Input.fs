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
    let move = match move with
                | Normal playerNextPos -> Hungry(playerNextPos :: rmOneTail body)
                | Eat playerNextPos -> FedUp(playerNextPos :: body)
    Ok move

let CheckInputExists (dir: Direction) =
    match dir with
    | x when x = Point.Zero -> Error NoInput
    | _ -> Ok dir

let CalculateNextPos body (dir:Direction) : Result<Point,Fail>  =
    Ok ((List.head body) + dir)
    
let CheckInsideField (playerNextPos : Point) =
    let isInsideField = Drawer.field.Contains(playerNextPos)
    match isInsideField with
    | false -> Error Outside
    | true -> Ok playerNextPos

let CheckHitOwnBody body (playerNextPos:Point)=
    let hitBody = List.contains playerNextPos body 
    match hitBody with
    | true -> Error HitBody
    | false -> Ok playerNextPos

let CreateMove itemPos playerNextPos =
    match playerNextPos = itemPos with
    | true -> Ok(Eat playerNextPos)
    | false -> Ok(Normal playerNextPos)
    