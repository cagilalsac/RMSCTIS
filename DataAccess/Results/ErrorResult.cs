using DataAccess.Results.Bases;

namespace DataAccess.Results
{
    public class ErrorResult : Result
	{
		// Example: Result result = new ErrorResult("Operation failed.");
		public ErrorResult(string message) : base(false, message)
		{
		}

		// Example: Result result = new ErrorResult();
		public ErrorResult() : base(false, "")
		{
		}
	}
}
