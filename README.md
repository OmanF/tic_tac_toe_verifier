# Tic-Tac-Toe verifier

## The spec

Implement a tic-tac-toe verifier with the following, purposely loose, spec:

* The input is a representation of a TTT board of dimension `N`.
  * `N` is not given explicitly, rather it can be deduced by the input's size.
  * `N` can be **any** `Natural` number, **including** 1 or 2 - with their unique win conditions.
  * Input's type an "implementor's choice".
* Given the board, the application should decide, and output:
  * Whether the board is legal, i.e., is it a representation of an `N` * `N` square.
    * An illegal board's output is it's illegal.
  * Whether the position of the board is valid.
    * An invalid position's output is it's invalid.
    * A valid position's output is either drawn, or the winner.

### Tic-tac-toe validity discussion

A game of TTT, where, according to the official rules, `X` goes first and both players played only legal moves, can end in one of two ways: draw, or *one* of the players having *one* winning combination.

That, however, by itself, is a necessary but *in*sufficient condition: an invalid input to the verifier may be: `X,X,X,-,-,-,-,-,-`, denoting a board where the first row is filled by `X`, and no other token is present on the board.  
It would appear `X` has a winning combination, it does, but the board itself is invalid.

To derive the rules of validity we'll recall that TTT is an alternating turn-based game, on a finite board, with `X` moving first.

It can be easily shown, how math professor of me 😄, that in a drawn game, `X` has one token more than `O`, and the sum of tokens for both players is `n^2`, where `n` is the size of the side of the board.  
It can also be shown that in a valid game where `X` won, it again has one token more than `O`. There may, or may not, be empty cells on the board.  
If `O` is the winner, the token count is the same, and there **must** be **at least one** empty cell on the board.

With that in mind, we can now **define what a valid TTT is**:

* The game was either drawn or a **single player** has **one** winning combination.
* If the game was drawn:
  * `X` **must** have **one** token more than `O`.
  * The sum of tokens of both players is `n^2` (where `n` is the length of the side of the board).
* If `X` won:
  * `X` **must** have **one** token more than `O`.
  * There may, or may not, be empty cells left, and the sum of tokens of both players plus empty cells, if any, is n^2.
* If `O` won:
  * Count of token of both players **must** be **the same**.
  * There **must** be **at least** one empty cell left, and the sum of tokens of both players plus empty cells, at least one of which exists, is n^2.

These rules apply to *any* `Natural n` (i.e., `n` is an integer greater than or equal to 1), including the degenerate case of `n = 1`, and the less than interesting `n = 2`.

## The repo

This repo contains the implementation of the verifier in several (programming) language - see the branches selector, each branch corresponds to a different language.  
(I used *some* AI/LLM help, but all implementations are mostly mine, in languages I know).
