using System;

namespace GameCenterV2
{
    public class PublicVariables
    {
        // رقم اليوزر
        public static Int32 _CurrentUserId;
        public static string _CurrentUser;
        public static Int32 _CurrentRoleId; // رقم صلاحية اليوزر
        public static string _CurrentRole;
        public static string _EmployeeName; // اسم المستخدم
        public static DateTime _dtCurrentDate = DateTime.Now; // التاريخ
        public static string _ProgName = " - ";

        public static Int32 BoxID;
        public static bool OpenBox = false; 
        public static double PlusItemPrice = 0;




        public static DateTime PlusTime(DateTime dateTime,int plusMinits)
        {
            DateTime resultDateTime = dateTime.AddMinutes(plusMinits);
            return resultDateTime;            
        }
        public static DateTime MinusTime(DateTime dateTime)
        {
            //try
            //{
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            var day = dateTime.Day;
            if (minute >= 0)
            {
                minute = dateTime.Minute - (29);
                if (minute <= 0)
                {
                    int oldm = minute;
                    int newminut = 58 - oldm;
                    if (newminut >= 60)
                    {
                        int oldm1 = newminut;
                        int newminut1 = oldm1 - 60;
                        if (hour > 0)
                            hour = hour - 1;
                        minute = 60 - newminut1;
                    }
                    // hour = hour - 1;
                    //minute = newminut;
                }
            }
            if (hour > 24)
            {
                day += 1;
            }
            return new DateTime(dateTime.Year, dateTime.Month,
                 day, hour, minute, 0);
            //}
            //catch (Exception ex)
            //{
            //    Messages.ErrorMessage(ex.Message);
            //}
        }
        public static double[] DeffrintTime(int hourStart, int hourEnd, int minutStart, int minutEnd)
        {
            double[] hourminuts = new double[2];
            if (hourEnd >= hourStart)
            {
                hourminuts[0] = Convert.ToDouble(hourEnd - hourStart);

                if (minutEnd >= minutStart)
                {
                    hourminuts[1] = (minutEnd - minutStart);
                }
                else
                {
                    int men = minutEnd;
                    men += 60;
                    hourminuts[1] = (men - minutStart);
                    if (hourminuts[0] > 0)
                        hourminuts[0] -= 1;
                }
            }
            else
            {
                int hoend = hourEnd;
                hoend += 23;
                hourminuts[0] = (hoend - hourStart);
                if (minutEnd > minutStart)
                {
                    hourminuts[1] = (minutEnd - minutStart) + 60;
                }
                else
                {
                    int mend = minutEnd;
                    mend += 60;
                    hourminuts[1] = (mend - minutStart);
                    //if (hourminuts[0] > 0)
                      //  hourminuts[0] -= 1;
                }
            }
            return hourminuts;
        }
    }
}
