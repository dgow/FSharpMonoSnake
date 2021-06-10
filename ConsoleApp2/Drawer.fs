module Drawer

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

let rectSize = 50
let screenWidth = 12
let screenHeight = 8
let margin = 2

let field = Rectangle(0,0, screenWidth, screenHeight)

let Draw (sprite: SpriteBatch) (pixel: Texture2D) (pos: Point) color =
    sprite.Draw(
        pixel,
        Rectangle(pos.X * rectSize + margin, pos.Y * rectSize + margin, rectSize - margin, rectSize - margin),
        color
    )