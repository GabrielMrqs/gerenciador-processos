namespace GerenciadorProcessos.Domain.Shared
{
    public struct Result<TSuccess, TFailure>
    {
        public TFailure Failure { get; internal set; }

        public TSuccess Success { get; internal set; }

        public bool IsFailure { get; }

        public bool IsSuccess => !IsFailure;

        internal Result(TFailure failure)
        {
            IsFailure = true;
            Failure = failure;
            Success = default;
        }

        internal Result(TSuccess success)
        {
            IsFailure = false;
            Failure = default;
            Success = success;
        }

        public static implicit operator Result<TSuccess, TFailure>(TFailure failure)
        {
            return new Result<TSuccess, TFailure>(failure);
        }

        public static implicit operator Result<TSuccess, TFailure>(TSuccess success)
        {
            return new Result<TSuccess, TFailure>(success);
        }
    }
}
