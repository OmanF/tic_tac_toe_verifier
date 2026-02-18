import {expect, test} from "bun:test";
import {prettyPrintGameReport, ticTacToeVerifier} from "./business_logic";
import type {CellValue} from "./domain";

// Edge case: n = 1
test("n=1, X wins", () => {
    const board = [["X"]] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe("The board is valid. State: XWon. Winner: X.");
});

// O cannot win because X always goes first!
test("n=1, O wins", () => {
    const board = [["O"]] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe(
        "The board is invalid: The number of 'X' tokens must be equal to or exactly one more than the number of 'O' tokens."
    );
});

test("n=1, empty", () => {
    const board = [["."]] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe(
        "The board is valid. State: InProgress. Winner: No Winner."
    );
});

// Edge case: n = 2
test("n=2, X wins", () => {
    const board = [
        ["X", "X"],
        ["O", "."],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe("The board is valid. State: XWon. Winner: X.");
});

// Invalid: implies O either "double dipped" on his turn or went first, both of which are illegal
test("n=2, O wins", () => {
    const board = [
        ["O", "X"],
        ["O", "."],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe(
        "The board is invalid: The number of 'X' tokens must be equal to or exactly one more than the number of 'O' tokens."
    );
});

test("n=2, in progress", () => {
    const board = [
        ["X", "."],
        ["O", "."],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe(
        "The board is valid. State: InProgress. Winner: No Winner."
    );
});

test("n=3, invalid: both players win", () => {
    const board = [
        ["X", "X", "X"],
        ["O", "O", "O"],
        ["X", "O", "X"],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe(
        "The board is invalid: Both players cannot win simultaneously."
    );
});

test("n=3, X wins", () => {
    const board = [
        ["X", "O", "X"],
        ["O", "X", "O"],
        ["O", "X", "X"],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe("The board is valid. State: XWon. Winner: X.");
});

test("n=3, O wins", () => {
    const board = [
        ["O", "X", "X"],
        ["O", "X", "X"],
        ["O", "O", "."],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe("The board is valid. State: OWon. Winner: O.");
});

test("n=3, draw", () => {
    const board = [
        ["X", "O", "X"],
        ["O", "X", "X"],
        ["O", "X", "O"],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe("The board is valid. State: Draw. Winner: No Winner.");
});

test("n=3, invalid: too many X's", () => {
    const board = [
        ["X", "X", "X"],
        ["O", "O", "X"],
        [".", ".", "."],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe(
        "The board is invalid: The number of 'X' tokens must be equal to or exactly one more than the number of 'O' tokens."
    );
});

test("n=4, X wins with a row, valid token count", () => {
    const board = [
        ["X", "X", "X", "X"],
        ["O", "O", ".", "."],
        ["O", ".", ".", "."],
        [".", ".", ".", "."],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe("The board is valid. State: XWon. Winner: X.");
});

test("n=4, O wins diagonal, valid token count", () => {
    const board = [
        ["O", ".", ".", "."],
        ["X", "O", ".", "."],
        ["X", "X", "O", "."],
        ["X", ".", ".", "O"],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe("The board is valid. State: OWon. Winner: O.");
});

test("n=4, draw", () => {
    const board = [
        ["X", "X", "X", "O"],
        ["O", "O", "X", "O"],
        ["X", "O", "X", "O"],
        ["O", "X", "O", "X"],
    ] as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe("The board is valid. State: Draw. Winner: No Winner.");
});

test("illegal board: not square", () => {
    const board = [
        ["X", "O", "X"],
        ["O", "X"],
        ["O", "X", "X"],
    ] as unknown as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe(
        "The board is illegal: Input must be a non-empty square matrix"
    );
});

test("illegal board: empty", () => {
    const board = [] as unknown as CellValue[][];
    expect(prettyPrintGameReport(ticTacToeVerifier(board))).toBe(
        "The board is illegal: Input must be a non-empty square matrix"
    );
});
