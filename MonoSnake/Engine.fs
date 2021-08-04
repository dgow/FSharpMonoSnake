module Engine

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open MonoSnake
open Types

type MyCrazyGame() as x =
    inherit Game()

    let initializeEvt = Event<_>()
    let loadEvt = Event<_>()
    let loopEvt = Event<_>()
    
    let mutable prevKeyboardState = KeyboardState()

    let graphics = new GraphicsDeviceManager(x)
    
    override x.Initialize() =
        initializeEvt.Trigger()
        do base.Initialize()

    override x.LoadContent() =
        graphics.PreferredBackBufferWidth <- Types.rectSize * Types.screenWidth
        graphics.PreferredBackBufferHeight <- Types.rectSize * Types.screenHeight
        graphics.ApplyChanges()
        loadEvt.Trigger()

    override x.Update(gameTime) =
        let keys = Keyboard.GetState()
        let dir = MonoSnake.Input.GetInput keys prevKeyboardState
        let data = { time = gameTime
                     dir = dir }
        
        if dir <> Point.Zero then
            loopEvt.Trigger(Update data)
            
        prevKeyboardState <- keys

    override x.Draw(gameTime) = loopEvt.Trigger(Draw gameTime)

    member game.InitializeAsync = Async.AwaitEvent initializeEvt.Publish
    member game.LoadContentAsync = Async.AwaitEvent loadEvt.Publish
    member game.LoopAsync = Async.AwaitEvent loopEvt.Publish
