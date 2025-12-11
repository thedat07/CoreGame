using System;

namespace UnityUtilities
{
    public static class DateTimeExtensions
    {

        /// <summary>
        /// Trả về thứ trong tuần dưới dạng số 1-7 (Thứ Hai=1, Chủ Nhật=7)
        /// </summary>
        public static int AdjustedDayOfWeek(DateTime date)
        {
            int dayOfWeekAsInt = (int)date.DayOfWeek;
            int adjustedDayOfWeek = dayOfWeekAsInt == 0 ? 7 : dayOfWeekAsInt;
            return adjustedDayOfWeek;
        }

        /// <summary>
        /// Tìm ngày đầu tiên của tuần chứa ngày dateTime, với tuần bắt đầu từ thứ Hai.
        /// </summary>
        public static DateTime FindFirstDateOfTheWeek(DateTime dateTime)
        {
            int daysOffset = (int)dateTime.DayOfWeek - 1;
            if (daysOffset < 0) daysOffset += 7;
            return dateTime.AddDays(-daysOffset).Date;
        }

        /// <summary>
        /// Tìm ngày cuối cùng của tuần chứa ngày dateTime, với tuần bắt đầu từ thứ Hai.
        /// </summary>
        public static DateTime FindLastDateOfTheWeek(DateTime dateTime)
        {
            int daysOffset = 7 - AdjustedDayOfWeek(dateTime);
            return dateTime.AddDays(daysOffset).Date;
        }

        /// <summary>
        /// Tìm ngày đầu tiên của tháng chứa dateTime (ngày 1 tháng đó).
        /// </summary>
        public static DateTime FindFirstDateOfTheMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Tìm ngày cuối cùng của tháng chứa dateTime.
        /// </summary>
        public static DateTime FindLastDateOfTheMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Tìm ngày đầu tiên của năm chứa dateTime (01/01).
        /// </summary>
        public static DateTime FindFirstDateOfTheYear(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        /// <summary>
        /// Tìm ngày cuối cùng của năm chứa dateTime (31/12).
        /// </summary>
        public static DateTime FindLastDateOfTheYear(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 12, 31);
        }

        // ----- HÀM MỚI BỔ SUNG -----

        /// <summary>
        /// Kiểm tra xem date có phải là ngày cuối tuần (Thứ 7 hoặc Chủ Nhật) hay không.
        /// </summary>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Kiểm tra xem date có phải là ngày đầu tuần (Thứ Hai) hay không.
        /// </summary>
        public static bool IsStartOfWeek(this DateTime date)
        {
            return AdjustedDayOfWeek(date) == 1;
        }

        /// <summary>
        /// Kiểm tra xem date có phải là ngày cuối tuần (Chủ Nhật) hay không.
        /// </summary>
        public static bool IsEndOfWeek(this DateTime date)
        {
            return AdjustedDayOfWeek(date) == 7;
        }

        /// <summary>
        /// Lấy số ngày còn lại trong tháng của date.
        /// </summary>
        public static int DaysRemainingInMonth(this DateTime date)
        {
            DateTime lastDay = FindLastDateOfTheMonth(date);
            return (lastDay - date.Date).Days;
        }

        /// <summary>
        /// Lấy số ngày đã qua trong năm tính từ ngày 1/1 đến date.
        /// </summary>
        public static int DayOfYearPassed(this DateTime date)
        {
            return date.DayOfYear;
        }

        /// <summary>
        /// Tính số ngày giữa 2 ngày (bỏ qua phần thời gian)
        /// </summary>
        public static int DaysBetween(this DateTime start, DateTime end)
        {
            return Math.Abs((end.Date - start.Date).Days);
        }

        /// <summary>
        /// Tính khoảng thời gian giữa 2 DateTime (có thể có phần giờ, phút, giây)
        /// </summary>
        public static TimeSpan TimeSpanBetween(this DateTime start, DateTime end)
        {
            return (end - start).Duration();
        }

        /// <summary>
        /// Kiểm tra xem date có nằm trong khoảng [start, end] (bao gồm 2 đầu)
        /// </summary>
        public static bool IsBetween(this DateTime date, DateTime start, DateTime end)
        {
            return date >= start && date <= end;
        }

        /// <summary>
        /// Chuyển đổi DateTime sang một múi giờ khác (theo Id múi giờ IANA/Windows)
        /// Ví dụ: "Pacific Standard Time", "SE Asia Standard Time"
        /// </summary>
        public static DateTime ConvertToTimeZone(this DateTime date, string timeZoneId)
        {
            try
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTime(date, tz);
            }
            catch (TimeZoneNotFoundException)
            {
                throw new ArgumentException($"Không tìm thấy TimeZoneId: {timeZoneId}");
            }
            catch (InvalidTimeZoneException)
            {
                throw new ArgumentException($"TimeZoneId không hợp lệ: {timeZoneId}");
            }
        }

        /// <summary>
        /// Lấy ngày bắt đầu quý chứa dateTime (ví dụ: quý 1 bắt đầu từ 1/1)
        /// </summary>
        public static DateTime FindFirstDateOfQuarter(DateTime dateTime)
        {
            int currentQuarter = (dateTime.Month - 1) / 3 + 1;
            int firstMonthOfQuarter = (currentQuarter - 1) * 3 + 1;
            return new DateTime(dateTime.Year, firstMonthOfQuarter, 1);
        }

        /// <summary>
        /// Lấy ngày cuối cùng của quý chứa dateTime
        /// </summary>
        public static DateTime FindLastDateOfQuarter(DateTime dateTime)
        {
            int currentQuarter = (dateTime.Month - 1) / 3 + 1;
            int firstMonthOfNextQuarter = currentQuarter * 3 + 1;
            if (firstMonthOfNextQuarter > 12)
                return new DateTime(dateTime.Year, 12, 31);
            else
                return new DateTime(dateTime.Year, firstMonthOfNextQuarter, 1).AddDays(-1);
        }

        /// <summary>
        /// Chuyển đổi DateTime thành chuỗi theo chuẩn ISO 8601 (UTC)
        /// </summary>
        public static string ToIso8601String(this DateTime date)
        {
            return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// Chuyển đổi DateTime thành chuỗi theo chuẩn ISO 8601 có mili giây (UTC)
        /// </summary>
        public static string ToIso8601WithMillisecondsString(this DateTime date)
        {
            return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public static string ToTextTime(int second)
        {
            if (second > 0)
            {
                int minutes = second / 60;
                int seconds = second % 60;
                return string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                return string.Format("00:00");
            }
        }

        private static readonly DateTime baseDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Tính ngày bắt đầu của chu kỳ sự kiện hiện tại
        /// </summary>
        public static DateTime GetEventStartTime(this DateTime currentTime, int durationDays, int intervalDays)
        {
            int totalCycleDays = durationDays + intervalDays;
            if (totalCycleDays <= 0)
                throw new ArgumentException("durationDays + intervalDays must be > 0");

            int daysPassed = (int)(currentTime - baseDate).TotalDays;

            if (daysPassed < 0)
                return baseDate; // chưa tới ngày bắt đầu thì lấy baseDate

            int cycleIndex = daysPassed / totalCycleDays;
            return baseDate.AddDays(cycleIndex * totalCycleDays);
        }

        /// <summary>
        /// Tính ngày kết thúc của chu kỳ sự kiện hiện tại
        /// </summary>
        public static DateTime GetEventEndTime(this DateTime currentTime, int durationDays, int intervalDays)
        {
            DateTime start = currentTime.GetEventStartTime(durationDays, intervalDays);
            return start.AddDays(durationDays);
        }

        /// <summary>
        /// Kiểm tra hiện tại có đang nằm trong sự kiện không
        /// </summary>
        public static bool IsInEvent(this DateTime currentTime, int durationDays, int intervalDays)
        {
            DateTime start = currentTime.GetEventStartTime(durationDays, intervalDays);
            DateTime end = start.AddDays(durationDays);
            return currentTime >= start && currentTime < end;
        }

        /// <summary>
        /// Lấy ngày bắt đầu của sự kiện kế tiếp
        /// </summary>
        public static DateTime GetNextEventStartTime(this DateTime currentTime, int durationDays, int intervalDays)
        {
            DateTime start = currentTime.GetEventStartTime(durationDays, intervalDays);
            return start.AddDays(durationDays + intervalDays);
        }
    }
}