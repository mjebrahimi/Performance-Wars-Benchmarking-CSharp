namespace Throw_Exception_Benchmark
{
    public interface IDevideOperator
    {
        int Devide(int a, int b);
    }

    public class DevideDecorator : IDevideOperator
    {
        private readonly IDevideOperator _devideOperator;

        public DevideDecorator(IDevideOperator devideOperator)
        {
            _devideOperator = devideOperator;
        }

        public int Devide(int a, int b)
        {
            return _devideOperator.Devide(a, b);
        }
    }

    public class DevideOperator : IDevideOperator
    {
        public int Devide(int a, int b)
        {
            return InternalDevide(a, b);
        }

        private int InternalDevide(int a, int b)
        {
            return a / b;
        }
    }
}
