module Engine

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open Types

type MyCrazyGame() as x =
    inherit Game()

    let initializeEvt = Event<_>()
    let loadEvt = Event<_>()
    let loopEvt = Event<_>()

    let graphics = new GraphicsDeviceManager(x)
    
    override x.Initialize() =
        initializeEvt.Trigger()
        do base.Initialize()

    override x.LoadContent() =
        graphics.PreferredBackBufferWidth <- Drawer.rectSize * Drawer.screenWidth
        graphics.PreferredBackBufferHeight <- Drawer.rectSize * Drawer.screenHeight
        graphics.ApplyChanges()
        loadEvt.Trigger()

    override x.Update(gameTime) =
        let data = { time = gameTime
                     keyboard = Keyboard.GetState() }
        loopEvt.Trigger(Update data)

    override x.Draw(gameTime) = loopEvt.Trigger(Draw gameTime)

    member game.InitializeAsync = Async.AwaitEvent initializeEvt.Publish
    member game.LoadContentAsync = Async.AwaitEvent loadEvt.Publish
    member game.LoopAsync = Async.AwaitEvent loopEvt.Publish
