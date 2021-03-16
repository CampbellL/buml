namespace TestAssembly
{
    public class Banana : Fruit
    {
        public double Angle;
        private bool _skin = true;

        public void Peel()
        {
            this._skin = false;
        }
    }
}