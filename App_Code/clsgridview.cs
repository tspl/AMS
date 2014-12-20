
using System;

/// <summary>
/// Summary description for clsgridview
/// </summary>

public class clsgridview
{
    public string head = "ARMS";

	public clsgridview()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public enum gridView_HeadingType
    {
        Donor =1,
        TDB=2,
        Season=3,
        Complaint=4,
        Reservation_policy=5,
        Address_change=6,
        Duplicate_pass=7,
        key_lost=8

    }

    public String Heading(gridView_HeadingType Htype)
    {
            switch (Htype)
            {
                case gridView_HeadingType.Donor:
                    return ("" + head + " Donor Resevation Details");
                case gridView_HeadingType.TDB:
                    return (""+head+" TDB Reservation Details");
                case gridView_HeadingType.Season:
                    return ("" + head + " Season Details");
                case gridView_HeadingType.Complaint:
                    return (""+head+" Complaint Details");
                case gridView_HeadingType.Reservation_policy:
                    return ("" + head + " Reservation Policy Details");
                case gridView_HeadingType.Address_change:
                    return ("" + head + " Donor's Address Details");
                case gridView_HeadingType.Duplicate_pass:
                    return ("" + head + " Duplicate Pass Details");
                case gridView_HeadingType.key_lost:
                    return ("" + head + " Key Lost Details");
                default:
                    return "ARMS View";
            }
        
    }

}