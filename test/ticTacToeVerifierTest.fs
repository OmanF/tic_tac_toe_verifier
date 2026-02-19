module TicTacToeVerifierTests

open Expecto
open TicTacToeVerifier.Domain
open TicTacToeVerifier.BusinessLogic

let tests =
    testList
        "TicTacToeVerifier tests"
        [ testList
              "Illegal board"
              [ testCase "Empty board"
                <| fun _ ->
                    let board = [||] |> Array.map (fun _ -> [||])

                    Expect.equal
                        (board |> ticTacToeVerifier |> prettyPrintGameReport)
                        "Illegal board. Board must be non-empty."
                        "Expected the board to be illegal due to being empty."

                testCase "Non-square board"
                <| fun _ ->
                    let board = [| [| Player X; Player O |]; [| Player O |] |]

                    Expect.equal
                        (board |> ticTacToeVerifier |> prettyPrintGameReport)
                        "Illegal board. Board must be square."
                        "Expected the board to be illegal due to not being square." ]
          testList
              "Edge cases"
              [ testCase "n=1, X wins"
                <| fun _ ->
                    let board = [| [| Player X |] |]

                    Expect.equal
                        (board |> ticTacToeVerifier |> prettyPrintGameReport)
                        "Valid board. Game State: Win for X."
                        "Expected the board to be valid with X winning."

                testCase "n=1, O wins"
                <| fun _ ->
                    let board = [| [| Player O |] |]

                    Expect.equal
                        (board |> ticTacToeVerifier |> prettyPrintGameReport)
                        "Invalid board. Invalid token counts: X has 0 tokens, O has 1 tokens. In valid games, X token count **must** be equal to or **exactly one** more than O token count."
                        "Expected the board to be invalid due to O winning without X."

                testCase "n=1, empty"
                <| fun _ ->
                    let board = [| [| Empty |] |]

                    Expect.equal
                        (board |> ticTacToeVerifier |> prettyPrintGameReport)
                        "Valid board. Game State: Ongoing."
                        "Expected the board to be valid and in progress with no winner."

                testCase "n=2, X Wins"
                <| fun _ ->
                    let board = [| [| Player X; Player X |]; [| Player O; Empty |] |]

                    Expect.equal
                        (board |> ticTacToeVerifier |> prettyPrintGameReport)
                        "Valid board. Game State: Win for X."
                        "Expected the board to be valid with X winning."

                // Invalid board: this means O either "double dipped" on his turn or went first, both of which are illegal
                testCase "n=2, O Wins"
                <| fun _ ->
                    let board = [| [| Player O; Player O |]; [| Player X; Empty |] |]

                    Expect.equal
                        (board |> ticTacToeVerifier |> prettyPrintGameReport)
                        "Invalid board. Invalid token counts: X has 1 tokens, O has 2 tokens. In valid games, X token count **must** be equal to or **exactly one** more than O token count."
                        "Expected the board to be invalid due to O winning without X."

                testCase "n=2, Ongoing"
                <| fun _ ->
                    let board = [| [| Player X; Empty |]; [| Player O; Empty |] |]

                    Expect.equal
                        (board |> ticTacToeVerifier |> prettyPrintGameReport)
                        "Valid board. Game State: Ongoing."
                        "Expected the board to be valid and in progress with no winner." ]

          testCase "n=3, invalid: both players win"
          <| fun _ ->
              let board =
                  [| [| Player X; Player X; Player X |]
                     [| Player O; Player O; Player O |]
                     [| Player X; Player O; Player X |] |]

              Expect.equal
                  (board |> ticTacToeVerifier |> prettyPrintGameReport)
                  "Invalid board. Both players cannot win simultaneously."
                  "Expected the board to be invalid due to both players winning simultaneously."

          testCase "n=3, invalid: too many X's"
          <| fun _ ->
              let board =
                  [| [| Player X; Player X; Player X |]
                     [| Player O; Player O; Player X |]
                     [| Empty; Empty; Empty |] |]

              Expect.equal
                  (board |> ticTacToeVerifier |> prettyPrintGameReport)
                  "Invalid board. Invalid token counts: X has 4 tokens, O has 2 tokens. In valid games, X token count **must** be equal to or **exactly one** more than O token count."
                  "Expected the board to be invalid due to too many X's."

          testCase "n=3, X wins"
          <| fun _ ->
              let board =
                  [| [| Player X; Player O; Player X |]
                     [| Player O; Player X; Player O |]
                     [| Player O; Player X; Player X |] |]

              Expect.equal
                  (board |> ticTacToeVerifier |> prettyPrintGameReport)
                  "Valid board. Game State: Win for X."
                  "Expected the board to be valid with X winning."

          testCase "n=3, O wins"
          <| fun _ ->
              let board =
                  [| [| Player O; Player X; Player X |]
                     [| Player O; Player X; Player X |]
                     [| Player O; Player O; Empty |] |]

              Expect.equal
                  (board |> ticTacToeVerifier |> prettyPrintGameReport)
                  "Valid board. Game State: Win for O."
                  "Expected the board to be valid with O winning."

          testCase "n=3, Draw"
          <| fun _ ->
              let board =
                  [| [| Player X; Player O; Player X |]
                     [| Player O; Player X; Player X |]
                     [| Player O; Player X; Player O |] |]

              Expect.equal
                  (board |> ticTacToeVerifier |> prettyPrintGameReport)
                  "Valid board. Game State: Draw."
                  "Expected the board to be valid with a draw."

          testCase "n=4, X wins"
          <| fun _ ->
              let board =
                  [| [| Player X; Player X; Player X; Player X |]
                     [| Player O; Player O; Empty; Empty |]
                     [| Player O; Empty; Empty; Empty |]
                     [| Empty; Empty; Empty; Empty |] |]

              Expect.equal
                  (board |> ticTacToeVerifier |> prettyPrintGameReport)
                  "Valid board. Game State: Win for X."
                  "Expected the board to be valid with X winning."

          testCase "n=4, O wins"
          <| fun _ ->
              let board =
                  [| [| Player O; Empty; Empty; Empty |]
                     [| Player X; Player O; Empty; Empty |]
                     [| Player X; Player X; Player O; Empty |]
                     [| Player X; Empty; Empty; Player O |] |]

              Expect.equal
                  (board |> ticTacToeVerifier |> prettyPrintGameReport)
                  "Valid board. Game State: Win for O."
                  "Expected the board to be valid with O winning."

          testCase "n=4, Draw"
          <| fun _ ->
              let board =
                  [| [| Player X; Player X; Player X; Player O |]
                     [| Player O; Player O; Player X; Player O |]
                     [| Player X; Player O; Player X; Player O |]
                     [| Player O; Player X; Player O; Player X |] |]

              Expect.equal
                  (board |> ticTacToeVerifier |> prettyPrintGameReport)
                  "Valid board. Game State: Draw."
                  "Expected the board to be valid and in progress with no winner." ]
