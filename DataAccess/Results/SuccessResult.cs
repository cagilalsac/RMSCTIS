using DataAccess.Results.Bases;

namespace DataAccess.Results
{
    public class SuccessResult : Result
	{
		// Example: Result result = new SuccessResult("Operation successful.");
		public SuccessResult(string message) : base(true, message)
		{
		}

		// Example: Result result = new SuccessResult();
        public SuccessResult() : base(true, "")
        {
        }
    }
}
