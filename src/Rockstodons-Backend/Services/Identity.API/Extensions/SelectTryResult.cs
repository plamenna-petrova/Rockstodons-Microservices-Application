namespace Identity.API.Extensions
{
    public class SelectTryResult<TSource, TResult>
    {
        public TSource Source { get; private set; }

        public TResult Result { get; private set; }

        public Exception? CaughtException { get; private set; }

        internal SelectTryResult(TSource source, TResult result, Exception? exception)
        {
            Source = source;
            Result = result;
            CaughtException = exception;
        }
    }
}
