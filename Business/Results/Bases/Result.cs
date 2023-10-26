#nullable disable

namespace Business.Results.Bases
{
	public abstract class Result
	{
		public bool IsSuccessful { get; } // readonly: can only be assigned through constructor
        public string Message { get; set; }

		protected Result(bool isSuccessful, string message)
		{
			IsSuccessful = isSuccessful;
			Message = message;
		}
	}
}
