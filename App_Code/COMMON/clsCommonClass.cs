using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll_PCB.Common_Class
{
    class clsCommonClass
    {
        public static string strCompCode = "ANPC";
        public static string strBranchCode = "ALKHOBAR";
        public static string strDivcode = "MANPOWER";
        public static int iYearSet = 3;
        public static DateTime dtCreatedon =Convert.ToDateTime(DateTime.Now.ToShortDateString());
        public static DateTime dtModifiedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        public static string sCreatedBy = "ADMIN";
        public static string sModifiedBy = "ADMIN";
        public static String strCodes;
        public static String strNames;
        public static string strcontrolname;
        public static string strcontrol1;
        public static string strcontrol2;
        public static string strcontrol3;
        public static string strcontrol4;
        public static string strFilterEmpCode;
        public static string strFilterEmpName;
        public static string strFilterDepCode;
        public static string strFilterDepName;
        public static string strFilterDesigCode;
        public static string strFilterDesigName;
        public static string strFilterHOACode;
        public static string strFilterHOAName;
        public static string strFilterProjectCode;
        public static string strFilterProjectName;
        public static string strFilterOfficeCode;
        public static string strFilterOfficeName;
        public static double dblCapital;
    }

}
