using System;
using System.Threading;
using GameField;

// масштаб одной игровой клетки 4 на 2 (ввиду размеров ячейки консоли, к примеру квадарт будет 8 на 4)
// все методы класcа Figure потоконебезопасные, ответственность за безопасность берут на себя классы наследники

namespace GameFigure
{
    public struct Vector2
    {
        public int x;
        public int y;
    }

    public class Figure
    {
        public static ConsoleColor FigureColor { get; set; }
        protected static int speed; // скорость падения
        protected Mutex mutexMovement;
        internal Vector2 coord; // координаты фигуры
        internal Vector2 size; // размеры фиугры
        protected bool orientation; // false - горизонтальное положение, true - вертикальное

        public virtual int YCoord // для проверки заполнености линий поля необходим y
        {
            get { return coord.y; }
        }
        public virtual int YSize
        {
            get { return size.y; }
        }

        static Figure()
        {
            FigureColor = ConsoleColor.Red;
            speed = 0;
        }
        public Figure()
        {
            mutexMovement = new Mutex();
        }

        public Figure(int sizeX, int sizeY) : this()
        {
            size.x = sizeX;
            size.y = sizeY;
            orientation = false;
            coord.x = (50 - size.x) / 2 + 5; // центровка начального положения фигуры (50 - диапазон поля, 5 - отступ)
            coord.y = 2;

            Console.BackgroundColor = FigureColor; // этот процесс можно было бы упаковать в метод (так как он повторяется несколько раз), однако при этом перерисовка фигур происходит с бОльшим мерцанием
            Console.ForegroundColor = FigureColor;
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(coord.x, coord.y + i);
                for (int k = 0; k < size.x; k++)
                {
                    Field.FillGridIn(coord.y + i, coord.x + k); // вносим фигуру в матрицу игрового поля
                    Console.Write('0');
                }
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        internal Figure(int sizeX, int sizeY, int coordX, int coordY) : this() // специальный конструктор для составных фигур, 
        {                                                                      // начальные координаты расположения в них задаются вручную
            size.x = sizeX;
            size.y = sizeY;
            orientation = false;
            coord.x = coordX;
            coord.y = coordY;

            Console.BackgroundColor = FigureColor;
            Console.ForegroundColor = FigureColor;
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(coord.x, coord.y + i);
                for (int k = 0; k < size.x; k++)
                {
                    Field.FillGridIn(coord.y + i, coord.x + k);
                    Console.Write('0');
                }
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public void DeleteFigure() // с помощью этих двух методов осуществляется перерисовка фигуры без её смещения (повороты)
        {
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(coord.x, coord.y + i);
                for (int k = 0; k < size.x; k++)
                {
                    Field.ClearGridIn(coord.y + i, coord.x + k);
                    Console.Write(' ');
                }
            }
        }
        public void DrawFigure()
        {
            Console.BackgroundColor = FigureColor;
            Console.ForegroundColor = FigureColor;
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(coord.x, coord.y + i);
                for (int k = 0; k < size.x; k++)
                {
                    Field.FillGridIn(coord.y + i, coord.x + k);
                    Console.Write('0');
                }
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        public virtual void MoveDown(int milliseconds = 500)
        {
            Console.SetCursorPosition(coord.x, coord.y);
            for (int k = 0; k < size.x; k++) // очищаем верхнюю строку фигуры
            {
                Field.ClearGridIn(coord.y, coord.x + k);
                Console.Write(' ');
            }
            coord.y++;
            Console.SetCursorPosition(coord.x, coord.y + size.y - 1);
            Console.BackgroundColor = FigureColor;
            Console.ForegroundColor = FigureColor;
            for (int k = 0; k < size.x; k++) // добавляем нижнюю
            {
                Field.FillGridIn(coord.y + size.y - 1, coord.x + k);
                Console.Write('0');
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public virtual void MoveDownBoost()
        {
            while (!OnBottom())
            {
                MoveDown();
                Thread.Sleep(5);
            }
        }

        public virtual void MoveRight()
        {
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(coord.x, coord.y + i);
                for (int k = 0; k < 4; k++)
                {
                    Field.ClearGridIn(coord.y + i, coord.x + k);
                    Console.Write(' ');
                }
            }
            coord.x += 4; // ход любой фигуры составляет 4 клетки
            Console.BackgroundColor = FigureColor;
            Console.ForegroundColor = FigureColor;
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(coord.x + size.x - 4, coord.y + i);
                for (int k = 0; k < 4; k++)
                {
                    Field.FillGridIn(coord.y + i, coord.x + size.x - 4 + k);
                    Console.Write('0');
                }
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public virtual void MoveLeft()
        {
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(coord.x + size.x - 4, coord.y + i);
                for (int k = 0; k < 4; k++)
                {
                    Field.ClearGridIn(coord.y + i, coord.x + size.x - 4 + k);
                    Console.Write(' ');
                }
            }
            coord.x -= 4;
            Console.BackgroundColor = FigureColor;
            Console.ForegroundColor = FigureColor;
            for (int i = 0; i < size.y; i++)
            {
                Console.SetCursorPosition(coord.x, coord.y + i);
                for (int k = 0; k < 4; k++)
                {
                    Field.FillGridIn(coord.y + i, coord.x + k);
                    Console.Write('0');
                }
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        public virtual void RotateRight() { }
        public virtual void RotateLeft() { }
        public virtual bool OnBottom()
        {
            if (Field.IsEmptyInRange(coord.y + size.y, coord.y + size.y, coord.x, coord.x + size.x - 1))
                return false;

            return true;
        }
    }
}
