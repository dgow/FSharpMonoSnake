open ConsoleApp2
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

open Input
open Types

type MyCrazyGame() =
    inherit Game()

    let initializeEvt = Event<_>()
    let loadEvt = Event<_>()
    let loopEvt = Event<_>()

    override x.Initialize() =
        initializeEvt.Trigger()
        do base.Initialize()

    override x.LoadContent() = loadEvt.Trigger()

    override x.Update(gameTime) = loopEvt.Trigger(Update gameTime)

    override x.Draw(gameTime) = loopEvt.Trigger(Draw gameTime)

    member game.InitializeAsync = Async.AwaitEvent initializeEvt.Publish
    member game.LoadContentAsync = Async.AwaitEvent loadEvt.Publish
    member game.LoopAsync = Async.AwaitEvent loopEvt.Publish

[<EntryPoint>]
let main _ =
    printfn "Start Game"

    use game = new MyCrazyGame()

    let gameWorkflow =
        async {
            let graphics = new GraphicsDeviceManager(game)
            do! game.InitializeAsync
            graphics.PreferredBackBufferWidth <- Drawer.rectSize * Drawer.screenWidth
            graphics.PreferredBackBufferHeight <- Drawer.rectSize * Drawer.screenHeight
            graphics.ApplyChanges()
            
            do! game.LoadContentAsync

            let spriteBatch = new SpriteBatch(game.GraphicsDevice)
            let whitePixel = new Texture2D(game.GraphicsDevice, 1, 1)
            whitePixel.SetData([| Color.Beige |])

            let draw pos color =
                Drawer.Draw spriteBatch whitePixel pos color

            let rec gameLoop lastKeyBoardState itemPos body =
                async {
                    let! nextState = game.LoopAsync
                    let keyboardState = Keyboard.GetState()
                    let repeatLoop = gameLoop keyboardState itemPos body
                    let nextLoopWithMove = gameLoop keyboardState itemPos
                    
                    match nextState with
                    | Update time ->
                        
                        let movedBody =
                            DirectionFromKeyboard keyboardState lastKeyBoardState
                            |> GameLogic.MakeMove itemPos body

                        match movedBody with
                            | Some (FedUp newBody) -> 
                                return! gameLoop keyboardState (Item.createItem ()) newBody
                            | Some (Hungry newBody)  ->
                               return! nextLoopWithMove newBody
                            | None -> return! repeatLoop
                            
                    | Draw time ->
                        do game.GraphicsDevice.Clear Color.CornflowerBlue
                        spriteBatch.Begin()

                        List.iter (fun x -> draw x Color.Bisque) (List.tail body)
                        draw itemPos Color.Green
                        draw (List.head body) Color.Pink

                        spriteBatch.End()

                        return! repeatLoop
                }


            return! gameLoop (Keyboard.GetState()) (Item.createItem ()) [ Point.Zero ]
        }

    gameWorkflow |> Async.StartImmediate

    game.Run()
    0
