import _ from "lodash";
import type {CellValue, GameReport, Player} from "./domain";
import {Option, Exit, Cause} from "effect";

/**
 * Transposes a square matrix.
 * @param {CellValue[][]} board - A 2D array representing the tic-tac-toe board
 * @returns {CellValue[][]} The transposed matrix
 */
const transpose = (board: CellValue[][]): CellValue[][] => {
    return _.zip(...board) as CellValue[][];
};

/**
 * Collects the main diagonal of a square matrix.
 * @param {CellValue[][]} board - A 2D array representing the tic-tac-toe board
 * @returns {CellValue[]} An array containing the elements of the main diagonal
 */
const collectDiagonal = (board: CellValue[][]): CellValue[] => board.map((row, index) => row[index]) as CellValue[];

/** Collects the anti-diagonal of a square matrix.
 * @param {CellValue[][]} board - A 2D array representing the tic-tac-toe board
 * @returns {CellValue[]} An array containing the elements of the anti-diagonal
 */
const collectAntiDiagonal = (board: CellValue[][]): CellValue[] => {
    return board.map((row, index) => row[row.length - 1 - index]) as CellValue[];
};

/**
 * Validates the input is a non-empty square matrix.
 * @param {CellValue[][]} board - A 2D array representing the tic-tac-toe board
 * @returns {Effect.Exit} An `Effect.Exit` object indicating either success, or the failure cause
 */
const validateLegalBoard = (board: CellValue[][]): Exit.Exit<CellValue[][], string> => {
    if (
        !Array.isArray(board) ||
        board.length === 0 ||
        !board.every((sub) => Array.isArray(sub) && sub.length === board.length)
    ) {
        return Exit.failCause(Cause.fail("Input must be a non-empty square matrix"));
    } else return Exit.succeed(board);
};

/**
 * @param {CellValue[][]} board - A 2D array representing the tic-tac-toe board
 * @returns {Record<CellValue, number>} An object counting the occurrences of each player's tokens and empty cells
 */
const tokenCounter = (board: CellValue[][]): Record<CellValue, number> => {
    return _.countBy(_.flattenDeep(board), (cell) => (cell === "X" || cell === "O" ? cell : ".")) as Record<
        CellValue,
        number
    >;
};

/**
 * @param {CellValue[]} subArray - A 1D array representing a row, column, or diagonal of the tic-tac-toe board
 * @returns {Option.Option<Player>} An `Option.Option` object indicating the winner, if any
 */
const subArrayWinner = (subArray: CellValue[]): Option.Option<Player> => {
    if (subArray.every((elem) => elem === subArray[0]) && subArray[0] !== ".") {
        return Option.some(subArray[0] as Player);
    }
    return Option.none();
};

export const ticTacToeVerifier = (board: CellValue[][]): GameReport => {
    // 1. Validate the board is a non-empty square matrix
    if (Exit.isFailure(validateLegalBoard(board))) {
        return {
            validity: "Illegal",
            cause: Option.some("Input must be a non-empty square matrix"),
            state: Option.none(),
            winner: Option.none(),
        };
    }

    // 2. Count the tokens for each player and empty cells, and validate the counts are legal
    const tokenCounts = tokenCounter(board);
    const xCount = tokenCounts["X"] || 0;
    const oCount = tokenCounts["O"] || 0;

    if (xCount < oCount || xCount > oCount + 1) {
        return {
            validity: "Invalid",
            cause: Option.some(
                "The number of 'X' tokens must be equal to or exactly one more than the number of 'O' tokens."
            ),
            state: Option.none(),
            winner: Option.none(),
        };
    }

    // 3. Check for winners in rows, columns, and diagonals
    const linesToCheck = [
        ...board, // rows
        ...transpose(board), // columns
        collectDiagonal(board), // main diagonal
        collectAntiDiagonal(board), // anti-diagonal
    ];

    const winners = linesToCheck
    .map(subArrayWinner)
    .filter(Option.isSome)
    .map((opt) => opt.value);

    const uniqueWinners = Array.from(new Set(winners));

    if (uniqueWinners.length > 1) {
        return {
            validity: "Invalid",
            cause: Option.some("Both players cannot win simultaneously."),
            state: Option.none(),
            winner: Option.none(),
        };
    }

    if (uniqueWinners.length === 1) {
        const winner = uniqueWinners[0];
        if (winner === "X" && xCount !== oCount + 1) {
            return {
                validity: "Invalid",
                cause: Option.some("Invalid number of 'X' tokens for 'X' to win."),
                state: Option.none(),
                winner: Option.none(),
            };
        }
        if (winner === "O" && xCount !== oCount) {
            return {
                validity: "Invalid",
                cause: Option.some("Invalid number of 'O' tokens for 'O' to win."),
                state: Option.none(),
                winner: Option.none(),
            };
        }
        return {
            validity: "Valid",
            cause: Option.none(),
            state: winner === "X" ? Option.some("XWon") : Option.some("OWon"),
            winner: Option.some(winner) as Option.Option<Player>,
        };
    }

    // 4. If no winners, determine if the game is a draw or still in progress
    const emptyCount = tokenCounts["."] ?? 0;
    if (emptyCount === 0) {
        return {
            validity: "Valid",
            cause: Option.none(),
            state: Option.some("Draw"),
            winner: Option.none(),
        };
    } else {
        return {
            validity: "Valid",
            cause: Option.none(),
            state: Option.some("InProgress"),
            winner: Option.none(),
        };
    }
};

export const prettyPrintGameReport = (report: GameReport): string => {
    const {validity, state, winner} = report;

    if (validity === "Illegal") {
        return (
            "The board is illegal: " +
            Option.match(report.cause, {
                onNone: () => "Unknown cause",
                onSome: (cause) => cause,
            })
        );
    } else if (validity === "Invalid") {
        return (
            "The board is invalid: " +
            Option.match(report.cause, {
                onNone: () => "Unknown cause",
                onSome: (cause) => cause,
            })
        );
    } else {
        const stateStr = Option.match(state, {
            onNone: () => "Unknown State",
            onSome: (s) => s,
        });
        const winnerStr = Option.match(winner, {
            onNone: () => "No Winner",
            onSome: (w) => w,
        });
        return `The board is valid. State: ${stateStr}. Winner: ${winnerStr}.`;
    }
};
