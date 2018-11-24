using GameField;

namespace GameFigure
{
    public class ZShapedR : ZShaped // реверс з-образной фиугры
    {
        public ZShapedR(int sizeX, int sizeY)
        {
            upPart = new Figure(sizeX, sizeY / 2, (50 - sizeX) / 2 + 5 + 4, 2);
            downPart = new Figure(sizeX, sizeY / 2, (50 - sizeX) / 2 + 5, 4);
        }

        public override void MoveLeft()
        {
            mutexMovement.WaitOne();
            if (!orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y,
                downPart.coord.y + 1, downPart.coord.x - 4, downPart.coord.x - 1))
                {
                    upPart.MoveLeft();
                    downPart.MoveLeft();
                }
            }
            else if (orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 3,
                upPart.coord.x - 4, upPart.coord.x - 1) && Field.IsEmptyInRange(downPart.coord.y + 2,
                downPart.coord.y + 3, downPart.coord.x - 4, downPart.coord.x - 1))
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
            if (!orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x + upPart.size.x, upPart.coord.x + upPart.size.x + 3) &&
                Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                downPart.coord.x + downPart.size.x, downPart.coord.x + downPart.size.x + 3))
                {
                    downPart.MoveRight();
                    upPart.MoveRight();
                }
            }
            else if (orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y, upPart.coord.y + 1,
                upPart.coord.x + upPart.size.x, upPart.coord.x + upPart.size.x + 3) &&
                Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 3,
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

            if (!orientation)
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 2, upPart.coord.y + 2,
                    upPart.coord.x + 4, upPart.coord.x + 7) &&
                    Field.IsEmptyInRange(downPart.coord.y + 2, downPart.coord.y + 2,
                    downPart.coord.x, downPart.coord.x + 7))
                    onBottomValue = false;
            }
            else
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
                if (Field.IsEmptyInRange(upPart.coord.y - 2, upPart.coord.y + 1, upPart.coord.x - 4,
                    upPart.coord.x - 1))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.y -= 2;
                    upPart.coord.x -= 4;
                    upPart.size.x = 4;
                    upPart.size.y = 4;
                    downPart.coord.y -= 2;
                    downPart.coord.x += 4;
                    downPart.size.x = 4;
                    downPart.size.y = 4;
                    upPart.DrawFigure();
                    downPart.DrawFigure();
                    orientation = true;
                }
            }
            else
            {
                if (Field.IsEmptyInRange(upPart.coord.y + 4, upPart.coord.y + 5, upPart.coord.x,
                    upPart.coord.x + 3) && Field.IsEmptyInRange(downPart.coord.y, downPart.coord.y + 1,
                    downPart.coord.x + 4, downPart.coord.x + 7))
                {
                    upPart.DeleteFigure();
                    downPart.DeleteFigure();
                    upPart.coord.y += 2;
                    upPart.coord.x += 4;
                    upPart.size.x = 8;
                    upPart.size.y = 2;
                    downPart.coord.y += 2;
                    downPart.coord.x -= 4;
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
