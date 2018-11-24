using System;

namespace GameField
{
    public static class Field
    {
        private static bool[,] grid;
        public static bool[,] gridCopy; // копия игрового поля, сохраняемая после каждого хода для сейва (без учета падающей фигуры)
        public static ConsoleColor FieldColor;
        static Field()
        {
            grid = new bool[38, 55]; // поле, определяющее пустая ли клетка (false - пустая)
            // логическое поле сделано больше визуального его представления (так как визуально оно смещено),чтоб индексы массива совпадали с координатами курсора
            for (int i = 0; i < 55; i++)
                grid[37, i] = true; // нижняя линяя true (занята)
            for (int i = 3; i < 38; i++)
                grid[i, 5] = true; // левая стена true 
            for (int i = 3; i < 38; i++)
                grid[i, 54] = true; // правая стена true 
            gridCopy = (bool[,])grid.Clone();
            FieldColor = ConsoleColor.Red;

        }
        public static void DrawMap()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            for (int i = 2; i < 37; i++)
            {
                Console.SetCursorPosition(5, i);
                Console.Write('|');
                Console.SetCursorPosition(54, i);
                Console.Write('|');
            }
            Console.SetCursorPosition(5, 37);
            for (int i = 0; i < 50; i++)
                Console.Write('=');
        }


        public static void FillGridIn(int y, int x)
        {
            grid[y, x] = true;
        }
        public static void ClearGridIn(int y, int x)
        {
            grid[y, x] = false;
        }
        /// <summary>
        /// Метод возвращает истину в случае, если в указанном диапазоне нет занятых ячеек
        /// </summary>
        /// <param name="yFrom"> от строки</param>
        /// <param name="yTo"> до строки</param>
        /// <param name="xFrom"> от столбца</param>
        /// <param name="xTo"> до столбца</param>
        public static bool IsEmptyInRange(int yFrom, int yTo, int xFrom, int xTo)
        {
            if (yFrom < 0 || yTo > 37 || xFrom < 5 || xTo > 54)
                return false;

            for (int i = yFrom; i <= yTo; i++)
                for (int k = xFrom; k <= xTo; k++)
                    if (grid[i, k])
                        return false;

            return true;
        }

        private static bool IsLineFilled(int yLine) // возращает истину, если строка полностью заполнена
        {
            for (int k = 6; k < 55; k++)
                if (!grid[yLine, k])
                    return false;

            return true;
        }

        public static void RedrawField(int yLine) // перерисовывает поле начиная с самой нижний строки(int yLine)
        {
            for (int i = 2; i <= yLine; i++)
            {
                Console.SetCursorPosition(6, i);
                for (int k = 6; k < 54; k++)
                {
                    if (grid[i, k])
                    {
                        Console.BackgroundColor = FieldColor;
                        Console.ForegroundColor = FieldColor;
                        Console.Write('0');
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else
                        Console.Write(' ');
                }
            }
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static int DeleteLine(int figureYCoord, int figureYSize)
        {
            gridCopy = (bool[,])grid.Clone(); // сохраняем состояние поля на момент достижения фигурой дна
            int DeletedLinesAmount = 0;
            bool changesInGrid = false;
            for (int i = 1; i < figureYSize; i += 2)
            {
                if (IsLineFilled(figureYCoord + i)) // проверяем наличие полностью заполненных строк, начиная с верхней строки фигуры (с шагом в два ввиду масштаба)
                {
                    changesInGrid = true;
                    DeletedLinesAmount++;
                    for (int currentY = figureYCoord + i; currentY > 2; currentY--)
                    {
                        for (int currentX = 6; currentX < 54; currentX++)
                        {
                            grid[currentY, currentX] = grid[currentY - 2, currentX];
                        }
                    }
                }
            }
            if (changesInGrid)
                RedrawField(figureYCoord + figureYSize - 1);

            return DeletedLinesAmount;
        }

        public static void ClearGrid()
        {
            for (int i = 0; i < 37; i++)
                for (int k = 6; k < 54; k++)
                {
                    grid[i, k] = false;
                }
        }

        public static void LoadGrid(bool[,] newGrid)
        {
            grid = newGrid;
            DeleteLine(1, 36);
        }
    }
}
