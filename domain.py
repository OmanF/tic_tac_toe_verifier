from enum import Enum, auto
from typing import Literal, NamedTuple, TypeAlias

Cell: TypeAlias = Literal["X", "O", "-"]
Board: TypeAlias = list[list[Cell]]


class Validity(Enum):
    LEGAL = auto()
    ILLEGAL = auto()
    INVALID = auto()
    VALID = auto()


class GameResult(Enum):
    DRAW = auto()
    X_WINS = auto()
    O_WINS = auto()
    ONGOING = auto()


class VerificationResult(NamedTuple):
    legality: Validity
    validity: Validity | None = None
    result: GameResult | None = None
    cause: str | None = None
