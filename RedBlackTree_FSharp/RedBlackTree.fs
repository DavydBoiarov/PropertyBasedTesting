module RedBlackTree_FSharp.RedBlackTree

open RedBlackTree_FSharp.Result

type Color =
    | Red
    | Black

type Tree<'T> =
    | Nil
    | Node of Color * Tree<'T> * 'T * Tree<'T>

let (|Left|Right|None|) (x, y) =
    if x < y then Left
    elif x > y then Right
    else None

let insert tree x =

    let rec ins tree =
        match tree with
        | Nil -> ToDo(Node(Red, Nil, x, Nil))
        | Node(color, left, y, right) ->
            match (x, y) with
            | Left -> left |> ins |> map (fun left' -> Node(color, left', y, right)) |> bind balance
            | Right -> right |> ins |> map (fun right' -> Node(color, left, y, right')) |> bind balance
            | None -> Done tree

    and balance tree =
        match tree with
        | Node(Black, Node(Red, a, x, Node(Red, b, y, c)), z, d)
        | Node(Black, Node(Red, Node(Red, a, x, b), y, c), z, d)
        | Node(Black, a, x, Node(Red, Node(Red, b, y, c), z, d))
        | Node(Black, a, x, Node(Red, b, y, Node(Red, c, z, d))) ->
            Node(Red, Node(Black, a, x, b), y, Node(Black, c, z, d)) |> ToDo
        | Node(Black, _, _, _) -> Done tree
        | _ -> ToDo tree

    and blacken tree =
        match tree with
        | Node(Red, left, x, right) -> Node(Black, left, x, right)
        | _ -> tree

    tree |> ins |> fromResult |> blacken

