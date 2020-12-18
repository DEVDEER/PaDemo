namespace Ui.Frontend.Models
{
	using System;
	using System.Linq;

	public class ErrorViewModel
	{
		#region properties

		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

		#endregion
	}
}
