# Tic-Tac-Toe verifier

## The spec

Implement a tic-tac-toe verifier with the following, purposely loose, spec:

* The input is a representation of a TTT board of dimension `N`.
  * `N` is not given explicitly, rather it can be deduced by the input's size.
  * `N` can be **any** `Natural` number, **including** 1 or 2 - with their unique win conditions.
  * Input's type an "implementor's choice".
* Given the board, the application should decide, and output:
  * Whether the board is legal, i.e., is it a representation of an `N` * `N` square.
    * If not, throw a *descriptive* error and quit.
  * Whether the position of the board is valid.
    * While no formal definition is given, a valid board is achieved by a *normal* play of the game, according to all official rules and constraints.
  * **Regardless** of validity, if the position has a winner - declare it.
    * Including the *same player* having *multiple winning combinations* (while an invalid board).
    * If **both** players have a winning combination, an invalid position by definition, declare a draw.

## The repo

This repo contains the implementation of the verifier in several (programming) language - see the branches selector, each branch corresponds to a different language.  
(I used *some* AI/LLM help, but all implementations are mostly mine, in languages I know).
