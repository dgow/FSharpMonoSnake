module Item

open Microsoft.Xna.Framework

let createItem () : Point =
    let rnd = System.Random()
    let x = rnd.Next(Drawer.screenWidth)
    let y = rnd.Next(Drawer.screenHeight)
    printfn $"%d{x} %d{y}" 
    Point(x, y) 
    