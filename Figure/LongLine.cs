using GameField;

namespace GameFigure
{
    public class LongLine : Rectangle
    {
        public LongLine(int SizeX, int SizeY) : base(SizeX, SizeY) { }

        public override void RotateRight()
        {
            mutexMovement.WaitOne();
            if (!orientation)
            {
                if (Field.IsEmptyInRange(coord.y - 4, coord.y - 1, coord.x + 8, coord.x + 11)
                    && Field.IsEmptyInRange(coord.y + 2, coord.y + 3, coord.x + 8, coord.x + 11))
                {
                    DeleteFigure();
                    coord.y -= 4;
                    coord.x += 8;
                    size.x = 4;
                    size.y = 8;
                    DrawFigure();
                    orientation = true;
                }
            }
            else
            {
                if (Field.IsEmptyInRange(coord.y + 4, coord.y + 5, coord.x - 8, coord.x - 1)
                    && Field.IsEmptyInRange(coord.y + 4, coord.y + 5, coord.x + 4, coord.x + 7))
                {
                    DeleteFigure();
                    coord.y += 4;
                    coord.x -= 8;
                    size.x = 16;
                    size.y = 2;
                    DrawFigure();
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
