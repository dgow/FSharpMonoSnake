module ConsoleApp2.InGameLoop

open Microsoft.Xna.Framework
open Engine
open Types

let startGameLoop (game: MyCrazyGame) renderer =
    
    let rec gameLoop itemPos body = 
        async {
            let! nextState = game.LoopAsync

            match nextState with
            | Update data ->
                let movedBody = GameLogic.MakeMove itemPos body data.dir

                match movedBody with
                | Ok (FedUp newBody) -> do! gameLoop (Item.createItem ()) newBody
                | Ok (Hungry newBody) -> do! gameLoop itemPos newBody
                | Error NoInput -> do! gameLoop itemPos body
                | Error Outside -> printfn "You are Outside! GAME OVER"
                | Error HitBody -> printfn "Dont eat yourself! GAME OVER"

            | Draw time ->
                do renderer itemPos body
                do! gameLoop itemPos body
            | Input data -> failwith "todo"
        }
        
    gameLoop (Item.createItem ()) [Point.Zero]
