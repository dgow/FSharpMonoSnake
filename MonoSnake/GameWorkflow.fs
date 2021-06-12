module GameWorkflow

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

open Engine
open Types
open Input

let gameWorkflow (game : MyCrazyGame) =
    async {
            do! game.InitializeAsync
            do! game.LoadContentAsync

            let spriteBatch = new SpriteBatch(game.GraphicsDevice)
            let whitePixel = new Texture2D(game.GraphicsDevice, 1, 1)
            whitePixel.SetData([| Color.Beige |])

            let draw pos color =
                Drawer.Draw spriteBatch whitePixel pos color

            let rec gameLoop lastKeyBoardState itemPos body =
                async {
                    let! nextState = game.LoopAsync
                    
                    match nextState with
                    | Update data ->
                        let nextLoopWithBody = gameLoop data.keyboard itemPos
                        
                        let movedBody =
                            DirectionFromKeyboard data.keyboard lastKeyBoardState
                            |> GameLogic.MakeMove itemPos body

                        match movedBody with
                            | Ok (FedUp newBody) -> 
                                return! gameLoop data.keyboard (Item.createItem ()) newBody
                            | Ok (Hungry newBody)  ->
                               return! nextLoopWithBody newBody
                            | Error NoInput -> return! nextLoopWithBody body
                            | Error Outside ->
                                printf "You are Outside! GAME OVER"
                            | Error HitBody ->
                                printf "Dont eat yourself! GAME OVER"
                                
                    | Draw time ->
                        do game.GraphicsDevice.Clear Color.CornflowerBlue
                        spriteBatch.Begin()

                        List.iter (fun x -> draw x Color.Bisque) (List.tail body)
                        draw itemPos Color.Green
                        draw (List.head body) Color.Pink

                        spriteBatch.End()

                        return! gameLoop lastKeyBoardState itemPos body
                }


            return! gameLoop (Keyboard.GetState()) (Item.createItem ()) [ Point.Zero ]
        }
    