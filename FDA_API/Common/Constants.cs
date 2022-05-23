namespace FDA_API.Common
{
	public static class Constants
	{
		#region Fda

		/// <summary>
		/// Maximun number of records that API can return per request.
		/// </summary>
		public const int MaxRecordsLimit = 1000;

		/// <summary>
		/// Date format for API.
		/// </summary>
		public const string DateFormat = "yyyyMMdd";

		/// <summary>
		/// Minimum word length to find most frequent word.
		/// </summary>
		public const int MinWordLength = 4;

		#endregion Fda
	}
}
