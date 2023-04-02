using System;
using System.Linq;
using System.Collections.Generic;

namespace Chess
{
    class checkIfMoveIsLegal //checks if moves are legal :)
    {
        public static int check(string[,] game, string currentPos, string destinationPos, string pieceToMove)
        {
            Dictionary<string,string> movesCoordinates= DictionaryReturn.returnDictionary(); //for example a1 is 7, 0 in the matrix
            int currentX = 0, currentY = 0, desX = 0, desY = 0; //current x and y, destiantion x and y
            int turn;

            if(pieceToMove.Contains("'")) //BLACK
            {
                turn = 2;
            }
            else //WHITE
            {
                turn = 1;
            }

            if(currentPos != "oo" && currentPos != "ooo")
            {
                currentPos = movesCoordinates[currentPos];
                destinationPos = movesCoordinates[destinationPos];

                currentX = Convert.ToInt32(currentPos.Split(",")[0].Trim());
                currentY = Convert.ToInt32(currentPos.Split(",")[1].Trim());

                desX = Convert.ToInt32(destinationPos.Split(",")[0].Trim());
                desY = Convert.ToInt32(destinationPos.Split(",")[1].Trim());
                
                if(pieceToMove.Contains("'")) //BLACK
                {
                    if(game[desX, desY].Contains("'"))
                    {
                        return 1;
                    }
                }
                else //WHITE
                {
                    if(!game[desX, desY].Contains("'") && game[desX, desY] != "  ")
                    {
                        return 1;
                    }
                }
            }

            if(pieceToMove.Contains("P")) //PAWN
            {
                if(pieceToMove.Contains("'")) //BLACK PAWN
                {
                    if(desX - currentX == 1) //one forward
                    {
                        if(desY == currentY) //forward
                        {
                            if(game[desX, desY] != "  ")
                            {
                                return 7;
                            }
                        }
                        else if(desY - currentY == -1 || desY - currentY == 1) //diagonal 
                        {
                            if(!game[desX, desY].Contains("'") && game[desX, desY] == "  ") //en passant
                            {
                                if(desY - currentY == -1) //SE
                                {
                                    if (Program.lastPieceMoved != game[currentX, currentY - 1])
                                    {
                                        return 7;
                                    }   
                                    else
                                    {
                                        Program.enPassantExecuted = true;
                                    }
                                }
                                else //(desY - currentY == 1) //SW
                                {
                                    if(Program.lastPieceMoved != game[currentX, currentY + 1])
                                    {
                                        return 7;
                                    }
                                    else
                                    {
                                        Program.enPassantExecuted = true;
                                    }
                                }
                            }
                            else if(game[desX, desY].Contains("'") || game[desX, desY] == "  ")
                            {
                                return 7;
                            }
                        }
                        else
                        {
                            return 7;
                        }
                    }
                    else if(currentY == desY && desX - currentX == 2 && currentX == 1 && game[desX, desY] == "  " && game[desX - 1, desY] == "  ") //two forward
                    {
                        //return 0;
                    }
                    else
                    {
                        return 7;
                    }
                }
                else //WHITE PAWN
                {
                    if(desX - currentX == -1) //one forward
                    {
                        if(desY == currentY) //forward
                        {
                            if(game[desX, desY] != "  ")
                            {
                                return 7;
                            }
                        }
                        else if(desY - currentY == -1 || desY - currentY == 1) //diagonal 
                        {
                            if(!game[desX, desY].Contains("'") && game[desX, desY] == "  ") //en passant
                            {
                                if(desY - currentY == -1) //NE
                                {
                                    if (Program.lastPieceMoved != game[currentX, currentY - 1])
                                    {
                                        return 7;
                                    }   
                                    else
                                    {
                                        Program.enPassantExecuted = true;
                                    }
                                }
                                else //(desY - currentY == 1) //NW
                                {
                                    if(Program.lastPieceMoved != game[currentX, currentY + 1])
                                    {
                                        return 7;
                                    }
                                    else
                                    {
                                        Program.enPassantExecuted = true;
                                    }
                                }
                            }
                            else if(!game[desX, desY].Contains("'") || game[desX, desY] == "  ")
                            {
                                return 7;
                            }
                        }
                        else
                        {
                            return 7;
                        }
                    }
                    else if(currentY == desY && desX - currentX == -2 && currentX == 6 && game[desX, desY] == "  " && game[desX + 1, desY] == "  ") //two forward
                    {
                        //return 0;
                    }
                    else
                    {
                        return 7;
                    }
                }
            }
            else if(pieceToMove.Contains("K") && currentPos != "oo" && currentPos != "ooo") //KING OR KNIGHT
            {
                if(pieceToMove.Contains("1") || pieceToMove.Contains("2")) //KNIGHT
                {
                    if(Math.Abs(desX - currentX) == 2) //two forward or back
                    {
                        if(Math.Abs(desY - currentY) != 1) //only one left or right
                        {
                            return 7;
                        }
                    }
                    else if(Math.Abs(desX - currentX) == 1) //one forward or back
                    {
                        if(Math.Abs(desY - currentY) != 2) //only two left or right
                        {
                            return 7;
                        }
                    }
                    else
                    {
                        return 7;
                    }
                }
                else //KING
                {
                    if(Math.Abs(desX - currentX) == 1) //one forward or back
                    {
                        if(desY != currentY && Math.Abs(desY - currentY) != 1) 
                        {
                            return 7;
                        }
                    }
                    else if(Math.Abs(desY - currentY) == 1) //one left or right
                    {
                        if(desX != currentX)
                        {
                            return 7;
                        }
                    }
                }
            }
            else if(pieceToMove.Contains("B")) //BISHOP
            {
                int a, b;

                if(desX + desY == currentX + currentY) //↙ or ↗
                {
                    if(desX + desY < currentX + currentY) //↙
                    {
                        List<int> toCheckXSW = new List<int>(); // south west
                        List<int> toCheckYSW = new List<int>();

                        a = currentX; 
                        b = currentY;

                        while (a < 7 && b > 0)
                        {
                            toCheckXSW.Add(++a);
                            toCheckYSW.Add(--b);
                        }

                        for (int i = 0; i < toCheckXSW.Count; i++)
                        {
                            if(game[toCheckXSW[i], toCheckYSW[i]] != "  " && desX - desY < toCheckXSW[i] - toCheckYSW[i])
                            {
                                return 7;
                            }   
                        }
                    }
                    else //↗
                    {
                        List<int> toCheckXNE = new List<int>(); // north east
                        List<int> toCheckYNE = new List<int>();

                        a = currentX; 
                        b = currentY;

                        while (a > 0 && b < 7)
                        {
                            toCheckXNE.Add(--a);
                            toCheckYNE.Add(++b);
                        }

                        for (int i = 0; i < toCheckXNE.Count; i++)
                        {
                            if(game[toCheckXNE[i], toCheckYNE[i]] != "  " && desX - desY < toCheckXNE[i] - toCheckYNE[i])
                            {
                                return 7;
                            }   
                        }
                    }
                }
                else if(desX - desY == currentX - currentY) //↘ or ↖
                {
                    if(desX + desY < currentX + currentY) //↖
                    {
                        List<int> toCheckXNW = new List<int>(); // north west
                        List<int> toCheckYNW = new List<int>();

                        a = currentX; 
                        b = currentY;

                        while (a > 0 && b > 0)
                        {
                            toCheckXNW.Add(--a);
                            toCheckYNW.Add(--b);
                        }

                        for (int i = 0; i < toCheckXNW.Count; i++)
                        {
                            if(game[toCheckXNW[i], toCheckYNW[i]] != "  " && desX + desY < toCheckXNW[i] + toCheckYNW[i])
                            {
                                return 7;
                            }   
                        }
                    }
                    else //↘
                    {
                        List<int> toCheckXSE = new List<int>(); // south east
                        List<int> toCheckYSE = new List<int>();

                        a = currentX;
                        b = currentY;

                        while (a < 7 && b < 7)
                        {
                            toCheckXSE.Add(++a);
                            toCheckYSE.Add(++b);
                        }
                        
                        for (int i = 0; i < toCheckXSE.Count; i++)
                        {
                            if(game[toCheckXSE[i], toCheckYSE[i]] != "  " && desX + desY > toCheckXSE[i] + toCheckYSE[i])
                            {
                                return 7;
                            }   
                        }
                    }
                }
                else
                {
                    return 7;
                }
            }
            else if(pieceToMove.Contains("R")) //ROOK
            {
                int a;

                if(desX == currentX) // → or ←
                {
                    if(desY > currentY) // →
                    {
                        List<int> toCheckE = new List<int>(); // east
                        a = currentY;

                        while (a < 7)
                        {
                            toCheckE.Add(++a);
                        }

                        for (int i = 0; i < toCheckE.Count; i++)
                        {
                            if(game[currentX, toCheckE[i]] != "  " && desX + desY > toCheckE[i] + currentX)
                            {
                                return 7;
                            }   
                        }
                    }
                    else // ←
                    {
                        List<int> toCheckW = new List<int>(); // west
                        a = currentY;

                        while (a > 0)
                        {
                            toCheckW.Add(--a);
                        }

                        for (int i = 0; i < toCheckW.Count; i++)
                        {
                            if(game[currentX, toCheckW[i]] != "  " && desX + desY < toCheckW[i] + currentX)
                            {
                                return 7;
                            }   
                        }
                    }
                }
                else if(desY == currentY) // ↓ or ↑
                {
                    if(desX > currentX) // ↓
                    {
                        List<int> toCheckS = new List<int>(); // south
                        a = currentX;

                        while (a < 7)
                        {
                            toCheckS.Add(++a);
                        }

                        for (int i = 0; i < toCheckS.Count; i++)
                        {
                            if(game[toCheckS[i], currentY] != "  " && desX + desY > toCheckS[i] + currentY)
                            {
                                return 7;
                            }   
                        }
                    }
                    else // ↑
                    {
                        List<int> toCheckN = new List<int>(); // north
                        a = currentX;

                        while (a > 0)
                        {
                            toCheckN.Add(--a);
                        }

                        for (int i = 0; i < toCheckN.Count; i++)
                        {
                            if(game[toCheckN[i], currentY] != "  " && desX + desY < toCheckN[i] + currentY)
                            {
                                return 7;
                            }   
                        }
                    }
                }
                else
                {
                    return 7;
                }
            }
            else if(pieceToMove.Contains("Q")) //QUEEN
            {
                int a, b;

                if(desX + desY == currentX + currentY) //↙ or ↗
                {
                    if(desX + desY < currentX + currentY) //↙
                    {
                        List<int> toCheckXSW = new List<int>(); // south west
                        List<int> toCheckYSW = new List<int>();

                        a = currentX; 
                        b = currentY;

                        while (a < 7 && b > 0)
                        {
                            toCheckXSW.Add(++a);
                            toCheckYSW.Add(--b);
                        }

                        for (int i = 0; i < toCheckXSW.Count; i++)
                        {
                            if(game[toCheckXSW[i], toCheckYSW[i]] != "  " && desX - desY < toCheckXSW[i] - toCheckYSW[i])
                            {
                                return 7;
                            }   
                        }
                    }
                    else //↗
                    {
                        List<int> toCheckXNE = new List<int>(); // north east
                        List<int> toCheckYNE = new List<int>();

                        a = currentX; 
                        b = currentY;

                        while (a > 0 && b < 7)
                        {
                            toCheckXNE.Add(--a);
                            toCheckYNE.Add(++b);
                        }

                        for (int i = 0; i < toCheckXNE.Count; i++)
                        {
                            if(game[toCheckXNE[i], toCheckYNE[i]] != "  " && desX - desY < toCheckXNE[i] - toCheckYNE[i])
                            {
                                return 7;
                            }   
                        }
                    }
                }
                else if(desX - desY == currentX - currentY) //↘ or ↖
                {
                    if(desX + desY < currentX + currentY) //↖
                    {
                        List<int> toCheckXNW = new List<int>(); // north west
                        List<int> toCheckYNW = new List<int>();

                        a = currentX; 
                        b = currentY;

                        while (a > 0 && b > 0)
                        {
                            toCheckXNW.Add(--a);
                            toCheckYNW.Add(--b);
                        }

                        for (int i = 0; i < toCheckXNW.Count; i++)
                        {
                            if(game[toCheckXNW[i], toCheckYNW[i]] != "  " && desX + desY < toCheckXNW[i] + toCheckYNW[i])
                            {
                                return 7;
                            }   
                        }
                    }
                    else //↘
                    {
                        List<int> toCheckXSE = new List<int>(); // south east
                        List<int> toCheckYSE = new List<int>();

                        a = currentX;
                        b = currentY;

                        while (a < 7 && b < 7)
                        {
                            toCheckXSE.Add(++a);
                            toCheckYSE.Add(++b);
                        }
                        
                        for (int i = 0; i < toCheckXSE.Count; i++)
                        {
                            if(game[toCheckXSE[i], toCheckYSE[i]] != "  " && desX + desY > toCheckXSE[i] + toCheckYSE[i])
                            {
                                return 7;
                            }   
                        }
                    }
                }
                else if(desX == currentX) // → or ←
                {
                    if(desY > currentY) // →
                    {
                        List<int> toCheckE = new List<int>(); // east
                        a = currentY;

                        while (a < 7)
                        {
                            toCheckE.Add(++a);
                        }

                        for (int i = 0; i < toCheckE.Count; i++)
                        {
                            if(game[currentX, toCheckE[i]] != "  " && desX + desY > toCheckE[i] + currentX)
                            {
                                return 7;
                            }   
                        }
                    }
                    else // ←
                    {
                        List<int> toCheckW = new List<int>(); // west
                        a = currentY;

                        while (a > 0)
                        {
                            toCheckW.Add(--a);
                        }

                        for (int i = 0; i < toCheckW.Count; i++)
                        {
                            if(game[currentX, toCheckW[i]] != "  " && desX + desY < toCheckW[i] + currentX)
                            {
                                return 7;
                            }   
                        }
                    }
                }
                else if(desY == currentY) // ↓ or ↑
                {
                    if(desX > currentX) // ↓
                    {
                        List<int> toCheckS = new List<int>(); // north
                        a = currentX;

                        while (a < 7)
                        {
                            toCheckS.Add(++a);
                        }

                        for (int i = 0; i < toCheckS.Count; i++)
                        {
                            if(game[toCheckS[i], currentY] != "  " && desX + desY > toCheckS[i] + currentY)
                            {
                                return 7;
                            }   
                        }
                    }
                    else // ↑
                    {
                        List<int> toCheckN = new List<int>(); // north
                        a = currentX;

                        while (a > 0)
                        {
                            toCheckN.Add(--a);
                        }

                        for (int i = 0; i < toCheckN.Count; i++)
                        {
                            if(game[toCheckN[i], currentY] != "  " && desX + desY < toCheckN[i] + currentY)
                            {
                                return 7;
                            }   
                        }
                    }
                }
                else
                {
                    return 7;
                }
            }
            else if(currentPos == "oo")
            {
                if(Program.underCheck)
                {
                    return 6;
                }

                if(turn == 1)
                {

                    if(!Program.whiteKingsideCastling)
                    {
                        return 3;
                    }

                    if(game[7, 5] != "  " || game[7, 6] != "  ")
                    {
                        return 4;
                    }
                }
                else
                {
                    if(!Program.blackKingsideCastling)
                    {
                        return 3;
                    }
                    
                    if(game[0, 5] != "  " || game[0, 6] != "  ")
                    {
                        return 4;
                    }
                }
            }
            else if(currentPos == "ooo")
            {
                if(Program.underCheck)
                {
                    return 6;
                }

                if(turn == 1)
                {
                    if(!Program.whiteQueensideCastling)
                    {
                        return 3;
                    }

                    if(game[7, 1] != "  " || game[7, 2] != "  "  || game[7, 3] != "  ")
                    {
                        return 4;
                    }
                }
                else
                {
                    if(!Program.blackQueensideCastling)
                    {
                        return 3;
                    }
                    
                    if(game[0, 1] != "  " || game[0, 2] != "  " || game[0, 3] != "  ")
                    {
                        return 4;
                    }
                }
            }

            return 0;
        }

        public static bool checkIfSquareIsUnderAttack(string[,] game, int turn, string currentPos, int x, int y)
        {
            int kingsX = 0, kingsY = 0;
            int a, b;

            if(currentPos == "oo" || currentPos == "ooo")
            {
                kingsX = x;
                kingsY = y;
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if(game[i, j] == "K" && turn == 1)
                        {
                            kingsX = i;
                            kingsY = j;
                        }
                        else if(game[i, j] == "K'")
                        {
                            kingsX = i;
                            kingsY = j;
                        }
                    }
                }
            }


            #region Bishop and queen diagonal check

            List<int> toCheckXSW = new List<int>(); // south west
            List<int> toCheckYSW = new List<int>();

            a = kingsX;
            b = kingsY;

            while (a < 7 && b > 0)
            {
                toCheckXSW.Add(++a);
                toCheckYSW.Add(--b);
            }

            for (int i = 0; i < toCheckXSW.Count; i++)
            {
                if(turn == 1)
                {
                    if(!game[toCheckXSW[i], toCheckYSW[i]].Contains("'") && game[toCheckXSW[i], toCheckYSW[i]] != "  ")
                    {
                        break;
                    }
                    else if(game[toCheckXSW[i], toCheckYSW[i]].Contains("'") && !game[toCheckXSW[i], toCheckYSW[i]].Contains("B") && !game[toCheckXSW[i], toCheckYSW[i]].Contains("Q"))
                    {
                        break;
                    }
                    if(game[toCheckXSW[i], toCheckYSW[i]].Contains("B'") || game[toCheckXSW[i], toCheckYSW[i]].Contains("Q'"))
                    {
                        return true;
                    }   
                }
                else
                {
                    if(game[toCheckXSW[i], toCheckYSW[i]].Contains("'"))
                    {
                        break;
                    }
                    else if(!game[toCheckXSW[i], toCheckYSW[i]].Contains("'") && game[toCheckXSW[i], toCheckYSW[i]] != "  " && !game[toCheckXSW[i], toCheckYSW[i]].Contains("B") && !game[toCheckXSW[i], toCheckYSW[i]].Contains("Q"))
                    {
                        break;
                    }
                    else if(game[toCheckXSW[i], toCheckYSW[i]].Contains("B") || game[toCheckXSW[i], toCheckYSW[i]].Contains("Q"))
                    {
                        return true;
                    }   
                }
            }

            List<int> toCheckXNE = new List<int>(); // north east
            List<int> toCheckYNE = new List<int>();

            a = kingsX; 
            b = kingsY;

            while (a > 0 && b < 7)
            {
                toCheckXNE.Add(--a);
                toCheckYNE.Add(++b);
            }

            for (int i = 0; i < toCheckXNE.Count; i++)
            {
                if(turn == 1)
                {
                    if(!game[toCheckXNE[i], toCheckYNE[i]].Contains("'") && game[toCheckXNE[i], toCheckYNE[i]] != "  ")
                    {
                        break;
                    }
                    else if(game[toCheckXNE[i], toCheckYNE[i]].Contains("'") && !game[toCheckXNE[i], toCheckYNE[i]].Contains("B") && !game[toCheckXNE[i], toCheckYNE[i]].Contains("Q"))
                    {
                        break;
                    }
                    if(game[toCheckXNE[i], toCheckYNE[i]].Contains("B'") || game[toCheckXNE[i], toCheckYNE[i]].Contains("Q'"))
                    {
                        return true;
                    }   
                }
                else
                {
                    if(game[toCheckXNE[i], toCheckYNE[i]].Contains("'"))
                    {
                        break;
                    }
                    else if(!game[toCheckXNE[i], toCheckYNE[i]].Contains("'") && game[toCheckXNE[i], toCheckYNE[i]] != "  " && !game[toCheckXNE[i], toCheckYNE[i]].Contains("B") && !game[toCheckXNE[i], toCheckYNE[i]].Contains("Q"))
                    {
                        break;
                    }
                    else if(game[toCheckXNE[i], toCheckYNE[i]].Contains("B") || game[toCheckXNE[i], toCheckYNE[i]].Contains("Q"))
                    {
                        return true;
                    }   
                }
            }

            List<int> toCheckXNW = new List<int>(); // north west
            List<int> toCheckYNW = new List<int>();

            a = kingsX; 
            b = kingsY;

            while (a > 0 && b > 0)
            {
                toCheckXNW.Add(--a);
                toCheckYNW.Add(--b);
            }

            for (int i = 0; i < toCheckXNW.Count; i++)
            {
                if(turn == 1)
                {
                    if(!game[toCheckXNW[i], toCheckYNW[i]].Contains("'") && game[toCheckXNW[i], toCheckYNW[i]] != "  ")
                    {
                        break;
                    }
                    else if(game[toCheckXNW[i], toCheckYNW[i]].Contains("'") && !game[toCheckXNW[i], toCheckYNW[i]].Contains("B") && !game[toCheckXNW[i], toCheckYNW[i]].Contains("Q"))
                    {
                        break;
                    }
                    if(game[toCheckXNW[i], toCheckYNW[i]].Contains("B") || game[toCheckXNW[i], toCheckYNW[i]].Contains("Q"))
                    {
                        return true;
                    }   
                }
                else
                {
                    if(game[toCheckXNW[i], toCheckYNW[i]].Contains("'"))
                    {
                        break;
                    }
                    else if(!game[toCheckXNW[i], toCheckYNW[i]].Contains("'") && game[toCheckXNW[i], toCheckYNW[i]] != "  " && !game[toCheckXNW[i], toCheckYNW[i]].Contains("B") && !game[toCheckXNW[i], toCheckYNW[i]].Contains("Q"))
                    {
                        break;
                    }
                    else if(game[toCheckXNW[i], toCheckYNW[i]].Contains("B") || game[toCheckXNW[i], toCheckYNW[i]].Contains("Q"))
                    {
                        return true;
                    }   
                }
            }

            List<int> toCheckXSE = new List<int>(); // south east
            List<int> toCheckYSE = new List<int>();

            a = kingsX;
            b = kingsY;

            while (a < 7 && b < 7)
            {
                toCheckXSE.Add(++a);
                toCheckYSE.Add(++b);
            }
                        
            for (int i = 0; i < toCheckXSE.Count; i++)
            {
                if(turn == 1)
                {
                    if(!game[toCheckXSE[i], toCheckYSE[i]].Contains("'") && game[toCheckXSE[i], toCheckYSE[i]] != "  ")
                    {
                        break;
                    }
                    else if(game[toCheckXSE[i], toCheckYSE[i]].Contains("'") && !game[toCheckXSE[i], toCheckYSE[i]].Contains("B") && !game[toCheckXSE[i], toCheckYSE[i]].Contains("Q"))
                    {
                        break;
                    }
                    if(game[toCheckXSE[i], toCheckYSE[i]].Contains("B'") || game[toCheckXSE[i], toCheckYSE[i]].Contains("Q'"))
                    {
                        return true;
                    }   
                }
                else
                {
                    if(game[toCheckXSE[i], toCheckYSE[i]].Contains("'"))
                    {
                        break;
                    }
                    else if(!game[toCheckXSE[i], toCheckYSE[i]].Contains("'") && game[toCheckXSE[i], toCheckYSE[i]] != "  " && !game[toCheckXSE[i], toCheckYSE[i]].Contains("B") && !game[toCheckXSE[i], toCheckYSE[i]].Contains("Q"))
                    {
                        break;
                    }
                    else if(game[toCheckXSE[i], toCheckYSE[i]].Contains("B") || game[toCheckXSE[i], toCheckYSE[i]].Contains("Q"))
                    {
                        return true;
                    }   
                }
            }

            #endregion

            #region Rook and Queen rank/file check

            List<int> toCheckE = new List<int>(); // east
            a = kingsY;

            while (a < 7)
            {
                toCheckE.Add(++a);
            }

            for (int i = 0; i < toCheckE.Count; i++)
            {
                if(turn == 1)
                {
                    if(game[kingsX, toCheckE[i]] != "  " && !game[kingsX, toCheckE[i]].Contains("'")) //if white piece accross 
                    {
                        break;
                    }
                    else if(game[kingsX, toCheckE[i]] != "  ") //if black piece
                    {
                        if(game[kingsX, toCheckE[i]].Contains("Q") || game[kingsX, toCheckE[i]].Contains("R"))
                        {
                            return true;
                        }
                        else if(game[kingsX, toCheckE[i]].Contains("'"))
                        {
                            break;
                        }
                    }   
                }
                else
                {
                    if(game[kingsX, toCheckE[i]] != "  " && game[kingsX, toCheckE[i]].Contains("'")) //if black piece accross 
                    {
                        break;
                    }
                    else if(game[kingsX, toCheckE[i]] != "  ") //if white piece
                    {
                        if(game[kingsX, toCheckE[i]].Contains("Q") || game[kingsX, toCheckE[i]].Contains("R"))
                        {
                            return true;
                        }
                        else if(!game[kingsX, toCheckE[i]].Contains("'"))
                        {
                            break;
                        }
                    }   
                }
            }

            List<int> toCheckW = new List<int>(); // west
            a = kingsY;

            while (a > 0)
            {
                toCheckW.Add(--a);
            }

            for (int i = 0; i < toCheckW.Count; i++)
            {
                if(turn == 1)
                {
                    if(game[kingsX, toCheckW[i]] != "  " && !game[kingsX, toCheckW[i]].Contains("'")) //if white piece accross 
                    {
                        break;
                    }
                    else if(game[kingsX, toCheckW[i]] != "  ") //if black piece
                    {
                        if(game[kingsX, toCheckW[i]].Contains("Q") || game[kingsX, toCheckW[i]].Contains("R"))
                        {
                            return true;
                        }
                        else if(game[kingsX, toCheckW[i]].Contains("'"))
                        {
                            break;
                        }
                    }   
                }
                else
                {
                    if(game[kingsX, toCheckW[i]] != "  " && game[kingsX, toCheckW[i]].Contains("'")) //if black piece accross 
                    {
                        break;
                    }
                    else if(game[kingsX, toCheckW[i]] != "  ") //if white piece
                    {
                        if(game[kingsX, toCheckW[i]].Contains("Q") || game[kingsX, toCheckW[i]].Contains("R"))
                        {
                            return true;
                        }
                        else if(!game[kingsX, toCheckW[i]].Contains("'"))
                        {
                            break;
                        }
                    }   
                }
            }

            List<int> toCheckS = new List<int>(); // south
            a = kingsX;

            while (a < 7)
            {
                toCheckS.Add(++a);
            }

            for (int i = 0; i < toCheckS.Count; i++)
            {
                if(turn == 1)
                {
                    if(game[toCheckS[i], kingsY] != "  " && !game[toCheckS[i], kingsY].Contains("'")) //if white piece accross 
                    {
                        break;
                    }
                    else if(game[toCheckS[i], kingsY] != "  ") //if black piece
                    {
                        if(game[toCheckS[i], kingsY].Contains("Q") || game[toCheckS[i], kingsY].Contains("R"))
                        {
                            return true;
                        }
                        else if(game[toCheckS[i], kingsY].Contains("'"))
                        {
                            break;
                        }
                    }   
                }
                else
                {
                    if(game[toCheckS[i], kingsY] != "  " && game[toCheckS[i], kingsY].Contains("'")) //if black piece accross 
                    {
                        break;
                    }
                    else if(game[toCheckS[i], kingsY] != "  ") //if white piece
                    {
                        if(game[toCheckS[i], kingsY].Contains("Q") || game[toCheckS[i], kingsY].Contains("R"))
                        {
                            return true;
                        }
                        else if(!game[toCheckS[i], kingsY].Contains("'"))
                        {
                            break;
                        }
                    }   
                }
            }

            List<int> toCheckN = new List<int>(); // north
            a = kingsX;

            while (a > 0)
            {
                toCheckN.Add(--a);
            }

            for (int i = 0; i < toCheckN.Count; i++)
            {
                if(turn == 1)
                {
                    if(game[toCheckN[i], kingsY] != "  " && !game[toCheckN[i], kingsY].Contains("'")) //if white piece accross 
                    {
                        break;
                    }
                    else if(game[toCheckN[i], kingsY] != "  ") //if black piece
                    {
                        if(game[toCheckN[i], kingsY].Contains("Q") || game[toCheckN[i], kingsY].Contains("R"))
                        {
                            return true;
                        }
                        else if(game[toCheckN[i], kingsY].Contains("'"))
                        {
                            break;
                        }
                    }   
                }
                else
                {
                    if(game[toCheckN[i], kingsY] != "  " && game[toCheckN[i], kingsY].Contains("'")) //if black piece accross 
                    {
                        break;
                    }
                    else if(game[toCheckN[i], kingsY] != "  ") //if white piece
                    {
                        if(game[toCheckN[i], kingsY].Contains("Q") || game[toCheckN[i], kingsY].Contains("R"))
                        {
                            return true;
                        }
                        else if(!game[toCheckN[i], kingsY].Contains("'"))
                        {
                            break;
                        }
                    }   
                }
            }

            #endregion

            #region Knight check

            if(turn == 1)
            {
                try
                {
                    if(game[kingsX + 2, kingsY + 1].Contains("K") && game[kingsX + 2, kingsY + 1].Contains("'"))
                    {
                        if(game[kingsX + 2, kingsY + 1].Contains("1") || game[kingsX + 2, kingsY + 1].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX + 2, kingsY - 1].Contains("K") && game[kingsX + 2, kingsY - 1].Contains("'"))
                    {
                        if(game[kingsX + 2, kingsY - 1].Contains("1") || game[kingsX + 2, kingsY - 1].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX - 2, kingsY - 1].Contains("K") && game[kingsX - 2, kingsY - 1].Contains("'"))
                    {
                        if(game[kingsX - 2, kingsY - 1].Contains("1") || game[kingsX - 2, kingsY - 1].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX - 2, kingsY + 1].Contains("K") && game[kingsX - 2, kingsY + 1].Contains("'"))
                    {
                        if(game[kingsX - 2, kingsY + 1].Contains("1") || game[kingsX - 2, kingsY + 1].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX + 1, kingsY + 2].Contains("K") && game[kingsX + 1, kingsY + 2].Contains("'"))
                    {
                        if(game[kingsX + 1, kingsY + 2].Contains("1") || game[kingsX + 1, kingsY + 2].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX + 1, kingsY - 2].Contains("K") && game[kingsX + 1, kingsY - 2].Contains("'"))
                    {
                        if(game[kingsX + 1, kingsY - 2].Contains("1") || game[kingsX + 1, kingsY - 2].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX - 1, kingsY - 2].Contains("K") && game[kingsX - 1, kingsY - 2].Contains("'"))
                    {
                        if(game[kingsX - 1, kingsY - 2].Contains("1") || game[kingsX - 1, kingsY - 2].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX - 1, kingsY + 2].Contains("K") && game[kingsX - 1, kingsY + 2].Contains("'"))
                    {
                        if(game[kingsX - 1, kingsY + 2].Contains("1") || game[kingsX - 1, kingsY + 2].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }
            }
            else
            {
                try
                {
                    if(game[kingsX + 2, kingsY + 1].Contains("K") && !game[kingsX + 2, kingsY + 1].Contains("'"))
                    {
                        if(game[kingsX + 2, kingsY + 1].Contains("1") || game[kingsX + 2, kingsY + 1].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX + 2, kingsY - 1].Contains("K") && !game[kingsX + 2, kingsY - 1].Contains("'"))
                    {
                        if(game[kingsX + 2, kingsY - 1].Contains("1") || game[kingsX + 2, kingsY - 1].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX - 2, kingsY - 1].Contains("K") && !game[kingsX - 2, kingsY - 1].Contains("'"))
                    {
                        if(game[kingsX - 2, kingsY - 1].Contains("1") || game[kingsX - 2, kingsY - 1].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX - 2, kingsY + 1].Contains("K") && !game[kingsX - 2, kingsY + 1].Contains("'"))
                    {
                        if(game[kingsX - 2, kingsY + 1].Contains("1") || game[kingsX - 2, kingsY + 1].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX + 1, kingsY + 2].Contains("K") && !game[kingsX + 1, kingsY + 2].Contains("'"))
                    {
                        if(game[kingsX + 1, kingsY + 2].Contains("1") || game[kingsX + 1, kingsY + 2].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX + 1, kingsY - 2].Contains("K") && !game[kingsX + 1, kingsY - 2].Contains("'"))
                    {
                        if(game[kingsX + 1, kingsY - 2].Contains("1") || game[kingsX + 1, kingsY - 2].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX - 1, kingsY - 2].Contains("K") && !game[kingsX - 1, kingsY - 2].Contains("'"))
                    {
                        if(game[kingsX - 1, kingsY - 2].Contains("1") || game[kingsX - 1, kingsY - 2].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }

                try
                {
                    if(game[kingsX - 1, kingsY + 2].Contains("K") && !game[kingsX - 1, kingsY + 2].Contains("'"))
                    {
                        if(game[kingsX - 1, kingsY + 2].Contains("1") || game[kingsX - 1, kingsY + 2].Contains("2"))
                        {
                            return true;
                        }
                    }
                }
                catch (System.Exception)
                {

                }
            }

            #endregion

            #region Pawn check

            if(turn == 1)
            {
                try
                {
                    if(game[kingsX - 1, kingsY - 1].Contains("P") && game[kingsX - 1, kingsY - 1].Contains("'"))
                    {
                        return true;
                    }
                    
                    if(game[kingsX - 1, kingsY + 1].Contains("P") && game[kingsX - 1, kingsY + 1].Contains("'"))
                    {
                        return true;
                    }
                }
                catch (System.Exception)
                {
                    
                }
            }
            else
            {
                try
                {
                    if(game[kingsX + 1, kingsY - 1].Contains("P") && !game[kingsX + 1, kingsY - 1].Contains("'"))
                    {
                        return true;
                    }
                    
                    if(game[kingsX + 1, kingsY + 1].Contains("P") && !game[kingsX + 1, kingsY + 1].Contains("'"))
                    {
                        return true;
                    }
                }
                catch (System.Exception)
                {
                    
                }
            }

            #endregion

            return false;
        }
    }
}