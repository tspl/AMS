using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;

namespace BunnyBear
{
	/// <summary>
	/// Summary description for WebCustomControl1.
	/// </summary>
	[DefaultProperty("Text"),
		ToolboxData("<{0}:msgBox runat=server></{0}:msgBox>")]
	public class msgBox : System.Web.UI.WebControls.WebControl
	{
		//private string msg;
		private string content;

		[Bindable(true),
			Category("Appearance"),
			DefaultValue("")]
		
		public void alert(string msg)
		{
			string  sMsg = msg.Replace( "\n", "\\n" );
			sMsg =  msg.Replace( "\"", "'" );
 
			StringBuilder sb = new StringBuilder();
			
			sb.Append( @"<script language='javascript'>" );
			
			sb.Append( @"alert( """ + sMsg + @""" );" );

			sb.Append( @"</script>" );	

			content=sb.ToString();
		}

		//good for the page with only one form
		public void confirm(string msg,string hiddenfield_name)
		{
			string  sMsg = msg.Replace( "\n", "\\n" );
			sMsg =  msg.Replace( "\"", "'" );
 
			StringBuilder sb = new StringBuilder();
			
			sb.Append( @"<INPUT type=hidden value='0' name='" + hiddenfield_name + "'>");

			sb.Append( @"<script language='javascript'>" );
			
			sb.Append( @" if(confirm( """ + sMsg + @""" ))" );
			sb.Append( @" { ");
			sb.Append( "document.forms[0]." + hiddenfield_name + ".value='1';" + "document.forms[0].submit(); }" );
			sb.Append( @" else { ");
			sb.Append("document.forms[0]." + hiddenfield_name + ".value='0'; }" );

			sb.Append( @"</script>" );

			content=sb.ToString();
		}

		//good for the page with multiple forms; NOT VERY NECESSARY
		/*public void confirm(string msg,string hiddenfield_name,string formname)
		{
			string  sMsg = msg.Replace( "\n", "\\n" );
			sMsg =  msg.Replace( "\"", "'" );
 
			StringBuilder sb = new StringBuilder();
			
			sb.Append( @"<INPUT type=hidden value='0' name='" + hiddenfield_name + "'>");

			sb.Append( @"<script language='javascript'>" );
			
			sb.Append( @" if(confirm( """ + sMsg + @""" ))" );
			sb.Append( @" { ");
			sb.Append( formname +"." + hiddenfield_name + ".value='1';" + formname + ".submit(); }" );
			sb.Append( @" else { ");
			sb.Append(formname +"." + hiddenfield_name + ".value='0'; }" );

			sb.Append( @"</script>" );

			content=sb.ToString();
		}*/

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			output.Write(this.content);
		}
	}
}
