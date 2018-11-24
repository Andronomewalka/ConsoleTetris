using System.Threading;
using GameField;

namespace GameFigure
{
    public class Rectangle : Figure
    {
        public Rectangle(int sizeX, int sizeY) : base(sizeX, sizeY)
        { }

        public override void MoveDown(int milliseconds = 500)
        {
            mutexMovement.WaitOne();

            if (!OnBottom())
                base.MoveDown();

            mutexMovement.ReleaseMutex();
            Thread.Sleep(milliseconds);
        }

        public override void MoveDownBoost()
        {
            mutexMovement.WaitOne();
            while (!OnBottom())
            {
                base.MoveDown();
                Thread.Sleep(5);
            }
            mutexMovement.ReleaseMutex();
        }

        public override void MoveLeft()
        {
            mutexMovement.WaitOne();
            if (Field.IsEmptyInRange(coord.y, coord.y + size.y - 1, coord.x - 4, coord.x - 1))
                base.MoveLeft();
            mutexMovement.ReleaseMutex();
        }

        public override void MoveRight()
        {
            mutexMovement.WaitOne();
            if (Field.IsEmptyInRange(coord.y, coord.y + size.y - 1, coord.x + size.x, coord.x + size.x + 3))
            {
                base.MoveRight();
            }
            mutexMovement.ReleaseMutex();
        }
        public override bool OnBottom()
        {
            mutexMovement.WaitOne();

            bool onBottomValue = false;
            onBottomValue = base.OnBottom();

            mutexMovement.ReleaseMutex();
            return onBottomValue;
        }
    }
}
