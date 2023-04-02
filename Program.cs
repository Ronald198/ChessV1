using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class Program
    {
        public static bool whiteQueensideCastling = true;
        public static bool blackQueensideCastling = true;
        public static bool whiteKingsideCastling = true;
        public static bool blackKingsideCastling = true;
        public static bool underCheck = false;
        public static string lastPieceMoved;
        public static bool enPassantExecuted = false;

        public static void printGame(string[,] game)
        {
            int nr = 8; //numbers next to the board

            for (int i = 0; i < 10; i++)
            {
                char columns = 'A'; //letters for the coordinates

                for (int j = 0; j < 10; j++)
                {
                    if(i == 0 || i == 9) //first and last rows only
                    {
                        if(j < 9)
                        {
                            if(j == 0)
                            {
                                Console.Write("   "); //top left corner
                            }
                            else
                            {
                                Console.Write(" " + columns++ + " "); //letters + some spacing
                            }
                        }
                    }
                    else if(j == 0 || j == 9) //first and last columns only
                    {
                        Console.ResetColor();
                        Console.Write(" " + nr + " "); //numbers and some spacing

                        if(j == 9)
                        {
                            nr--;
                        }
                    }
                    else if(i < 9 && j < 9) //inside the board
                    {
                        if((i + j) %2 == 0) //if sum is even then the color of the square should be white (light colour)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                        }
                        else //if sum is odd then the color of the square should be black (dark colour)
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;                            
                        }

                        if(game[i - 1, j - 1].Contains("'")) //if black piece
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else //if white piece
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        if(game[i - 1, j - 1].Contains("K") || game[i - 1, j - 1].Contains("Q")) //king or queen
                        {
                            if(!game[i - 1, j - 1].Contains("1") && !game[i - 1, j - 1].Contains("2")) //if isnt knight
                            {
                                if(game[i - 1, j - 1].Contains("'")) //if black piece
                                {
                                    // Console.Write(" " + game[i - 1, j - 1]);
                                    if(game[i - 1, j - 1].Contains("Q"))
                                    {
                                        Console.Write(" ♛ ");
                                    }
                                    else
                                    {
                                        Console.Write(" ♚ ");
                                    }
                                }
                                else //if white piece
                                {
                                    // Console.Write(" " + game[i - 1, j - 1] + " ");
                                    if(game[i - 1, j - 1].Contains("Q"))
                                    {
                                        Console.Write(" ♛ ");
                                    }
                                    else
                                    {
                                        Console.Write(" ♚ ");
                                    }
                                }
                            }
                            else //if is knight
                            {
                                if(game[i - 1, j - 1].Contains("'"))
                                {
                                    //Console.Write(game[i - 1, j - 1]);
                                    Console.Write(" ♞ ");
                                }
                                else
                                {
                                    //Console.Write(game[i - 1, j - 1] + " ");
                                    Console.Write(" ♞ ");
                                }
                            }
                        }
                        else //any other piece
                        {
                            if(game[i - 1, j - 1].Contains("'")) //if black piece
                            {
                                // Console.Write(game[i - 1, j - 1]);

                                if(game[i - 1, j - 1].Contains("P"))
                                {
                                    Console.Write(" ♟");
                                }
                                else if(game[i - 1, j - 1].Contains("B"))
                                {   
                                    Console.Write(" ♝ ");
                                }
                                else
                                {
                                    Console.Write(" ♜ ");
                                }
                            }
                            else //if white piece or empty square
                            {
                                // Console.Write(game[i - 1, j - 1] + " "); 

                                if(game[i - 1, j - 1].Contains("P"))
                                {
                                    Console.Write(" ♟");
                                }
                                else if(game[i - 1, j - 1].Contains("B"))
                                {   
                                    Console.Write(" ♝ ");
                                }
                                else if(game[i - 1, j - 1].Contains("R"))
                                {
                                    Console.Write(" ♜ ");
                                }
                                else
                                {
                                    Console.Write(game[i - 1, j - 1] + " "); 
                                }
                            }
                        }
                    }
                }

                Console.ResetColor();
                Console.WriteLine();
            }
        }

        public static void Game(Dictionary<string,string> movesCoordinates, List<string> capturedWhite, List<string> capturedBlack, string[,] game, int turn, int errorCode)
        {
            string pieceToMove = ""; //pawn, rook, bishop, knight, etc..
            string pieceToMoveCoordinates; //current position of piece
            string coordinates = ""; //destination position of piece
            string matrixCoordinate; //gets x and y position from coordinates
            int x = 0, y = 0; //for the matrix

            string[] validCoordinates = {
                "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8",
                "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8",
                "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8",
                "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8",
                "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8",
                "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8",
                "G1", "G2", "G3", "G4", "G5", "G6", "G7", "G8",
                "H1", "H2", "H3", "H4", "H5", "H6", "H7", "H8",
            };

            printGame(game);

            Console.WriteLine();

            if(errorCode != 0)
            {
                print.printError(errorCode);
            }

            //turn = 1; //for testing purposes

            if(turn == 1) //whites turn
            {
                Console.WriteLine("White, enter the current coordinates of the piece you want to move (type h for help): ");
                pieceToMoveCoordinates = Console.ReadLine();

                if(pieceToMoveCoordinates == "oo" || pieceToMoveCoordinates == "ooo") //kingside or queenside castling
                {
                    pieceToMove = "K";
                }
                else
                {
                    pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper(); //uppercase

                    while(!validCoordinates.Contains(pieceToMoveCoordinates)) //while coordinates arent valid
                    {
                        Console.WriteLine("Enter valid coordinates (type h for help): ");
                        pieceToMoveCoordinates = Console.ReadLine();

                        if(pieceToMoveCoordinates == "oo" || pieceToMoveCoordinates == "ooo") //if castle
                        {
                            break;
                        }
                        
                        while(pieceToMoveCoordinates == "h")
                        {
                            print.printHelpSelect();
                            Console.WriteLine("Enter coordinates (type h for help): ");
                            pieceToMoveCoordinates = Console.ReadLine();
                        }

                        pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper(); //uppercase
                    }
                }

                if(pieceToMoveCoordinates != "oo" && pieceToMoveCoordinates != "ooo") //if not kingside or queenside castling
                {
                    while(pieceToMoveCoordinates == "h")
                    {
                        print.printHelpSelect();
                        Console.WriteLine("Enter coordinates (type h for help): ");
                        pieceToMoveCoordinates = Console.ReadLine();
                    }

                    pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper(); //uppercase

                    matrixCoordinate = movesCoordinates[pieceToMoveCoordinates]; //gets the coordinates after checking they exist

                    x = Convert.ToInt32(matrixCoordinate.Split(",")[0].Trim());
                    y = Convert.ToInt32(matrixCoordinate.Split(",")[1].Trim());

                    while(game[x, y] == "  " || game[x, y].Contains("'")) //while the coordinates are empty or black piece is selected
                    {
                        Console.WriteLine("Enter valid coordinates (type h for help): ");
                        pieceToMoveCoordinates = Console.ReadLine();
                        
                        if(pieceToMoveCoordinates == "h")
                        {
                            print.printHelpSelect();
                        }

                        pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper();

                        while(!validCoordinates.Contains(pieceToMoveCoordinates)) //while coordinates arent valid
                        {
                            Console.WriteLine("Enter valid coordinates (type h for help): ");
                            pieceToMoveCoordinates = Console.ReadLine();
                        
                            while(pieceToMoveCoordinates == "h")
                            {
                                print.printHelpSelect();
                                Console.WriteLine("Enter coordinates (type h for help): ");
                                pieceToMoveCoordinates = Console.ReadLine();
                            }

                            pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper();
                        }

                        matrixCoordinate = movesCoordinates[pieceToMoveCoordinates]; //gets the coordinates after checking they exist

                        x = Convert.ToInt32(matrixCoordinate.Split(",")[0].Trim());
                        y = Convert.ToInt32(matrixCoordinate.Split(",")[1].Trim());
                    }

                    pieceToMove = game[x, y]; //selects the piece
                }
            }
            else
            {
                Console.WriteLine("Black, enter the current coordinates of the piece you want to move (type h for help): ");
                pieceToMoveCoordinates = Console.ReadLine();

                if(pieceToMoveCoordinates == "oo" || pieceToMoveCoordinates == "ooo") //kingside or queenside castling
                {
                    pieceToMove = "K'";
                }
                else
                {
                    pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper();

                    while(!validCoordinates.Contains(pieceToMoveCoordinates)) //while coordinates arent valid
                    {
                        Console.WriteLine("Enter valid coordinates (type h for help): ");
                        pieceToMoveCoordinates = Console.ReadLine();

                        if(pieceToMoveCoordinates == "oo" || pieceToMoveCoordinates == "ooo")
                        {
                            break;
                        }
                        
                        while(pieceToMoveCoordinates == "h")
                        {
                            print.printHelpSelect();
                            Console.WriteLine("Enter coordinates (type h for help): ");
                            pieceToMoveCoordinates = Console.ReadLine();
                        }
    
                        pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper();
                    }
                }

                if(pieceToMoveCoordinates != "oo" && pieceToMoveCoordinates != "ooo") //if not kingside or queenside castling
                {
                    while(pieceToMoveCoordinates == "h")
                    {
                        print.printHelpSelect();
                        Console.WriteLine("Enter coordinates (type h for help): ");
                        pieceToMoveCoordinates = Console.ReadLine();
                    }
                
                    pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper();
    
                    matrixCoordinate = movesCoordinates[pieceToMoveCoordinates]; //gets the coordinates after checking they exist
    
                    x = Convert.ToInt32(matrixCoordinate.Split(",")[0].Trim());
                    y = Convert.ToInt32(matrixCoordinate.Split(",")[1].Trim());
    
                    while(game[x, y] == "  " || !game[x, y].Contains("'")) //while the coordinates are empty or white piece is selected
                    {
                        Console.WriteLine("Enter valid coordinates (type h for help): ");
                        pieceToMoveCoordinates = Console.ReadLine();
    
                        while(pieceToMoveCoordinates == "h")
                        {
                            print.printHelpSelect();
                            Console.WriteLine("Enter coordinates (type h for help): ");
                            pieceToMoveCoordinates = Console.ReadLine();
                        }
    
                        pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper();
    
                        while(!validCoordinates.Contains(pieceToMoveCoordinates)) //while coordinates arent valid
                        {
                            Console.WriteLine("Enter valid coordinates (type h for help): ");
                            pieceToMoveCoordinates = Console.ReadLine();
                            
                            if(pieceToMoveCoordinates == "h")
                            {
                                print.printHelpSelect();
                                Console.WriteLine("Enter coordinates (type h for help): ");
                                pieceToMoveCoordinates = Console.ReadLine();
                            }
    
                            pieceToMoveCoordinates = pieceToMoveCoordinates.ToUpper();
                        }
    
                        matrixCoordinate = movesCoordinates[pieceToMoveCoordinates]; //gets the coordinates after checking they exist
    
                        x = Convert.ToInt32(matrixCoordinate.Split(",")[0].Trim());
                        y = Convert.ToInt32(matrixCoordinate.Split(",")[1].Trim());
                    }
     
                    pieceToMove = game[x, y]; //selects the piece
                }
            }

            if(pieceToMoveCoordinates != "oo" && pieceToMoveCoordinates != "ooo")
            {
                Console.WriteLine("Enter destiantion coordinates (type h for help): ");
                coordinates = Console.ReadLine();

                while(coordinates == "h")
                {
                    print.printHelpDestination();
                    Console.WriteLine("Enter coordinates (type h for help): ");
                    coordinates = Console.ReadLine();   
                }

                coordinates = coordinates.ToUpper();

                while(!validCoordinates.Contains(coordinates)) //while coordinates arent valid
                {
                    Console.WriteLine("Enter a valid coordinates (type h for help): ");
                    coordinates = Console.ReadLine();

                    while(coordinates == "h")
                    {
                        print.printHelpDestination();
                        Console.WriteLine("Enter coordinates (type h for help): ");
                        coordinates = Console.ReadLine();
                    }

                    coordinates = coordinates.ToUpper();
                }

                matrixCoordinate = movesCoordinates[coordinates]; //gets the coordinates after checking they exist

                x = Convert.ToInt32(matrixCoordinate.Split(",")[0].Trim());
                y = Convert.ToInt32(matrixCoordinate.Split(",")[1].Trim());

                errorCode = checkIfMoveIsLegal.check(game, pieceToMoveCoordinates, coordinates, pieceToMove); //checks for errors
            }
            else
            {
                errorCode = checkIfMoveIsLegal.check(game, pieceToMoveCoordinates, "", pieceToMove); //checks for errors
            }

            if(pieceToMoveCoordinates != "oo" && pieceToMoveCoordinates != "ooo" && errorCode == 0) //if no castling
            {
                string[,] gameBackup = new string[8, 8];
                string newPiece = "  "; //"  " by default

                Array.Copy(game, gameBackup, game.Length); //copies game to gameBackup in case of illegal moves

                if(pieceToMove.Contains('P'))
                {
                    if(turn == 1) //whites turn
                    {
                        if(coordinates.Contains('8'))
                        {
                            //promotion
                            //Console.WriteLine("Choose piece to promote to: ");
                            //newPiece = Console.ReadLine();
                        }
                    }
                    else //blacks turn
                    {
                        if(coordinates.Contains('1'))
                        {
                            //promotion
                            //Console.WriteLine("Choose piece to promote to: ");
                            //newPiece = Console.ReadLine();
                        }
                    }
                }

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if(game[i, j] == pieceToMove)
                        {
                            game[i, j] = newPiece; //replaces piece with empty place
                        }

                        if(enPassantExecuted)
                        {
                            if(game[i, j] == lastPieceMoved)
                            {
                                game[i, j] = newPiece; //replaces piece with empty place
                                enPassantExecuted = false;
                            }
                        }
                    }
                }

                game[x, y] = pieceToMove; //replaces destiantion with piece

                if(checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 0, 0)) //if king is under attack
                {
                    game = gameBackup; //revert move cuz it was illegal

                    if(errorCode == 0) //if no error with the move but the piece is pinned
                    {
                        if(underCheck) //if under check
                        {
                            errorCode = 2;
                        }
                        else
                        {
                            errorCode = 7;
                        }
                    }
                }
            }
            else if(errorCode == 0)//Castling
            {   
                if(turn == 1) //checks if path where king moves is attacked (white)
                {
                    if(pieceToMoveCoordinates == "oo")
                    {
                        if(checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 7, 5) || checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 7, 6))
                        {
                            errorCode = 5;                            
                        }
                    }
                    else
                    {
                        if(checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 7, 1) || checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 7, 2) || checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 7, 3))
                        {
                            errorCode = 5;                            
                        }
                    }
                }
                else //checks if path where king moves is attacked (black)
                {
                    if(pieceToMoveCoordinates == "oo")
                    {
                        if(checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 0, 5) || checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 0, 6))
                        {
                            errorCode = 5;                            
                        }
                    }
                    else
                    {
                        if(checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 0, 1) || checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 0, 2) || checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, turn, pieceToMoveCoordinates, 0, 3))
                        {
                            errorCode = 5;                            
                        }
                    }
                }

                if(errorCode == 0) //castle if no errors
                {
                    if(turn == 1)
                    {
                        if(pieceToMoveCoordinates == "oo")
                        {
                            game[7, 4] = "  ";
                            game[7, 5] = "R2";
                            game[7, 6] = "K";
                            game[7, 7] = "  ";
                        }
                        else //ooo
                        {
                            game[7, 0] = "  ";
                            game[7, 1] = "  ";
                            game[7, 2] = "K";
                            game[7, 3] = "R1";
                            game[7, 4] = "  ";
                        }
                    }
                    else
                    {
                        if(pieceToMoveCoordinates == "oo")
                        {
                            game[0, 4] = "  ";
                            game[0, 5] = "R2'";
                            game[0, 6] = "K'";
                            game[0, 7] = "  ";
                        }
                        else //ooo
                        {
                            game[0, 0] = "  ";
                            game[0, 1] = "  ";
                            game[0, 2] = "K'";
                            game[0, 3] = "R1'";
                            game[0, 4] = "  ";
                        }
                    }
                }
            }

            if(errorCode == 0)
            {
                if(pieceToMove.Contains("R") && pieceToMove.Contains("1")) //no queenside white if rook moves
                {
                    if(turn == 1)
                    {
                        whiteQueensideCastling = false;
                    }
                    else
                    {
                        blackQueensideCastling = false;
                    }
                }
                else if(pieceToMove.Contains("R") && pieceToMove.Contains("2")) //no kingside white if rook moves
                {
                    if(turn == 1)
                    {
                        whiteKingsideCastling = false;
                    }
                    else
                    {
                        blackKingsideCastling = false;
                    }
                }

                if(pieceToMove.Contains("K") && pieceToMoveCoordinates != "oo" && pieceToMoveCoordinates != "ooo") //make castling illegal after moving king
                {
                    if(!pieceToMove.Contains("1") && !pieceToMove.Contains("2")) //is king
                    {
                        if(turn == 1) //white
                        {   
                            whiteKingsideCastling = false;
                            whiteQueensideCastling = false;
                        }
                        else //black
                        {
                            blackKingsideCastling = false;
                            blackQueensideCastling = false;
                        }
                    }
                }

                if(underCheck) //you get out of check if you make a legal move while you were under check
                {
                    underCheck = false;
                }

                if(turn == 1) 
                {
                    if(checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, 2, pieceToMoveCoordinates, 0, 0)) //checks for check while inverse checking opponents king
                    {
                        underCheck = true;
                        errorCode = 77;
                    }
                }
                else
                {
                    if(checkIfMoveIsLegal.checkIfSquareIsUnderAttack(game, 1, pieceToMoveCoordinates, 0, 0)) //checks for check while inverse checking opponents king
                    {
                        underCheck = true;
                        errorCode = 77;
                    }
                }

                lastPieceMoved = pieceToMove;
                turn = turn == 1 ? 2 : 1; //if whites turn, then nexts is black, else is whites next
            }

            Console.Clear(); //Refreshes
            Game(movesCoordinates, capturedWhite, capturedBlack, game, turn, errorCode); //restarts game
        }

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Dictionary<string,string> movesCoordinates= DictionaryReturn.returnDictionary(); //for example a1 is 7, 0 in the matrix
            List<string> capturedWhite = new List<string>(); //what black has captured from white
            List<string> capturedBlack = new List<string>(); //what white has captured from black
 
            string[,] game = new string[8, 8]{
                {"R1'", "K1'", "B1'", "Q'", "K'", "B2'", "K2'", "R2'"},
                {"P1'", "P2'", "P3'", "P4'", "P5'", "P6'", "P7'", "P8'"},
                {"  ", "  ", "  ", "  ", "  ", "  ", "  ", "  "},
                {"  ", "  ", "  ", "  ", "  ", "  ", "  ", "  "},
                {"  ", "  ", "  ", "  ", "  ", "  ", "  ", "  "},
                {"  ", "  ", "  ", "  ", "  ", "  ", "  ", "  "},
                {"P1", "P2", "P3", "P4", "P5", "P6", "P7", "P8"},
                {"R1", "K1", "B1", "Q", "K", "B2", "K2", "R2"},
            }; //game :)

            Game(movesCoordinates, capturedWhite, capturedBlack, game, 1, 0); //start game
        }
    }
}