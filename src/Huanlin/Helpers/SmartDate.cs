using System;

namespace Huanlin.Helpers
{
    /// <summary>
    /// SmartDate ���ѤF�i�H���ŭȪ�������O�A�H�K�Ω��Ʈw������ɶ����C
    /// </summary>
    /// <remarks>
    /// �ק�� CSLA .NET ���O�w�C
    /// See Chapter 5 for a full discussion of the need for this
    /// data type and the design choices behind it.
    /// </remarks>
    [Serializable()]
    sealed public class SmartDate : IComparable
    {
        DateTime _date;
        bool _emptyIsMin;

        #region Constructors

        /// <summary>
        /// Creates a new SmartDate object.
        /// </summary>
        public SmartDate()
        {
            _emptyIsMin = false;
            _date = DateTime.MaxValue;
        }

        /// <summary>
        /// Creates a new SmartDate object.
        /// </summary>
        /// <param name="emptyIsMin">
        /// Indicates whether an empty date is the min or max date value.</param>
        public SmartDate(bool emptyIsMin)
        {
            _emptyIsMin = emptyIsMin;
            if (_emptyIsMin)
                _date = DateTime.MinValue;
            else
                _date = DateTime.MaxValue;
        }

        /// <summary>
        /// Creates a new SmartDate object.
        /// </summary>
        /// <remarks>
        /// An empty date will be treated as the smallest
        /// possible date.
        /// </remarks>
        /// <param name="date">The initial value of the object.</param>
        public SmartDate(DateTime date)
        {
            _emptyIsMin = false;
            _date = date;
        }

        /// <summary>
        /// Creates a new SmartDate object.
        /// </summary>
        /// <remarks>
        /// An empty date will be treated as the smallest
        /// possible date.
        /// </remarks>
        /// <param name="date">The initial value of the object.</param>
        public SmartDate(string date)
        {
            _emptyIsMin = false;
            Text = date;
        }

        /// <summary>
        /// SmartDate �غc�禡�C
        /// </summary>
        /// <param name="date"></param>
        public SmartDate(object date)
        {
            _emptyIsMin = false;
            if (date == null || date == DBNull.Value)
            {
                _date = DateTime.MaxValue;
            }
            else
            {
                _date = Convert.ToDateTime(date);
            }
        }

        /// <summary>
        /// Creates a new SmartDate object.
        /// </summary>
        /// <param name="date">The initial value of the object.</param>
        /// <param name="emptyIsMin">
        /// Indicates whether an empty date is the min or max date value.</param>
        public SmartDate(DateTime date, bool emptyIsMin)
        {
            _emptyIsMin = emptyIsMin;
            _date = date;
        }

        /// <summary>
        /// Creates a new SmartDate object.
        /// </summary>
        /// <param name="date">The initial value of the object (as text).</param>
        /// <param name="emptyIsMin">
        /// Indicates whether an empty date is the min or max date value.</param>
        public SmartDate(string date, bool emptyIsMin)
        {
            _emptyIsMin = emptyIsMin;
            this.Text = date;
        }

        #endregion

        #region Text Support

        string _format = "{0:d}";

        /// <summary>
        /// Gets or sets the format string used to format a date
        /// value when it is returned as text.
        /// </summary>
        /// <remarks>
        /// The format string should follow the requirements for 
        /// standard .NET string formatting statements.
        /// </remarks>
        /// <value>A format string.</value>
        public string FormatString
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        /// <summary>
        /// Gets or sets the date value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property can be used to set the date value by passing a
        /// text representation of the date. Any text date representation
        /// that can be parsed by the .NET runtime is valid.
        /// </para><para>
        /// When the date value is retrieved via this property, the text
        /// is formatted by using the format specified by the 
        /// <see cref="P:CSLA.SmartDate.FormatString" /> property. The 
        /// default is the "d", or short date, format.
        /// </para>
        /// </remarks>
        /// <returns></returns>
        public string Text
        {
            get
            {
                return DateToString(_date, _format, _emptyIsMin);
            }
            set
            {
                if (value == null)
                    _date = StringToDate(string.Empty, _emptyIsMin);
                else
                    _date = StringToDate(value, _emptyIsMin);
            }
        }

        /// <summary>
        /// Returns a text representation of the date value.
        /// </summary>
        public override string ToString()
        {
            return this.Text;
        }

        #endregion

        #region Date Support

        /// <summary>
        /// Gets or sets the date value.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        #endregion

        #region DBValue

        /// <summary>
        /// Gets a database-friendly version of the date value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the SmartDate contains an empty date, this returns DBNull. Otherwise
        /// the actual date value is returned as type Date.
        /// </para><para>
        /// This property is very useful when setting parameter values for
        /// a Command object, since it automatically stores null values into
        /// the database for empty date values.
        /// </para><para>
        /// When you also use the SafeDataReader and its GetSmartDate method,
        /// you can easily read a null value from the database back into a
        /// SmartDate object so it remains considered as an empty date value.
        /// </para>
        /// </remarks>
        public object DBValue
        {
            get
            {
                if (this.IsEmpty)
                    return DBNull.Value;
                else
                    return _date;
            }
        }

        #endregion

        #region Empty Dates

        /// <summary>
        /// Indicates whether this object contains an empty date.
        /// </summary>
        /// <returns>True if the date is empty.</returns>
        public bool IsEmpty
        {
            get
            {
                if (_emptyIsMin)
					return IsSameDateTime(_date, DateTime.MinValue);
                else
                    return IsSameDateTime(_date, DateTime.MaxValue);
            }
        }

		/// <summary>
		/// ����� DateTime �O�_�۵��C
		/// �ѩ�s�J��Ʈw����A���X�Ӫ�����ɶ��|���ǷL�~�t�A
		/// �]���Y�n���q��Ʈw���X������ɶ��A�i�I�s���禡�C
		/// </summary>
		/// <param name="dt1"></param>
		/// <param name="dt2"></param>
		/// <returns></returns>
		public bool IsSameDateTime(DateTime dt1, DateTime dt2)
		{
			long diff = dt1.Ticks - dt2.Ticks;
			if (diff < 0)
				diff = 0 - diff;
			if (diff < 50000)	// ���\���~�t (���: ticks)
			{
				return true;
			}
			return false;
		}

        /// <summary>
        /// �ˬd���w�� SmartDate �ܼƬ� null �ΪŪ�����C
        /// </summary>
        /// <param name="value">SmartDate �ܼ�</param>
        /// <returns>�Y�� </returns>
        public static bool IsNullOrEmpty(SmartDate value)
        {
            return (value == null || value.IsEmpty);
        }

        /// <summary>
        /// Indicates whether an empty date is the min or max possible date value.
        /// </summary>
        /// <remarks>
        /// Whether an empty date is considered to be the smallest or largest possible
        /// date is only important for comparison operations. This allows you to
        /// compare an empty date with a real date and get a meaningful result.
        /// </remarks>
        /// <returns>True if an empty date is the smallest 
        /// date, False if it is the largest.</returns>
        public bool EmptyIsMin
        {
            get
            {
                return _emptyIsMin;
            }
        }       

        #endregion

        #region Conversion Functions

        /// <summary>
        /// Converts a text date representation into a Date value.
        /// </summary>
        /// <remarks>
        /// An empty string is assumed to represent an empty date. An empty date
        /// is returned as the MinValue.
        /// </remarks>
        /// <param name="date">The text representation of the date.</param>
        /// <returns>A Date value.</returns>
        static public DateTime StringToDate(string date)
        {
            return StringToDate(date, true);
        }

        /// <summary>
        /// Converts a text date representation into a Date value.
        /// </summary>
        /// <remarks>
        /// An empty string is assumed to represent an empty date. An empty date
        /// is returned as the MinValue or MaxValue of the Date datatype depending
        /// on the EmptyIsMin parameter.
        /// </remarks>
        /// <param name="date">The text representation of the date.</param>
        /// <param name="emptyIsMin">
        /// Indicates whether an empty date is the min or max date value.</param>
        /// <returns>A Date value.</returns>
        static public DateTime StringToDate(string date, bool emptyIsMin)
        {
            if (date.Length == 0)
            {
                if (emptyIsMin)
                    return DateTime.MinValue;
                else
                    return DateTime.MaxValue;
            }
            else
            {
                string ldate = date.ToLower();
                if (ldate == "today" || ldate == "t" || ldate == ".")
                    return DateTime.Now;
                else
                {
                    if (ldate == "yesterday" || ldate == "y" || ldate == "-")
                    {
                        return DateTime.Now.AddDays(-1);
                    }
                    else
                    {
                        if (ldate == "tomorrow" || ldate == "tom" || ldate == "+")
                        {
                            return DateTime.Now.AddDays(1);
                        }
                        else
                        {
                            return DateTime.Parse(date);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts a date value into a text representation.
        /// </summary>
        /// <remarks>
        /// The date value is considered empty assuming EmptyIsMin is true. 
        /// If the date is empty, this
        /// method returns an empty string. Otherwise it returns the date
        /// value formatted based on the FormatString parameter.
        /// </remarks>
        /// <param name="date">The date value to convert.</param>
        /// <param name="formatString">The format string used to format the date into text.</param>
        /// <returns>Text representation of the date value.</returns>
        static public string DateToString(DateTime date, string formatString)
        {
            return DateToString(date, formatString, true);
        }

        /// <summary>
        /// Converts a date value into a text representation.
        /// </summary>
        /// <remarks>
        /// Whether the date value is considered empty is determined by
        /// the EmptyIsMin parameter value. If the date is empty, this
        /// method returns an empty string. Otherwise it returns the date
        /// value formatted based on the FormatString parameter.
        /// </remarks>
        /// <param name="date">The date value to convert.</param>
        /// <param name="formatString">The format string used to format the date into text.</param>
        /// <param name="emptyIsMin">
        /// Indicates whether an empty date is the min or max date value.</param>
        /// <returns>Text representation of the date value.</returns>
        static public string DateToString(DateTime date, string formatString,
                                          bool emptyIsMin)
        {
            if (emptyIsMin && (date == DateTime.MinValue))
                return string.Empty;
            else
                if (!emptyIsMin && (date == DateTime.MaxValue))
                    return string.Empty;
                else
                    return string.Format(formatString, date);
        }

        #endregion

        #region Operators

        static public bool operator >(SmartDate date1, SmartDate date2)
        {
            return date1.Date > date2.Date;
        }

        static public bool operator >(SmartDate date1, DateTime date2)
        {
            return date1.Date > date2;
        }

        static public bool operator <(SmartDate date1, SmartDate date2)
        {
            return date1.Date < date2.Date;
        }

        static public bool operator <(SmartDate date1, DateTime date2)
        {
            return date1.Date < date2;
        }

        static public bool operator ==(SmartDate date1, SmartDate date2)
        {
            return Equals(date1, date2);
        }

        static public bool operator ==(SmartDate date1, DateTime date2)
        {
            return Equals(date1, date2);
        }

        static public bool operator !=(SmartDate date1, SmartDate date2)
        {
            return !Equals(date1, date2);
        }

        static public bool operator !=(SmartDate date1, DateTime date2)
        {
            return !Equals(date1, date2);
        }

        static public SmartDate operator +(SmartDate d, TimeSpan t)
        {
            if (d.IsEmpty)
                return d;
            else
                return new SmartDate(d.Date + t, d.EmptyIsMin);
        }

        static public SmartDate operator -(SmartDate d, TimeSpan t)
        {
            if (d.IsEmpty)
                return d;
            else
                return new SmartDate(d.Date - t, d.EmptyIsMin);
        }

        public override bool Equals(object o)
        {
            if (o is SmartDate)
            {
                SmartDate tmp = o as SmartDate;
                if (this.IsEmpty && tmp.IsEmpty)
                    return true;
                else
                    return _date.Equals(((SmartDate)o).Date);
            }
            else if (o is DateTime)
                return _date.Equals((DateTime)o);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _date.GetHashCode();
        }

        #endregion

        #region Manipulation Functions

        /// <summary>
        /// Compares one SmartDate to another.
        /// </summary>
        /// <remarks>
        /// This method works the same as the CompareTo method
        /// on the Date datetype, with the exception that it
        /// understands the concept of empty date values.
        /// </remarks>
        /// <param name="Value">The date to which we are being compared.</param>
        /// <returns>
        /// A value indicating if the comparison date is less than, 
        /// equal to or greater than this date.</returns>
        public int CompareTo(SmartDate date)
        {
            if (this.IsEmpty && date.IsEmpty)
                return 0;
            else
                return _date.CompareTo(date.Date);
        }

        int IComparable.CompareTo(object value)
        {
            if (value is SmartDate)
                return CompareTo((SmartDate)value);
            else
                throw new ArgumentException("�޼� value ���O SmartDate ���O!");
        }

        /// <summary>
        /// Adds a TimeSpan onto the object.
        /// </summary>
        public DateTime Add(TimeSpan span)
        {
            if (IsEmpty)
                return _date;
            else
                return _date.Add(span);
        }

        /// <summary>
        /// Subtracts a TimeSpan from the object.
        /// </summary>
        public DateTime Subtract(TimeSpan span)
        {
            if (IsEmpty)
                return _date;
            else
                return _date.Subtract(span);
        }

        /// <summary>
        /// Subtracts a DateTime from the object.
        /// </summary>
        public TimeSpan Subtract(DateTime date)
        {
            if (IsEmpty)
                return TimeSpan.Zero;
            else
                return _date.Subtract(date);
        }

        #endregion
    }
}
