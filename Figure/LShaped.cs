using System.Threading;
using GameField;

namespace GameFigure
{
    public class LShaped : Figure // L - образная фигура      
    {
        protected Figure upPart; // цельня фигура состоит из продолговатого прямоугольника и маленького квадрата
        protected Figure downPart;
        protected bool isReverse; // у L-образной фигуры возможны четыре, а не два положения, R - reverse: обратная сторона

        internal LShaped() { isReverse = false; }
        public LShaped(int sizeX, int sizeY)
        {
            upPart = new Figure(sizeX / 2, sizeY, (50 - sizeX) / 2 + 5, 2);
            downPart = new Figure(sizeX / 2, sizeY / 3, (50 - sizeX) / 2 + 5 + 4, 6);
        }
        public override int YCoord
        {   // возвращаем координаты начала фигуры (верхняя часть составной фигуры) для метода Field.DeleteLine
            get
            {
                return upPart.coord.y;
            }
        }
        public override int YSize
        {
            get
            {
                if ((orientation && isReverse) || (orientation && !isReverse))
                    return downPart.size.y + upPart.size.y; // а также размер составной фигуры
                else if (!orientation && !isReverse)
                    return upPart.size.y;
                else
                    return downPart.size.y;
            }
        }
        public override void MoveDown(int milliseconds = 500)
        {
            mutexMovement.WaitOne();
            if (!OnBottom())
            {
                downPart.MoveDown();
                upPart.MoveDown();
            }
            mutexMovement.ReleaseMutex();
            Thread.Sleep(milliseconds);
        }

        public override void MoveDownBoost()
        {
            mutexMovement.WaitOne();
            while (!OnBottom())
            {
                downPart.MoveDown();
                upPart.MoveDown();
                Thread.Sleep(5);
            }
            mutexMovement.ReleaseMutex();
        }

        public override void MoveLeft()
        {
            mutexMovement.WaitOne();

            // при повороте составных фигур меняется их чек на пустые поля (одна часть фигуры мешает другой части фигуры сделать шаг)
            if (!orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 5,
                upPart.coord.x - 4, upPart.coord.x - 1))
                {
                    upPart.MoveLeft();
                    downPart.MoveLeft();
                }
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y,
                downPart.coord.y + 1, downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    upPart.MoveLeft();
                    downPart.MoveLeft();
                }
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                    upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y + 2,
                    downPart.coord.y + downPart.size.y - 1, downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    upPart.MoveLeft();
                    downPart.MoveLeft();
                }
            }
            else if (orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                    upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y,
                    downPart.coord.y + 1, downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    upPart.MoveLeft();
                    downPart.MoveLeft();
                }
            }
            mutexMovement.ReleaseMutex();
        }

        public override void MoveRight()
        {
            mutexMovement.WaitOne();

            if (!orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 3,
                upPart.coord.x + 4, upPart.coord.x + 7) &&
                Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    downPart.MoveRight();
                    upPart.MoveRight();
                }
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                    upPart.coord.x + upPart.size.x, upPart.coord.x + upPart.size.x + 3) &&
                    Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                    downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    downPart.MoveRight();
                    upPart.MoveRight();
                }
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 5,
                    downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    downPart.MoveRight();
                    upPart.MoveRight();
                }
            }
            else if (orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                    upPart.coord.x + 4, upPart.coord.x + 7) &&
                    Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                    downPart.coord.x + downPart.size.x, downPart.coord.x + downPart.size.x + 3))
                {
                    downPart.MoveRight();
                    upPart.MoveRight();
                }
            }
            mutexMovement.ReleaseMutex();
        }

        public override bool OnBottom()
        {
            mutexMovement.WaitOne();
            bool onBottomValue = true;

            if (!orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                    downPart.coord.x - 4, downPart.coord.x + 3))
                    onBottomValue = false;
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 2,
                 upPart.coord.x + 4, upPart.coord.x + upPart.size.x - 1) &&
                 Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                 downPart.coord.x, downPart.coord.x + 3))
                    onBottomValue = false;
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 2,
                 upPart.coord.x, upPart.coord.x + 3) &&
                 Field.IsEmptyInRange(downPart.coord.y + 6, downPart.coord.y + 6,
                 downPart.coord.x, downPart.coord.x + 3))
                    onBottomValue = false;
            }
            else if (orientation && isReverse)
            {
                if (Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                 downPart.coord.x, downPart.coord.x + 11))
                    onBottomValue = false;
            }

            mutexMovement.ReleaseMutex();
            return onBottomValue;
        }
        public override void RotateRight()
        {
            mutexMovement.WaitOne();
            if (!orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3, upPart.coord.x - 4,
                    upPart.coord.x - 1) && Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3,
                    upPart.coord.x + 4, upPart.coord.x + 7) && Field.IsEmptyInRange(downPart.coord.y,
                    downPart.coord.y + 1, downPart.coord.x - 8, downPart.coord.x - 5))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.y += 2;
                    upPart.coord.x -= 4;
                    upPart.size.x = 12;
                    upPart.size.y = 2;
                    downPart.coord.x -= 8;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                    isReverse = false;
                }
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y - 2, upPart.coord.y - 1, upPart.coord.x,
                    upPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.y -= 2;
                    upPart.size.x = 4;
                    upPart.size.y = 2;
                    downPart.coord.x += 4;
                    downPart.coord.y = upPart.coord.y;
                    downPart.size.x = 4;
                    downPart.size.y = 6;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = false;
                    isReverse = true;
                }
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 3, downPart.coord.x + 4,
                    downPart.coord.x + 7) && Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3,
                    upPart.coord.x, upPart.coord.x + 3))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x += 8;
                    upPart.size.x = 4;
                    upPart.size.y = 2;
                    downPart.coord.x -= 4;
                    downPart.coord.y += 2;
                    downPart.size.x = 12;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                    isReverse = true;
                }
            }
            else if (orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1, upPart.coord.x - 4,
                    upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 3,
                    downPart.coord.x + 4, downPart.coord.x + 11))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x -= 4;
                    upPart.size.x = 4;
                    upPart.size.y = 6;
                    downPart.coord.x += 8;
                    downPart.coord.y += 2;
                    downPart.size.x = 4;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = false;
                    isReverse = false;
                }
            }
            mutexMovement.ReleaseMutex();
        }
        public override void RotateLeft()
        {
            mutexMovement.WaitOne();
            if (!orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1, upPart.coord.x + 4,
                    upPart.coord.x + 7) && Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3,
                    upPart.coord.x + 4, upPart.coord.x + 7) && Field.IsEmptyInRange(upPart.coord.y + 2,
                    upPart.coord.y + 3, upPart.coord.x - 4, upPart.coord.x - 1))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x += 4;
                    upPart.size.x = 4;
                    upPart.size.y = 2;
                    downPart.coord.x -= 8;
                    downPart.coord.y -= 2;
                    downPart.size.x = 12;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                    isReverse = true;
                }
            }
            else if (orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1, upPart.coord.x - 8,
                    upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 3,
                    downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x -= 8;
                    upPart.size.x = 4;
                    upPart.size.y = 2;
                    downPart.coord.x += 4;
                    downPart.coord.y -= 2;
                    downPart.size.x = 4;
                    downPart.size.y = 6;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = false;
                    isReverse = true;
                }
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, downPart.coord.y + 5, upPart.coord.x,
                    upPart.coord.x + 3) && Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 3,
                    downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.y += 2;
                    upPart.size.x = 12;
                    upPart.size.y = 2;
                    downPart.coord.x -= 4;
                    downPart.coord.y += 4;
                    downPart.size.x = 4;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                    isReverse = false;
                }
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y - 2, downPart.coord.y - 3, upPart.coord.x + 4,
                    upPart.coord.x + 7) && Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                    downPart.coord.x + 4, downPart.coord.x + 11))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x += 4;
                    upPart.coord.y -= 2;
                    upPart.size.x = 4;
                    upPart.size.y = 6;
                    downPart.coord.x += 8;
                    downPart.size.x = 4;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = false;
                    isReverse = false;
                }
            }
            mutexMovement.ReleaseMutex();
        }
    }
}
