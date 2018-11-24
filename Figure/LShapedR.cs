using GameField;

namespace GameFigure
{
    public class LShapedR : LShaped
    {
        public LShapedR(int sizeX, int sizeY)
        {
            upPart = new Figure(sizeX / 2, sizeY, (50 - sizeX) / 2 + 5 + 4, 2);
            downPart = new Figure(sizeX / 2, sizeY / 3, (50 - sizeX) / 2 + 5, 6);
        }

        public override void MoveLeft()
        {
            mutexMovement.WaitOne();

            // при повороте составных фигур меняется их чек на пустые поля (одна часть фигуры мешает другой части фигуры сделать шаг)
            if (!orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 3,
                upPart.coord.x - 4, upPart.coord.x - 1) &&
                Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    downPart.MoveLeft();
                    upPart.MoveLeft();
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
                if (Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 5,
                    downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    downPart.MoveLeft();
                    upPart.MoveLeft();
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
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 5,
                upPart.coord.x + 4, upPart.coord.x + 7))
                {
                    upPart.MoveRight();
                    downPart.MoveRight();
                }
            }
            else if (orientation && !isReverse)
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
            else if (!orientation && isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                    upPart.coord.x + 4, upPart.coord.x + 7) &&
                    Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 5,
                    downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    upPart.MoveRight();
                    downPart.MoveRight();
                }
            }
            else if (orientation && isReverse)
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
            mutexMovement.ReleaseMutex();
        }

        public override bool OnBottom()
        {
            mutexMovement.WaitOne();
            bool onBottomValue = true;

            if (!orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                    downPart.coord.x, downPart.coord.x + 7))
                    onBottomValue = false;
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                 downPart.coord.x, downPart.coord.x + downPart.size.x - 1))
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
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 2,
                 upPart.coord.x, upPart.coord.x + 7) &&
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
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 3, upPart.coord.x - 4,
                    upPart.coord.x - 1) && Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3,
                    upPart.coord.x + 4, upPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x -= 4;
                    upPart.size.x = 4;
                    upPart.size.y = 2;
                    downPart.coord.y -= 2;
                    downPart.size.x = 12;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                    isReverse = false;
                }
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1, upPart.coord.x + 4,
                    upPart.coord.x + 11) && Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 7,
                    downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x += 8;
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
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 5, upPart.coord.x,
                    upPart.coord.x + 3) && Field.IsEmptyInRange(downPart.coord.y + 2,
                    downPart.coord.y + 3, downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x -= 8;
                    upPart.coord.y += 2;
                    upPart.size.x = 12;
                    upPart.size.y = 2;
                    downPart.coord.x += 4;
                    downPart.coord.y += 4;
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
                if (Field.IsEmptyInRange(upPart.coord.y - 2, upPart.coord.y - 1, upPart.coord.x + 4,
                    upPart.coord.x + 7) && Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                    downPart.coord.x - 8, downPart.coord.x - 1))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x += 4;
                    upPart.coord.y -= 2;
                    upPart.size.x = 4;
                    upPart.size.y = 6;
                    downPart.coord.x -= 8;
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
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 5, upPart.coord.x + 4,
                    upPart.coord.x + 7) && Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3,
                    upPart.coord.x - 4, upPart.coord.x - 1))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x -= 4;
                    upPart.coord.y += 2;
                    upPart.size.x = 12;
                    upPart.size.y = 2;
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
                if (Field.IsEmptyInRange(upPart.coord.y - 2, upPart.coord.y - 1, upPart.coord.x + 4,
                    upPart.coord.x + 11) && Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                    downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x += 8;
                    upPart.coord.y -= 2;
                    upPart.size.x = 4;
                    upPart.size.y = 2;
                    downPart.coord.x -= 4;
                    downPart.coord.y -= 4;
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
                if (Field.IsEmptyInRange(upPart.coord.y + 2, downPart.coord.y + 3, upPart.coord.x,
                    upPart.coord.x + 3) && Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 3,
                    downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x -= 8;
                    upPart.size.x = 4;
                    upPart.size.y = 2;
                    downPart.coord.x -= 4;
                    downPart.coord.y += 2;
                    downPart.size.x = 12;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                    isReverse = false;
                }
            }
            else if (orientation && !isReverse)
            {
                if (Field.IsEmptyInRange(upPart.coord.y - 2, upPart.coord.y - 1, upPart.coord.x + 4,
                    upPart.coord.x + 11) && Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 3,
                    downPart.coord.x, downPart.coord.x + 3))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.x += 4;
                    upPart.size.x = 4;
                    upPart.size.y = 6;
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
