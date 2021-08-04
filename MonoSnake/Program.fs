open log4net
open log4net.Config

[<EntryPoint>]
let main _ =
    printfn "Start Game"
 
    use game = new Engine.MyCrazyGame()
    let gameWorkflow = GameWorkflow.gameWorkflow game
        
    gameWorkflow |> Async.StartImmediate

    game.Run()
    
    printfn "Quit"
    0
