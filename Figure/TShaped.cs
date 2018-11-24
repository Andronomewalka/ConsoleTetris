using System.Threading;
using GameField;

namespace GameFigure
{
    public class TShaped : Figure
    {
        private Figure upPart; // цельня фигура состоит из прямоугольника и квадрата
        private Figure downPart;
        private bool isReverse;
        public TShaped(int sizeX, int sizeY)
        {
            isReverse = false;
            upPart = new Figure(sizeX, sizeY / 2, (50 - sizeX) / 2 + 5 + 2, 2); // +2 небольшое смещение, необходимое, чтоб фигура ходила по тем же полям, что и другие
            downPart = new Figure(sizeX / 3, sizeY / 2, (50 - sizeX) / 2 + 5 + 4 + 2, 4);
        }
        public override int YCoord
        {   // возвращаем координаты начала фигуры (верхняя часть составной фигуры) для метода Field.DeleteLine
            get { return upPart.coord.y; }
        }
        public override int YSize
        {
            get
            {
                if ((orientation && isReverse) || (orientation && !isReverse))
                    return upPart.size.y; // а также размер составной фигуры
                else if ((!orientation && isReverse) || (!orientation && !isReverse))
                    return upPart.size.y + downPart.size.y;

                return 0; // все ветви кода должны возвращать значения (на деле эта строка недостижима)
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
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y,
                downPart.coord.y + 1, downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    upPart.MoveLeft();
                    downPart.MoveLeft();
                }
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y,
                downPart.coord.y + 1, downPart.coord.x - 4, downPart.coord.x - 1) &&
                Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 3,
                downPart.coord.x, downPart.coord.x + 3))
                {
                    downPart.MoveLeft();
                    upPart.MoveLeft();
                }
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                    upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y,
                    downPart.coord.y + 1, downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    upPart.MoveLeft();
                    downPart.MoveLeft();
                }
            }
            else if (orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 5,
                    upPart.coord.x - 4, upPart.coord.x - 1))
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
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x + 12, upPart.coord.x + 15) &&
                Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    downPart.MoveRight();
                    upPart.MoveRight();
                }
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 5,
                    upPart.coord.x + 4, upPart.coord.x + 7))
                {
                    upPart.MoveRight();
                    downPart.MoveRight();
                }
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x + 4, upPart.coord.x + 7) &&
                Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                downPart.coord.x + 12, downPart.coord.x + 15))
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
                    downPart.coord.x + 4, downPart.coord.x + 7) &&
                    Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 3,
                    downPart.coord.x, downPart.coord.x + 3))
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
                if (Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y,
                    downPart.coord.x - 4, downPart.coord.x - 1) &&
                    Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y,
                    downPart.coord.x + 4, downPart.coord.x + 7) &&
                    Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                    downPart.coord.x, downPart.coord.x + 3))
                    onBottomValue = false;
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 6, upPart.coord.y + 6,
                    upPart.coord.x, upPart.coord.x + 3) &&
                    Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                    downPart.coord.x, downPart.coord.x + 3))
                    onBottomValue = false;
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                    downPart.coord.x, downPart.coord.x + 11))
                    onBottomValue = false;
            }
            else if (orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 6, upPart.coord.y + 6,
                    upPart.coord.x, upPart.coord.x + 3) &&
                    Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                    downPart.coord.x, downPart.coord.x + 3))
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
                if (Field.IsEmptyInRange(upPart.coord.y - 2, upPart.coord.y - 1, upPart.coord.x + 4,
                    upPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.y -= 2;
                    upPart.coord.x += 4;
                    upPart.size.x = 4;
                    upPart.size.y = 6;
                    downPart.coord.x -= 4;
                    downPart.coord.y -= 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                    isReverse = false;
                }
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3, upPart.coord.x + 4,
                    upPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.size.x = 4;
                    upPart.size.y = 2;
                    downPart.size.x = 12;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = false;
                    isReverse = true;
                }
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 3, downPart.coord.x + 4,
                    downPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.size.x = 4;
                    upPart.size.y = 6;
                    downPart.coord.x += 8;
                    downPart.size.x = 4;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                    isReverse = true;
                }
            }
            else if (orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3, upPart.coord.x - 4,
                    upPart.coord.x - 1))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x -= 4;
                    upPart.coord.y += 2;
                    upPart.size.x = 12;
                    upPart.size.y = 2;
                    downPart.coord.x -= 4;
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
                if (Field.IsEmptyInRange(upPart.coord.y - 2, upPart.coord.y - 1, upPart.coord.x + 4,
                    upPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x += 4;
                    upPart.coord.y -= 2;
                    upPart.size.x = 4;
                    upPart.size.y = 6;
                    downPart.coord.x += 4;
                    downPart.coord.y -= 2;
                    downPart.size.x = 4;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                    isReverse = true;
                }
            }
            else if (orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3, upPart.coord.x - 4,
                    upPart.coord.x - 1))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.size.x = 4;
                    upPart.size.y = 2;
                    downPart.coord.x -= 8;
                    downPart.size.x = 12;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = false;
                    isReverse = true;
                }
            }
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 3, downPart.coord.x + 4,
                    downPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.size.x = 4;
                    upPart.size.y = 6;
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
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3, upPart.coord.x + 4,
                    upPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x -= 4;
                    upPart.coord.y += 2;
                    upPart.size.x = 12;
                    upPart.size.y = 2;
                    downPart.coord.x += 4;
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
    }
}
