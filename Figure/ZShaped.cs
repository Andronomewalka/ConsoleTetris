using System.Threading;
using GameField;

namespace GameFigure
{
    public class ZShaped : Figure
    {
        protected Figure upPart; // цельня фигура состоит из двух маленьких прямоугольников
        protected Figure downPart;

        internal ZShaped() { }
        public ZShaped(int sizeX, int sizeY)
        {
            upPart = new Figure(sizeX, sizeY / 2, (50 - sizeX) / 2 + 5, 2);
            downPart = new Figure(sizeX, sizeY / 2, (50 - sizeX) / 2 + 5 + 4, 4);
        }
        public override int YCoord
        {   // возвращаем координаты начала фигуры (верхняя часть составной фигуры) для метода Field.DeleteLine
            get { return upPart.coord.y; }
        }
        public override int YSize
        {
            get
            {
                if (orientation)
                    return downPart.size.y + upPart.size.y / 2; // а также размер составной фигуры
                else
                    return downPart.size.y + upPart.size.y;
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
            if (!orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y,
                downPart.coord.y + 1, downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    downPart.MoveLeft();
                    upPart.MoveLeft();
                }
            }

            else if (orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                  upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y,
                  downPart.coord.y + 3, downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    downPart.MoveLeft();
                    upPart.MoveLeft();
                }
            }
            mutexMovement.ReleaseMutex();
        }

        public override void MoveRight()
        {
            mutexMovement.WaitOne();
            if (!orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x + 8, upPart.coord.x + 11) &&
                Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                downPart.coord.x + 8, downPart.coord.x + 11))
                {
                    upPart.MoveRight();
                    downPart.MoveRight();
                }
            }
            else if (orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 3,
                upPart.coord.x + 4, upPart.coord.x + 7) &&
                Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 3,
                downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    upPart.MoveRight();
                    downPart.MoveRight();
                }
            }
            mutexMovement.ReleaseMutex();
        }

        public override bool OnBottom()
        {
            mutexMovement.WaitOne();
            bool onBottomValue = true;
            
            if (!orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 2,
                    upPart.coord.x, upPart.coord.x + 3) &&
                    Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                    downPart.coord.x, downPart.coord.x + downPart.size.x - 1))
                    onBottomValue = false;
            }
            else if (orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 4, upPart.coord.y + 4,
                    upPart.coord.x, upPart.coord.x + 3) &&
                    Field.IsEmptyInRange(downPart.coord.y + 4, downPart.coord.y + 4,
                    downPart.coord.x, downPart.coord.x + 3))
                    onBottomValue = false;
            }

            mutexMovement.ReleaseMutex();
            return onBottomValue;
        }

        public override void RotateRight()
        {
            mutexMovement.WaitOne();
            if (!orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y - 2, upPart.coord.y + 1, 
                    upPart.coord.x + upPart.size.x, upPart.coord.x + upPart.size.x + 3))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.y -= 2;
                    upPart.coord.x += 8;
                    upPart.size.x = 4;
                    upPart.size.y = 4;
                    downPart.coord.y -= 2;
                    downPart.size.x = 4;
                    downPart.size.y = 4;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                }
            }
            else
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 3, upPart.coord.x - 8,
                    upPart.coord.x - 5) && Field.IsEmptyInRange(upPart.coord.y + 4, upPart.coord.y + 5,
                    upPart.coord.x, upPart.coord.x + 3))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.y += 2;
                    upPart.coord.x -= 8;
                    upPart.size.x = 8;
                    upPart.size.y = 2;
                    downPart.coord.y += 2;
                    downPart.size.x = 8;
                    downPart.size.y = 2;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = false;
                }
            }
            mutexMovement.ReleaseMutex();
        }
        public override void RotateLeft()
        {
            RotateRight();
        }
    }

}
