// Commented out in lieu of extensive test bed. Left here as a reference.
// Useful if board input should become interactive, entered by the user.
// Of course, in that case, there would need to be a parser to convert user input into the `Cell array array` format expected by the business logic!

// namespace TicTacToeVerifier

// module Program =
//     let board =
//         [| [| Player X; Player O; Player X |]
//            [| Player O; Player X; Player O |]
//            [| Player O; Player X; Player X |] |]

//     board |> ticTacToeVerifier |> prettyPrintGameReport |> printfn "%s"

open Expecto
open TicTacToeVerifierTests

[<EntryPoint>]
let main argv =
    // Run tests
    runTestsWithCLIArgs [] argv tests
