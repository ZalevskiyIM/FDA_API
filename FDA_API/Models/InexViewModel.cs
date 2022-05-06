using FDA_API.Integration.Models;

namespace FDA_API.Models
{
	/// <summary>
	/// ViewModel for Index view.
	/// </summary>
	public class InexViewModel
	{
		/// <summary>
		/// Date that the FDA issued the enforcement report for the product recall.
		/// </summary>
		public DateTime? ReportDate { get; set; }

		/// <summary>
		/// List of enforcement report.
		/// </summary>
		public List<Report> Reports { get; set; }

		public InexViewModel()
		{
			Reports = new List<Report>();
		}
	}
}
