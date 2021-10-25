module Item

open Microsoft.Xna.Framework

let createItem () : Point =
    let rnd = System.Random()
    let x = rnd.Next(Types.screenWidth)
    let y = rnd.Next(Types.screenHeight)
    Point(x, y) 
    
