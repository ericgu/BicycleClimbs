using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BicycleClimbsLibrary
{
	public class MyTable: Table
	{
		public void Add(params string[] values)
		{
			TableRow row = new TableRow();
			
			foreach (string value in values)
			{
				TableCell cell = new TableCell();
				cell.Text = value;
				row.Cells.Add(cell);
			}
			Rows.Add(row);
		}

		public string this[int i, int j]
		{
			get
			{
				return Rows[i].Cells[j].Text;
			}
		}
	}
}
