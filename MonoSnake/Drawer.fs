module Drawer

open Engine
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Types

let DrawPixel (sprite: SpriteBatch) (pixel: Texture2D) (pos: Point) color =
    sprite.Draw(
        pixel,
        Rectangle(pos.X * rectSize + margin, pos.Y * rectSize + margin, rectSize - margin, rectSize - margin),
        color
    )
    
let Render draw itemPos body =
    List.iter (fun x -> draw x Color.Bisque) (List.tail body)
    draw itemPos Color.Green
    draw (List.head body) Color.Pink
    ()
    
let createRenderer (game: MyCrazyGame) =
    let spriteBatch = new SpriteBatch(game.GraphicsDevice)
    let whitePixel = new Texture2D(game.GraphicsDevice, 1, 1)
    whitePixel.SetData([| Color.Beige |])

    let draw pos color =
        DrawPixel spriteBatch whitePixel pos color

    let Render itemPos body =
        game.GraphicsDevice.Clear Color.CornflowerBlue
        spriteBatch.Begin()
        Render draw itemPos body
        spriteBatch.End()

    Render
