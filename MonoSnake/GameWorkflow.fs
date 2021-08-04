module GameWorkflow

open ConsoleApp2
open Engine

let gameWorkflow (game: MyCrazyGame) =
    async {
        do! game.InitializeAsync
        do! game.LoadContentAsync

        let Render = Drawer.createRenderer game

        let rec engineLoop x =
            async {
                printfn $"Start Round %d{x}"
                do! InGameLoop.startGameLoop game Render
                printfn "Round Finished"
                do! engineLoop (x + 1)
            }

        return! engineLoop 1
        printfn "Exiting..."
    }
