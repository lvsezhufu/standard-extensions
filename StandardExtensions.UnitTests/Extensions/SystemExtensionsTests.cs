﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StandardExtensions.UnitTests
{
    [TestClass]
    public class SystemExtensionsTests
    {
        [TestMethod]
        public void RoundBack__when__Thu11May_with_grain_7__then__returns_x()
        {
            var dto = new DateTimeOffset(2017, 5, 11, 13, 0, 0, TimeSpan.FromHours(0));

            var r = dto.RoundBack(TimeSpan.FromDays(7));

            Assert.AreEqual(r.DayOfWeek, DayOfWeek.Monday);
            Assert.AreEqual(r.Day, 8);
            Assert.AreEqual(r.Month, 5);
            Assert.AreEqual(r.Year, 2017);
            Assert.AreEqual(r.TimeOfDay.TotalHours, 0);
        }

        [TestMethod]
        public void RoundForward__when__1003am_with_grain_5__then__returns_1005am()
        {
            var raggedTime = DateTimeOffset.Parse("10:03");

            var cleanTime = raggedTime.RoundForward(TimeSpan.FromMinutes(5));

            Assert.AreEqual(10, cleanTime.Hour);
            Assert.AreEqual(5, cleanTime.Minute);
        }

        [TestMethod]
        public void RoundForward__when__1003am_1004am_1008am_with_grain_15__then__all_end_up_1015am()
        {
            List<DateTimeOffset> raggedTimes = new List<DateTimeOffset>();

            raggedTimes.Add(DateTimeOffset.Parse("10:03"));
            raggedTimes.Add(DateTimeOffset.Parse("10:04"));
            raggedTimes.Add(DateTimeOffset.Parse("10:08"));

            foreach (var raggy in raggedTimes)
            {
                var cleanTime = raggy.RoundForward(TimeSpan.FromMinutes(15));

                Assert.AreEqual(10, cleanTime.Hour);
                Assert.AreEqual(15, cleanTime.Minute);
            }
        }

        [TestMethod]
        public void RoundForward__when__Thur24thMarch_with_grain_1_week__then__returns_Mon28thMarch()
        {
            // Should round to the start of the next week.

            var raggedTime = DateTimeOffset.Parse("24/03/2016", System.Globalization.CultureInfo.GetCultureInfo("en-gb"), System.Globalization.DateTimeStyles.None); // 24th is mid-week.

            Assert.AreEqual(3, raggedTime.Month);

            var cleanTime = raggedTime.RoundForward(TimeSpan.FromDays(7));

            Assert.AreEqual(28, cleanTime.Day);
            Assert.AreEqual(3, cleanTime.Month);
            Assert.AreEqual(0, cleanTime.Hour);
            Assert.AreEqual(0, cleanTime.Minute);
        }

        [TestMethod]
        public void DateTimeOffset_To8601String__when__offset_value_supplied__then__roundtrips_perfectly()
        {
            const string bst = "2017-05-11T06:42:43+01:00";

            var dto = DateTimeOffset.Parse(bst);

            string tripped = dto.ToISO8601String();

            Assert.AreEqual(bst, tripped);
        }

        [TestMethod]
        public void DateTimeOffset_To8601String__when__utc_value_supplied__then__roundtrips_perfectly()
        {
            const string uni = "2017-05-11T05:42:43Z";

            var dto = DateTimeOffset.Parse(uni);

            string tripped = dto.ToISO8601String();

            Assert.AreEqual(uni, tripped);
        }

        [TestMethod]
        public void DateTimeOffset_ToStartOfNextMonth__when__15_Oct_2017__then__returns_1st_Nov_midnight()
        {
            DateTimeOffset october15 = new DateTimeOffset(2017, 10, 15, 1, 0, 0, TimeSpan.FromHours(1));

            DateTimeOffset actual = october15.ToStartOfNextMonth();

            DateTimeOffset november1st = new DateTimeOffset(2017, 11, 1, 0, 0, 0, TimeSpan.FromHours(1));

            Assert.AreEqual(november1st, actual);
        }

        [TestMethod]
        public void DateTimeOffset_ToStartOfNextMonth__when__15_Dec_2017__then__returns_1st_Jan_2018()
        {
            DateTimeOffset october15 = new DateTimeOffset(2017, 12, 15, 1, 0, 0, TimeSpan.FromHours(1));

            DateTimeOffset actual = october15.ToStartOfNextMonth();

            DateTimeOffset jan1str2018 = new DateTimeOffset(2018, 1, 1, 0, 0, 0, TimeSpan.FromHours(1));

            Assert.AreEqual(jan1str2018, actual);
        }

        [TestMethod]
        public void TimeSpan_ToCaption__outputs_hours_and_minutes()
        {
            var duration = TimeSpan.FromHours(3);
            duration = duration.Add(TimeSpan.FromMinutes(13));

            string caption = duration.ToCaption();

            Assert.AreEqual("3 hours 13 minutes", caption);
        }

        [TestMethod]
        public void TimeSpan_ToCaption__outputs_days_and_hours()
        {
            var duration = TimeSpan.FromDays(3);
            duration = duration.Add(TimeSpan.FromHours(13));

            string caption = duration.ToCaption();

            Assert.AreEqual("3 days 13 hours", caption);
        }

        [TestMethod]
        public void TimeSpan_ToCaption__outputs_minutes_and_seconds()
        {
            var duration = TimeSpan.FromMinutes(3);
            duration = duration.Add(TimeSpan.FromSeconds(13));

            string caption = duration.ToCaption();

            Assert.AreEqual("3 minutes 13 seconds", caption);
        }

        [TestMethod]
        public void TimeSpan_ToCaption__outputs_just_seconds()
        {
            var duration = TimeSpan.FromSeconds(3);

            string caption = duration.ToCaption();

            Assert.AreEqual("3 seconds", caption);
        }

        [TestMethod]
        public void TimeSpan_ToCaption__outputs_just_ms()
        {
            var duration = TimeSpan.FromMilliseconds(3);

            string caption = duration.ToCaption();

            Assert.AreEqual("3ms", caption);
        }

        [TestMethod]
        public void String_RightAfter__when__parsnip_delimited_by_s__then__returns_nip()
        {
            Assert.AreEqual("nip", "parsnip".RightAfter("s"));
        }

        [TestMethod]
        public void String_RightAfter__when__p_delimited_by_p__then__returns_empty()
        {
            Assert.AreEqual("", "p".RightAfter("p"));
        }

        [TestMethod]
        public void String_RightAfter__when__carrot_delimited_by_t__then__returns_empty()
        {
            Assert.AreEqual("", "carrot".RightAfter("t"));
        }

        [TestMethod]
        public void String_RightAfter__when__carrot_delimited_by_o__then__returns_empty()
        {
            Assert.AreEqual("t", "carrot".RightAfter("o"));
        }

        [TestMethod]
        public void String_RightAfter__when__pp_delimited_by_p__then__returns_p()
        {
            Assert.AreEqual("p", "pp".RightAfter("p"));
        }

        [TestMethod]
        public void String_RightAfter__when__empty_delimited_by_p__then__returns_empty()
        {
            Assert.AreEqual("", "".RightAfter("p"));
        }

        [TestMethod]
        public void String_RightAfter__when__helloMike_delimited_by_hello__then__returns_Mike()
        {
            Assert.AreEqual("Mike", "helloMike".RightAfter("hello"));
        }

        [TestMethod]
        public void String_RightAfter__when__helloMike_delimited_by_llo__then__returns_Mike()
        {
            Assert.AreEqual("Mike", "helloMike".RightAfter("llo"));
        }

        [TestMethod]
        public void String_RightAfter__when__pp_delimited_by_empty__then__returns_pp()
        {
            Assert.AreEqual("pp", "pp".RightAfter(""));
        }

        [TestMethod]
        public void String_RightAfter__when__empty_delimited_by_empty__then__returns_empty()
        {
            Assert.AreEqual("", "".RightAfter(""));
        }

        [TestMethod]
        public void String_Mid__when__0123456_start_4_end_6__then__returns_45()
        {
            Assert.AreEqual("45", "0123456".Mid(4, 6));
        }

        [TestMethod]
        public void String_SurroundWith__when__Sun_surrounded_with_moon__then__moonSunmoon()
        {
            Assert.AreEqual("moonSunmoon", "Sun".SurroundWith("moon"));
        }

        [TestMethod]
        public void String_SurroundWith_when__single_quote_char__then__works()
        {
            Assert.AreEqual("'Sun'", "Sun".SurroundWith("'"[0]));
        }

        [TestMethod]
        public void String_SurroundWith_when__double_quote_char__then__works()
        {
            string actual = "Sun".SurroundWith(true);
            Assert.AreEqual("\"Sun\"", actual);
        }

        [TestMethod]
        public void Guid_IfEmpty__when__Guid_is_empty__then__returns_new_guid()
        {
            var empty = Guid.Empty;
            var expected = Guid.NewGuid();
            var actual = empty.IfEmpty(expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Guid_IfEmpty__when__Guid_is_not_empty__then__returns_guid_value()
        {
            var notEmpty = Guid.NewGuid();
            var expected = notEmpty;
            var actual = notEmpty.IfEmpty(expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exception_IsSafeToHandle__when__ApplicationException__then__catches_it()
        {
            try
            {
                throw new ApplicationException();
            }
#pragma warning disable CS0618 // Type or member is obsolete
            catch (Exception ex) when (ex.IsSafeToHandle())
#pragma warning restore CS0618 // Type or member is obsolete
            {
                Assert.IsTrue(true);
                return;
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Exception_IsSafeToHandle__when__ApplicationException__then__bubbles_it()
        {
            try
            {
                try
                {
                    throw new NullReferenceException();
                }
#pragma warning disable CS0618 // Type or member is obsolete
                catch (Exception ex) when (ex.IsSafeToHandle())
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    Assert.Fail();
                    return;
                }
            }
            catch (NullReferenceException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
