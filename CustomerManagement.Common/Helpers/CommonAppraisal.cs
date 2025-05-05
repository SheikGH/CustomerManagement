namespace CustomerManagement.Common.Helpers
{
    public static class CommonAppraisal
    {
        private readonly static Dictionary<string, string> dicFormTitleEn = null;
        static CommonAppraisal()
        {
            dicFormTitleEn = new Dictionary<string, string>();
            dicFormTitleEn.Add("Form_Phase1_New", "Performance Objection - Form");
            dicFormTitleEn.Add("Form_Phase1_Reject", "Performance Objection - Reject");
            dicFormTitleEn.Add("Form_Phase2_New", "Achievement - Form");
            dicFormTitleEn.Add("Form_Phase2_Reject", "Achievement - Reject");
            dicFormTitleEn.Add("Form_Phase3_New", "Score - Form");
            dicFormTitleEn.Add("Form_Phase3_Reject", "Score - Reject");
            dicFormTitleEn.Add("Form_Phase3_Approve", "Score - Approve");   
            dicFormTitleEn.Add("Form_Phase4_New", "Final Review - Form");
            dicFormTitleEn.Add("Form_Phase4_Reject", "Final Review - Reject ");
            dicFormTitleEn.Add("Form_Phase4_Approve", "Final Review - Approve");
        }
        public enum Role
        {
            Employee = 1,
            Manager = 2,
            Admin = 3,
        }
        public enum UserAction
        {
            Phase_1_Emp_New = 1,
            Phase_1_Emp_ToMng_Assign = 2,
            Phase_1_Mng_FromEmp_New = 3,
            Phase_1_Mng_ToEmp_Reject = 4,
            Phase_1_Mng_ToEmp_Approve = 5,
            Phase_2_Emp_FromMng_New = 6,
            Phase_2_Emp_ToMng_Assign = 7,
            Phase_2_Mng_FromEmp_New = 8,
            Phase_2_Mng_ToEmp_Reject = 9,
            Phase_2_Mng_ToEmp_Approve = 10,
            Phase_3_Emp_FromMng_New = 11,
            Phase_3_Emp_ToMng_Assign = 12,
            Phase_3_Mng_FromEmp_New = 13,
            Phase_3_Mng_ToEmp_Reject = 14,
            Phase_3_Mng_ToHR_Approve = 15,
            Phase_4_HR_FromMng_New = 16,
            Phase_4_HR_ToHRMng_Assign = 17,
            Phase_4_HRMng_FromHR_New = 18,
            Phase_4_HRMng_ToHR_Reject = 19,
            Phase_4_HRMng_Close = 20,
        }
        public static int GetRoleID(Role rle)
        {
            return (int)rle;
        }
        public static string GetRole(Role rle)
        {
            string strRle = string.Empty;
            strRle = Enum.GetName(typeof(Role), rle);
            return strRle;
        }
        public static string GetUserAction(UserAction v)
        {
            string str = string.Empty;
            str = Enum.GetName(typeof(UserAction), v);
            return str;
        }
        public static string GetFormTitleEn(string k)
        {
            string strVal = string.Empty;
            if (!string.IsNullOrEmpty(k))
            {
                if (dicFormTitleEn.ContainsKey(k))
                    strVal = dicFormTitleEn[k];
                else
                    strVal = dicFormTitleEn.FirstOrDefault().Value;
            }
            else
                strVal = "Error";
            return strVal;
        }
    }
}