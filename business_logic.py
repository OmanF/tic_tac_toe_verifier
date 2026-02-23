from typing import Literal

from domain import Validity


def validate_board(board) -> Literal[Validity.ILLEGAL] | None:
    """
    Validate the input is a legal Tic-Tac-Toe board, i.e., a square matrix

    Args:
        board (list[list[Cell]]): A 2D list representing the Tic-Tac-Toe board
    Returns:
        Validity.ILLEGAL | None: If the board is illegal, return `Validity.ILLEGAL`; otherwise, return `None`
    """
    from domain import Validity

    if not isinstance(board, list) or len(board) == 0:
        return Validity.ILLEGAL
    n = len(board)
    for row in board:
        if not isinstance(row, list) or len(row) != n:
            return Validity.ILLEGAL
        for cell in row:
            if cell not in ("X", "O", "-"):
                return Validity.ILLEGAL
    return None


def transpose(board):
    """
    Transpose the input board, i.e., swap rows and columns

    Args:
        board (list[list[Cell]]): A 2D list representing the Tic-Tac-Toe board
    Returns:
        list[list[Cell]]: The transposed board
    """
    return [list(row) for row in zip(*board)]


def get_diagonals(board):
    """
    Get the two diagonals of the input board

    Args:
        board (list[list[Cell]]): A 2D list representing the Tic-Tac-Toe board
    Returns:
        list[list[Cell]]: A list containing the two diagonals of the board
    """
    n = len(board)
    return [[board[i][i] for i in range(n)], [board[i][n - 1 - i] for i in range(n)]]


def tic_tac_toe_verification(board):
    """
    Verify the input board is a valid Tic-Tac-Toe board and determine the game result

    Args:
        board (list[list[Cell]]): A 2D list representing the Tic-Tac-Toe board
    Returns:
        VerificationResult: A named tuple containing the legality, validity, game result, and cause of invalidity (if any)
    """
    from domain import GameResult, Validity, VerificationResult

    # 1. Check legality
    illegal = validate_board(board)
    if illegal is not None:
        return VerificationResult(
            legality=Validity.ILLEGAL,
            cause="Board is not a square or contains illegal values.",
        )

    n = len(board)
    flat = [cell for row in board for cell in row]
    x_count = flat.count("X")
    o_count = flat.count("O")
    dash_count = flat.count("-")

    # 2. Find winners
    def winner_lines():
        lines = []
        lines.extend(board)
        lines.extend(transpose(board))
        lines.extend(get_diagonals(board))
        return lines

    win_lines = winner_lines()
    x_win = any(all(cell == "X" for cell in line) for line in win_lines)
    o_win = any(all(cell == "O" for cell in line) for line in win_lines)

    # 3. Check for multiple winners
    if x_win and o_win:
        return VerificationResult(
            legality=Validity.LEGAL,
            validity=Validity.INVALID,
            cause="Both players cannot win.",
        )

    # 4. Check move counts
    # X always goes first
    if not (x_count == o_count or x_count == o_count + 1):
        return VerificationResult(
            legality=Validity.LEGAL,
            validity=Validity.INVALID,
            cause="Invalid number of moves.",
        )

    # 5. Check win conditions
    if x_win:
        if x_count != o_count + 1:
            return VerificationResult(
                legality=Validity.LEGAL,
                validity=Validity.INVALID,
                result=GameResult.X_WINS,
                cause="X must have one more move than O if X wins.",
            )
        return VerificationResult(
            legality=Validity.VALID, validity=Validity.VALID, result=GameResult.X_WINS
        )
    if o_win:
        if x_count != o_count:
            return VerificationResult(
                legality=Validity.VALID,
                validity=Validity.INVALID,
                result=GameResult.O_WINS,
                cause="O must have same number of moves as X if O wins.",
            )
        if dash_count == 0:
            return VerificationResult(
                legality=Validity.VALID,
                validity=Validity.INVALID,
                result=GameResult.O_WINS,
                cause="There must be at least one empty cell if O wins.",
            )
        return VerificationResult(
            legality=Validity.VALID, validity=Validity.VALID, result=GameResult.O_WINS
        )

    # 6. Draw
    if dash_count == 0:
        if x_count != o_count + 1:
            return VerificationResult(
                legality=Validity.VALID,
                validity=Validity.INVALID,
                result=GameResult.DRAW,
                cause="Drawn game must have X with one more move than O.",
            )
        return VerificationResult(
            legality=Validity.VALID, validity=Validity.VALID, result=GameResult.DRAW
        )

    # 7. Ongoing
    return VerificationResult(
        legality=Validity.VALID, validity=Validity.VALID, result=GameResult.ONGOING
    )


def pretty_print_results(verification_result):
    """
    Pretty print the verification result

    Args:
        verification_result (VerificationResult): A named tuple containing the legality, validity, game result, and cause of invalidity (if any)
    """
    from domain import GameResult, Validity

    if verification_result.legality == Validity.ILLEGAL:
        print("Board is ILLEGAL.")
        if verification_result.cause:
            print(f"Cause: {verification_result.cause}")
        return
    if verification_result.validity == Validity.INVALID:
        print("Board is INVALID.")
        if verification_result.cause:
            print(f"Cause: {verification_result.cause}")
        return
    if verification_result.result == GameResult.DRAW:
        print("Game ended in a DRAW.")
    elif verification_result.result == GameResult.X_WINS:
        print("X wins!")
    elif verification_result.result == GameResult.O_WINS:
        print("O wins!")
    elif verification_result.result == GameResult.ONGOING:
        print("Game is ONGOING.")
