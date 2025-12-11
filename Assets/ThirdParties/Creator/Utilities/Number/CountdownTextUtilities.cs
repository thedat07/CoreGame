using System;
using TMPro;
using UnityEngine.UI;

namespace UnityUtilities
{
    public static class CountdownTextUtilities
    {
        public static string FormatCountdownLives(TimeSpan time,
  string formatD, string formatH, string formatM, string formatZero)
        {
            string displayText;
            if (time.TotalSeconds <= 0)
            {
                return formatZero;
            }
            if (time.TotalDays >= 1)
                displayText = string.Format(formatD, time.Days, time.Hours);
            else if (time.TotalHours >= 1)
                displayText = string.Format(formatH, time.Hours, time.Minutes);
            else
                displayText = string.Format(formatM, time.Minutes, time.Seconds);

            return displayText;
        }

        public static bool UpdateCountdownText(this Text target, DateTime timeEnd,
            string formatD = "{0}d {1}h", string formatH = "{0}h {1}m",
            string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            var timeRemaining = timeEnd - DateTime.UtcNow;
            return target.UpdateCountdownText(timeRemaining, formatD, formatH, formatM, formatZero);
        }

        public static bool UpdateCountdownText(this TextMeshProUGUI target, DateTime timeEnd,
            string formatD = "{0}d {1}h", string formatH = "{0}h {1}m",
            string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            var timeRemaining = timeEnd - DateTime.UtcNow;
            return target.UpdateCountdownText(timeRemaining, formatD, formatH, formatM, formatZero);
        }

        public static bool UpdateCountdownText(this Text target, TimeSpan time,
            string formatD = "{0}d {1}h", string formatH = "{0}h {1}m",
            string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            target.text = FormatCountdown(time, formatD, formatH, formatM, formatZero, out bool isRunning);
            return isRunning;
        }

        public static bool UpdateCountdownText(this TextMeshProUGUI target, TimeSpan time,
            string formatD = "{0}d {1}h", string formatH = "{0}h {1}m",
            string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            target.text = FormatCountdown(time, formatD, formatH, formatM, formatZero, out bool isRunning);
            return isRunning;
        }

        /// <summary>
        /// Trả về chuỗi định dạng đếm ngược thay vì gán trực tiếp vào text.
        /// </summary>
        public static string FormatCountdown(
            TimeSpan time,
            string formatD, string formatH, string formatM, string formatZero,
            out bool isRunning)
        {
            string displayText;
            if (time.TotalSeconds <= 0)
            {
                isRunning = false;
                return formatZero;
            }

            isRunning = true;
            if (time.TotalDays >= 1)
            {
                displayText = string.Format(formatD, time.Days, time.Hours);
            }
            else if (time.TotalHours >= 1)
            {
                if (time.Minutes > 0)
                    displayText = string.Format(formatH, time.Hours, time.Minutes);
                else
                    displayText = $"{time.Hours}h";
            }
            else
            {
                if (time.Seconds > 0)
                    displayText = string.Format(formatM, time.Minutes, time.Seconds);
                else
                    displayText = $"{time.Minutes}m";
            }

            return displayText;
        }

        /// <summary>
        /// Hiển thị định dạng đơn giản: hh:mm:ss (vd: 02:15:09)
        /// </summary>
        public static void UpdateCountdownTextSimple(this Text target, TimeSpan time)
        {
            target.text = FormatSimple(time);
        }

        public static void UpdateCountdownTextSimple(this TextMeshProUGUI target, TimeSpan time)
        {
            target.text = FormatSimple(time);
        }

        public static string FormatSimple(TimeSpan time)
        {
            if (time.TotalSeconds <= 0)
                return "00:00:00";

            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                (int)time.TotalHours, time.Minutes, time.Seconds);
        }

        /// <summary>
        /// Cập nhật Text với input là số giây còn lại.
        /// </summary>
        public static bool UpdateCountdownTextFromSeconds(this Text target, float secondsRemaining,
            string formatD = "{0}d {1}h", string formatH = "{0}h {1}m",
            string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            return target.UpdateCountdownText(TimeSpan.FromSeconds(secondsRemaining), formatD, formatH, formatM, formatZero);
        }

        public static bool UpdateCountdownTextFromSeconds(this TextMeshProUGUI target, float secondsRemaining,
            string formatD = "{0}d {1}h", string formatH = "{0}h {1}m",
            string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            return target.UpdateCountdownText(TimeSpan.FromSeconds(secondsRemaining), formatD, formatH, formatM, formatZero);
        }

        /// <summary>
        /// Cập nhật Text với prefix/suffix tùy chỉnh.
        /// </summary>
        public static bool UpdateCountdownTextWithPrefixSuffix(this Text target, TimeSpan time,
            string prefix = "", string suffix = "",
            string formatD = "{0}d {1}h", string formatH = "{0}h {1}m",
            string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            var formatted = FormatCountdown(time, formatD, formatH, formatM, formatZero, out bool isRunning);
            target.text = $"{prefix}{formatted}{suffix}";
            return isRunning;
        }

        public static bool UpdateCountdownTextWithPrefixSuffix(this TextMeshProUGUI target, TimeSpan time,
            string prefix = "", string suffix = "",
            string formatD = "{0}d {1}h", string formatH = "{0}h {1}m",
            string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            var formatted = FormatCountdown(time, formatD, formatH, formatM, formatZero, out bool isRunning);
            target.text = $"{prefix}{formatted}{suffix}";
            return isRunning;
        }

        /// <summary>
        /// Định dạng rút gọn: chỉ hiển thị đơn vị lớn nhất (vd: 3d, 5h, 22m).
        /// </summary>
        public static string FormatCompact(TimeSpan time)
        {
            if (time.TotalSeconds <= 0)
                return "--";

            if (time.TotalDays >= 1)
                return $"{time.Days}d";
            if (time.TotalHours >= 1)
                return $"{time.Hours}h";
            if (time.TotalMinutes >= 1)
                return $"{time.Minutes}m";
            return $"{time.Seconds}s";
        }

        public static void UpdateCountdownTextCompact(this Text target, TimeSpan time)
        {
            target.text = FormatCompact(time);
        }

        public static void UpdateCountdownTextCompact(this TextMeshProUGUI target, TimeSpan time)
        {
            target.text = FormatCompact(time);
        }

        /// <summary>
        /// Reset text về mặc định khi không còn thời gian.
        /// </summary>
        public static void ClearCountdownText(this Text target, string placeholder = "--:--")
        {
            target.text = placeholder;
        }

        public static void ClearCountdownText(this TextMeshProUGUI target, string placeholder = "--:--")
        {
            target.text = placeholder;
        }
    }
}