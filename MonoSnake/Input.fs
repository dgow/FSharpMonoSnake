module MonoSnake.Input

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

open Types

let rmOneTail = List.rev >> List.tail >> List.rev

let keyToVec =
    [ Keys.Right, Direction(1, 0)
      Keys.Left, Direction(-1, 0)
      Keys.Up, Direction(0, -1)
      Keys.Down, Direction(0, 1) ]
    |> Map.ofList

let GetInput (keyState: KeyboardState) (lastState: KeyboardState) =
    keyToVec
    |> Seq.filter (fun x -> keyState.IsKeyDown(x.Key))
    |> Seq.filter (fun x -> lastState.IsKeyUp(x.Key))
    |> Seq.sumBy (fun x -> x.Value)

let CheckInputExists (dir: Direction) =
    match dir with
    | x when x = Point.Zero -> Error NoInput
    | _ -> Ok dir

let CalculateNextPos body (dir:Direction) : Result<Point,Fail>  =
    let head = List.head body
    Ok (head + dir)
    
let CheckInsideField (playerNextPos : Point) =
    let isInsideField = Types.field.Contains(playerNextPos)
    match isInsideField with
    | false -> Error Outside
    | true -> Ok playerNextPos

let CheckHitOwnBody body (playerNextPos:Point)=
    let hitBody = List.contains playerNextPos (rmOneTail body) 
    match hitBody with
    | true -> Error HitBody
    | false -> Ok playerNextPos

let CreateMove itemPos playerNextPos =
    match playerNextPos = itemPos with
    | true -> Ok(Eat playerNextPos)
    | false -> Ok(Normal playerNextPos)
    
let ApplyMove body move =
    let move = match move with
                | Normal playerNextPos -> Hungry(playerNextPos :: rmOneTail body)
                | Eat playerNextPos -> FedUp(playerNextPos :: body)
    Ok move
