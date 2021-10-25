module GameWorkflow

open ConsoleApp2
open Engine

let gameWorkflow (game: MyCrazyGame) =
    async {
        do! game.InitializeAsync
        do! game.LoadContentAsync

        let GameRender = Drawer.createRenderer game
        let SplashRenderer = Drawer.createSplashDrawer game

        let rec engineLoop x =
            async {
                printfn $"Press Any Key To Start"
                do! SplashScreen.startGameLoop game SplashRenderer "Mono Snake"
                printfn $"Start Round %d{x}"
                do! InGameLoop.startGameLoop game GameRender
                printfn "Round Finished"
                do! SplashScreen.startGameLoop game SplashRenderer "GAME OVER"
                do! engineLoop (x + 1)
            }

        return! engineLoop 1
        printfn "Exiting..."
    }

type Result<'TSuccess,'TFailure> =
    | Success of 'TSuccess
    | Failure of 'TFailure
    
let bind switchFunction twoTrackInput =
        match twoTrackInput with
        | Success s -> switchFunction s
        | Failure f -> Failure f
