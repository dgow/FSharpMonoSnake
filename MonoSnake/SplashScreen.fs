module ConsoleApp2.SplashScreen
open Engine
open Types

let startGameLoop (game: MyCrazyGame) renderer text =
    let rec splashLoop() =
        async {
            let! nextState = game.LoopAsync
            renderer text
            
            match nextState with
            | Update data -> printfn "Lets go!"
            | _ -> do! splashLoop() 
        }
        
    splashLoop() 
    
