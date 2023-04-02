using System;

namespace Chess
{
    public class print
    {
        public static void printError(int errorCode)
        {
            switch (errorCode)
            {
                case 77:
                    Console.WriteLine("Check!");
                    break;
                case 1:
                    Console.WriteLine("Illegal move. Can't move your piece at your own piece.");
                    break;
                case 2:
                    Console.WriteLine("Illegal move. Can't make that move under check.");
                    break;
                case 3:
                    Console.WriteLine("Illegal move. Can't castle this side because either the king or the rook have moved before.");
                    break;
                case 4:
                    Console.WriteLine("Illegal move. Can't castle if path is not clear.");
                    break;
                case 5:
                    Console.WriteLine("Illegal move. Can't castle if path where king moves are under check.");
                    break;
                case 6:
                    Console.WriteLine("Illegal move. Can't castle under check.");
                    break;
                case 7:
                    Console.WriteLine("Invalid move!");
                    break;
                case 99:
                    Console.WriteLine("Checkmate! White won!");
                    Console.ReadKey();
                    return;
                case 999:
                    Console.WriteLine("Checkmate! Black won!");
                    Console.ReadKey();
                    return;
                case 9999:
                    Console.WriteLine("Stalemate!");
                    Console.ReadKey();
                    return;
                case 69:
                    Console.WriteLine("Draw by repetition!");
                    Console.ReadKey();
                    return;
            }
        }

        public static void printHelpSelect()
        {
            Console.WriteLine("Type position of the piece you want to move for example: a2 or A2.\nDon't select an enemy piece or empty place.\nEnter king's position if you want to castle.\n");
        }

        public static void printHelpDestination()
        {
            Console.WriteLine("Type position of the piece you want to move for example: a2 or A2.\nDon't enter a square where a piece of yours is.\nType oo for kingside castling or ooo for queenside castling.\nIn case of en passant the destianation must be the square where the pawn will go.\n");
        }
    }
}