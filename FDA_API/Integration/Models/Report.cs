using System.Globalization;

namespace FDA_API.Integration.Models
{
	/// <summary>
	/// Report model.
	/// </summary>
	public class Report
	{
		private const string DateFormat = "yyyyMMdd";

		public string Country { get; set; }
		public string City { get; set; }
		public string Address_1 { get; set; }
		public string Reason_for_recall { get; set; }
		public string Address_2 { get; set; }
		public string Product_quantity { get; set; }
		public string Code_info { get; set; }
		public string Center_classification_date { get; set; }
		public string Distribution_pattern { get; set; }
		public string State { get; set; }
		public string Product_description { get; set; }
		public string Report_date { get; set; }
		public string Classification { get; set; }
		public OpenFda OpenFda { get; set; }
		public string Recalling_firm { get; set; }
		public string Recall_number { get; set; }
		public string Initial_firm_notification { get; set; }
		public string Product_type { get; set; }
		public string Event_id { get; set; }
		public string Termination_date { get; set; }
		public string More_code_info { get; set; }
		public string Recall_initiation_date { get; set; }

		public DateTime Recall_initiation_dateTime 
		{
			get
			{
				if (string.IsNullOrEmpty(Recall_initiation_date))
				{
					return DateTime.MinValue;
				}

				if (DateTime.TryParseExact(Recall_initiation_date, DateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
				{
					return dateValue;
				}

				return DateTime.MinValue;
			}
		}
		public string Postal_code { get; set; }
		public string Voluntary_mandated { get; set; }
		public string Status { get; set; }

		public Report()
		{
			OpenFda = new OpenFda();
		}
	}
}
