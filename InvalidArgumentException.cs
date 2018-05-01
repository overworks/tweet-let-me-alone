namespace Mh.Twitter
{
    class InvalidArgumentException : System.ArgumentException
    {
        public InvalidArgumentException()
            : base()
        { }

        public InvalidArgumentException(string message)
            : base(message)
        { }
    }
}
