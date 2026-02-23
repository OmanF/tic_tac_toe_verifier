from business_logic import pretty_print_results as ppr
from business_logic import tic_tac_toe_verification as tttv
from domain import Board


def test_n_1_x_wins(capsys) -> None:
    board: Board = [["X"]]
    ppr(tttv(board))
    out, err = capsys.readouterr()
    assert out.strip() == "X wins!"


# TODO: Implement the rest of the test cases, taking example from the TS or F# branches
