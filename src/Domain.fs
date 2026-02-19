namespace TicTacToeVerifier

[<AutoOpen>]
module Domain =
    type Player =
        | X
        | O

    type Cell =
        | Empty
        | Player of Player

    type Validity =
        | Valid
        | Invalid
        | Illegal

    type GameState =
        | Ongoing
        | Draw
        | Win of Player

    type GameReport =
        { Validity: Validity
          Cause: string option
          GameState: GameState option
          Winner: Player option }
