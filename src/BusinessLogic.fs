namespace TicTacToeVerifier

open Domain

[<AutoOpen>]
module BusinessLogic =
    /// <summary>
    /// Checks if all elements in the sub-array are the same and not empty, returning the winner if so.
    /// </summary>
    /// <param name="subArray">
    ///  An array of `Cell` representing a row, column, or diagonal of the game board.
    /// </param>
    /// <returns>
    /// `Some(Player)` if there is a winner, or `None` if there is no winner in the sub-array.
    /// </returns>
    let subArrayWinner (subArray: Cell array) : Player option =
        match subArray |> Array.distinct with
        | [| Player p |] -> Some p
        | _ -> None

    /// <summary>
    /// Collects the main diagonal and anti-diagonal from the game board.
    /// </summary>
    /// <param name="board">
    /// A 2D array of `Cell` representing the game board.
    /// </param>
    /// <returns>
    /// An array containing two sub-arrays: the main diagonal and the anti-diagonal of the board.
    /// </returns>
    let collectDiagonals (board: Cell array array) : Cell array array =
        let size = board.Length

        [| Array.init size (fun i -> board.[i].[i])
           Array.init size (fun i -> board.[i].[size - 1 - i]) |]

    /// <summary>
    /// Validates that the provided game board is a non-empty square matrix.
    /// </summary>
    /// <param name="board">
    /// A 2D array of `Cell` representing the game board.
    /// </param>
    /// <returns>
    /// `Ok()` if the board is valid, or `Error(string)` if the board is invalid.
    /// </returns>
    let validateLegalBoard (board: Cell array array) : Result<unit, string> =
        let size = board.Length

        if size = 0 then
            Error "Board must be non-empty."
        elif board |> Array.exists (fun row -> row.Length <> size) then
            Error "Board must be square."
        else
            Ok()

    /// <summary>
    /// Counts the number of tokens placed by each player on the game board.
    /// </summary>
    /// <param name="board">
    /// A 2D array of `Cell` representing the game board.
    /// </param>
    /// <returns>
    /// A map where the keys are players (`X` and `O`) and the values are the counts of tokens placed by each player on the board.
    /// </returns>
    let tokenCounter (board: Cell array array) : Map<Player, int> =
        board
        |> Array.concat
        |> Array.choose (function
            | Player p -> Some p
            | _ -> None)
        |> Array.countBy id
        |> Map.ofArray

    /// <summary>
    /// Verifies the validity of a Tic Tac Toe game board and determines the game state and winner if applicable.
    /// </summary>
    /// <param name="board">
    /// A 2D array of `Cell` representing the game board.
    /// </param>
    /// <returns>
    /// A `GameReport` containing the validity of the board, the cause of any invalidity if any. For valid games, includes the game state (ongoing, draw, or win), and the winner if there is one.
    /// </returns>
    let ticTacToeVerifier (board: Cell array array) : GameReport =
        match validateLegalBoard board with
        | Error e ->
            { Validity = Illegal
              Cause = Some(string e)
              GameState = None
              Winner = None }
        | Ok() ->
            let tokenCounts = tokenCounter board
            let xCount = Map.tryFind X tokenCounts |> Option.defaultValue 0
            let oCount = Map.tryFind O tokenCounts |> Option.defaultValue 0

            if xCount < oCount || xCount > oCount + 1 then
                { Validity = Invalid
                  Cause =
                    Some(
                        sprintf
                            "Invalid token counts: X has %d tokens, O has %d tokens. In valid games, X token count **must** be equal to or **exactly one** more than O token count."
                            xCount
                            oCount
                    )
                  GameState = None
                  Winner = None }
            else
                let rowsAndCols = [| yield! board; yield! board |> Array.transpose |]

                let diagonals = collectDiagonals board

                let allLines = Array.append rowsAndCols diagonals

                let winners = allLines |> Array.choose subArrayWinner |> Array.distinct

                match winners with
                | [| winner |] ->
                    { Validity = Valid
                      Cause = None
                      GameState = Win winner |> Some
                      Winner = Some winner }
                | [||] ->
                    if Array.exists (Array.exists ((=) Empty)) board then
                        { Validity = Valid
                          Cause = None
                          GameState = Ongoing |> Some
                          Winner = None }
                    else
                        { Validity = Valid
                          Cause = None
                          GameState = Draw |> Some
                          Winner = None }
                | _ ->
                    { Validity = Invalid
                      Cause = Some(sprintf "Both players cannot win simultaneously.")
                      GameState = None
                      Winner = None }

    /// <summary>
    /// Converts a `GameReport` into a human-readable string format for display.
    /// </summary>
    /// <param name="report">
    /// A `GameReport` containing the validity, cause of invalidity if any, game state, and winner of a Tic Tac Toe game.
    /// </param>
    /// <returns>
    /// A string summarizing the game report, including the validity of the board, any issues detected, the game state if valid, and the winner if there is one.
    /// </returns>
    let prettyPrintGameReport (report: GameReport) : string =
        let causeStr = report.Cause |> Option.defaultValue "No issues detected."

        let gameStateStr =
            match report.GameState with
            | Some Ongoing -> "Ongoing."
            | Some Draw -> "Draw."
            | Some(Win player) -> sprintf "Win for %A." player
            | None -> "Not applicable."

        match report.Validity with
        | Illegal -> sprintf "Illegal board. %s" causeStr
        | Invalid -> sprintf "Invalid board. %s" causeStr
        | Valid -> sprintf "Valid board. Game State: %s" gameStateStr
