import {Option} from "effect";

export type Player = "X" | "O";
export type CellValue = Player | ".";
export type Validity = "Valid" | "Invalid" | "Illegal";
export type GameState = "XWon" | "OWon" | "Draw" | "InProgress";
export type GameReport = {
    validity: Validity;
    cause: Option.Option<string>;
    state: Option.Option<GameState>;
    winner: Option.Option<Player>;
};
