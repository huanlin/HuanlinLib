using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using Huanlin.Helpers;
using Huanlin.Braille.Data;

namespace Huanlin.Braille
{
    public enum BrailleLanguage
    {
        Neutral = 0,
        Chinese,
        English
    };


	/// <summary>
	/// �N��@�Ӥ���r�A���t�`���X�P�I�r�ȡC
	/// </summary>
    [Serializable]
    [DataContract]
	public class BrailleWord
	{
        private static BrailleWord m_Blank;

		private string m_Text;	// �r���C�i��O�@�ӭ^�Ʀr�B����r�B���μ��I�Ÿ��B�����r�����I�Ÿ��A�Ҧp�G�}�鸹�C
        private BrailleLanguage m_Language;     // �y����O�C�Ψ��ѧO�O�����٬O�^��C
		private BrailleCellList m_CellList;	    // �I�r�C        
        private List<string> m_PhoneticCodes;   // �Ҧ��`���զr�r�ڡ]�H�䴩�}���r�^�C
        private int m_ActivePhoneticIndex;      // �ثe�ϥΪ��`���զr�r�گ��ޡC
        private bool m_DontBreakLineHere;       // �]�w/�P�O�b�_��ɬO�_���_�b�o�Ӧr�C

        [NonSerialized]
        private string m_PhoneticCode;          // �`���r�ڡ]�t�u�v�w�^�C

        [NonSerialized]
        private bool m_IsPolyphonic;          // �O�_���h���r�C

        [NonSerialized]
        private bool m_IsContextTag;            // �O�_�����Ҽ��ҡ]���Ҽ��Ҥ��|�]�t��ڥi�L���I�r�^

        [NonSerialized]
        private bool m_NoDigitCell;             // �O�_���[�ƲšC

		[NonSerialized]
		private bool m_IsEngPhonetic;			// �O�_���^�y���С]�ΨӧP�_���n�[�Ť�^.

		//private bool m_QuotationResolved;	// �O�_�w�g�ѧO�X���k�޸��]�^�媺��޸��M���޸����O�P�@�ӲŸ��A���I�r���P�^

        static BrailleWord()
        {
            m_Blank = BrailleWord.NewBlank();
        }

        /// <summary>
        /// �إߨöǦ^���Ҽ��Ҫ� BrailleWord�C
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static BrailleWord CreateAsContextTag(string tagName)
        {
            BrailleWord brWord = new BrailleWord();
            brWord.Text = tagName;
            brWord.IsContextTag = true;

            return brWord;
        }

		public BrailleWord()
		{
			m_Text = "";
            m_Language = BrailleLanguage.Neutral;
			m_CellList = new BrailleCellList();

            m_PhoneticCodes = new List<string>();
            m_ActivePhoneticIndex = -1;

            m_DontBreakLineHere = false;

            m_IsContextTag = false;
            m_NoDigitCell = false;
			m_IsEngPhonetic = false;
		}

        public BrailleWord(string aWord, BrailleCellCode brCode)
            : this()
        {
            m_Text = aWord;
            m_CellList.Add(BrailleCell.GetInstance(brCode));
        }

        public BrailleWord(string aWord, string brCode) : this()
        {
            m_Text = aWord;
            AddCell(brCode);
        }

		public BrailleWord(string aWord, byte brCode) : this()
		{
			m_Text = aWord;
			m_CellList.Add(BrailleCell.GetInstance(brCode));
		}

		public BrailleWord(string aWord, string phCode, string brCode) : this(aWord, brCode)
		{
            m_Language = BrailleLanguage.Chinese;
			m_PhoneticCodes.Add(phCode);
            m_ActivePhoneticIndex = 0;
		}

		public override int GetHashCode()
		{
			return m_Text.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
				return true;

			BrailleWord brWord = (BrailleWord)obj;

			if (m_CellList.Count != brWord.Cells.Count)
				return false;
			for (int i = 0; i < m_CellList.Count; i++)
			{				
				if (!m_CellList[i].Equals(brWord.Cells[i]))
					return false;
			}

			// �����`���A�]���@�n�`�`�M���Ϊťշd�V�C
			//if (m_PhoneticCode != brWord.PhoneticCode)
			//    return false;

			// ������r�A�]�����ΪťթM�b�Ϊťը���������۵��A��� cells �N���F�C
			//if (m_Text != brWord.Text)
			//
			//{
			//    return false;
			//}

			return true;
		}

		public override string ToString()
		{
			return m_Text;
		}

		public void Clear()
		{
			m_CellList.Clear();
		}

        [DataMember]
		public string Text
		{
			get { return m_Text; }
			set { m_Text = value; }			
		}

        public int CellCount
        {
            get { return m_CellList.Count; }
        }
        
		public List<BrailleCell> Cells
		{
			get
			{
				return m_CellList.Items;
			}
		}

        [DataMember]
        public BrailleCellList CellList
        {
            get
            {
                return m_CellList;
            }
            set
            {
                m_CellList = value;
            }
        }

        [DataMember]
		public string PhoneticCode
		{
			get
			{
                if (String.IsNullOrEmpty(m_PhoneticCode))   // �o�O���F�V�U�ۮe�A�ª��S�� PhoneticCode �ݩʡC
                {
                    // �Y�S���`���r�ڡA�h�Ǧ^�Ŧr��C
                    if (m_PhoneticCodes == null || m_PhoneticCodes.Count < 1 || m_ActivePhoneticIndex < 0)
                    {
                        return "";
                    }
                    return m_PhoneticCodes[m_ActivePhoneticIndex];
                }
                return m_PhoneticCode;
			}
            set
            {
                if (m_PhoneticCode == value)
                    return;
                m_PhoneticCode = value;
/*
                // �Y�S���`���r�ڡA�h�W�[�@�ӡC
                if (m_PhoneticCodes.Count < 1)
                {
                    m_PhoneticCodes.Add(value);
                    m_ActivePhoneticIndex = 0;
                }
                else
                {   // �_�h�]�w�@�Τ����`���r�گ���
                    int i = m_PhoneticCodes.IndexOf(value);
                    if (i < 0)
                    {
                        m_PhoneticCodes.Add(value);
                        i = m_PhoneticCodes.Count - 1;
                        System.Diagnostics.Trace.WriteLine("���w�� BrailleWord.PhoneticCode ���`���r�ڤ��s�b! �w�۰ʥ[�J�C");
                    }
                    m_ActivePhoneticIndex = i;
                }
*/
            }
		}

        [DataMember]
        public bool IsPolyphonic
        {
            get 
            {
                if (m_PhoneticCodes != null && m_PhoneticCodes.Count > 1)   // for �V�U�ۮe.
                {
                    return true;
                }
                return m_IsPolyphonic; 
            }
            set { m_IsPolyphonic = value; }
        }

        [DataMember]
        public bool DontBreakLineHere
        {
            get { return m_DontBreakLineHere; }
            set { m_DontBreakLineHere = value; }
        }

        public List<string> PhoneticCodes
        {
            get
            {
                if (m_PhoneticCodes == null)
                {
                    m_PhoneticCodes = new List<string>();
                }
                return m_PhoneticCodes;
            }
        }

        public int ActivePhoneticIndex
        {
            get
            {
                return m_ActivePhoneticIndex;
            }
            set
            {
                if (value >= m_PhoneticCodes.Count)
                    throw new ArgumentOutOfRangeException();
                m_ActivePhoneticIndex = value;
            }
        }

        public BrailleLanguage Language
        {
            get
            {
                return m_Language;
            }
            set
            {
                m_Language = value;
            }
        }

        public void SetPhoneticCodes(string[] phCodes)
        {
            m_PhoneticCodes.Clear();
            m_PhoneticCodes.AddRange(phCodes);
        }

        /// <summary>
        /// �إߤ@�ӷs�� BrailleWord ����A�ñN�ۤv�����e����ƻs��s������C
        /// </summary>
        /// <returns></returns>
		public BrailleWord Copy()
		{
			BrailleWord newBrWord = new BrailleWord();
			newBrWord.Text = m_Text;
            newBrWord.Language = m_Language;
			newBrWord.DontBreakLineHere = m_DontBreakLineHere;
			newBrWord.NoDigitCell = m_NoDigitCell;

            foreach (BrailleCell brCell in m_CellList.Items)
            {
                newBrWord.Cells.Add(brCell);
            }

            newBrWord.PhoneticCode = this.PhoneticCode;
            newBrWord.IsPolyphonic = this.IsPolyphonic;

/* PhoneticCodes �w�g�n�^�O
            newBrWord.PhoneticCodes.Clear();
            newBrWord.PhoneticCodes.AddRange(m_PhoneticCodes);
            newBrWord.ActivePhoneticIndex = m_ActivePhoneticIndex;
*/
			return newBrWord;
		}

        /// <summary>
        /// �N���w�� BrailleWord ���e����ƻs���ۤv�C
        /// </summary>
        /// <param name="brWord"></param>
        public void Copy(BrailleWord brWord)
        {
            System.Diagnostics.Debug.Assert(brWord != null, "�Ѽ� brWord ���i�� NULL!");

            m_Text = brWord.Text;
            m_Language = brWord.Language;

            m_CellList.Clear();
            foreach (BrailleCell brCell in brWord.CellList.Items)
            {
                m_CellList.Add(brCell);
            }

            m_PhoneticCode = brWord.PhoneticCode;
            m_IsPolyphonic = brWord.m_IsPolyphonic;
/*
            // �ƻs�Ҧ��`���r�ڻP�I�r��C, for �V�U�ۮe.
            if (brWord.PhoneticCodes != null)
            {
                m_PhoneticCodes.Clear();
                m_PhoneticCodes.AddRange(brWord.PhoneticCodes);
                m_ActivePhoneticIndex = brWord.ActivePhoneticIndex;
            }
 */
        }

        /// <summary>
        /// ����w���I�r�r��]16�i��^�ন BrailleCell ����A�å[�J�I�r��C���C
        /// </summary>
        /// <param name="brCode">���[�J��C���I�r�X 16 �i��r��C</param>
        public void AddCell(string brCode)
        {
            if (String.IsNullOrEmpty(brCode))
            {
                return;
            }

            for (int i = 0; i < brCode.Length; i += 2)
            {
                string s = brCode.Substring(i, 2);
                byte aByte = StrHelper.HexStrToByte(s);
                BrailleCell cell = BrailleCell.GetInstance(aByte);
                m_CellList.Add(cell);
            }
        }

        public bool IsWhiteSpace
        {
            get
            {
                if (m_Text.Length == 1)
                {
                    if (Char.IsWhiteSpace(m_Text[0]))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

		public bool IsLetter
		{
            get
            {
                if (m_Text.Length == 1)
                {
                    if (CharHelper.IsAsciiLetter(m_Text[0]))
                    {
                        return true;
                    }
                }
                return false;
            }
		}

		public bool IsDigit
		{
            get
            {
                if (m_Text.Length == 1)
                {
                    if (CharHelper.IsAsciiDigit(m_Text[0]))
                    {
                        return true;
                    }
                }
                return false;
            }
		}

		public bool IsLetterOrDigit
		{
            get
            {
                if (m_Text.Length == 1)
                {
                    if (CharHelper.IsAsciiLetterOrDigit(m_Text[0]))
                    {
                        return true;
                    }
                }
                return false;
            }
		}


        /// <summary>
        /// �Ǧ^���I�r�O�_������r�]�H�O�_���`���r�ڬ��P�_�̾ڡ^�C
        /// �`�N�G�� Language �ݩʵL���ALanguage �ݩʥ]�t������I�Ÿ�
        /// </summary>
        /// <returns></returns>
        public bool IsChinese
        {
            get
            {
                if (!String.IsNullOrEmpty(m_PhoneticCode))
                    return true;
                if (!String.IsNullOrEmpty(m_Text) && Huanlin.TextServices.Chinese.Zhuyin.IsTone(m_Text[0]))
                    return true;
                if (m_PhoneticCodes != null)
                {
                    return (m_PhoneticCodes.Count > 0);
                }
                return false;
            }
        }

        /// <summary>
        /// �إߤ@�ӷs���Ť��I�r����C
        /// </summary>
        /// <returns></returns>
        public static BrailleWord NewBlank()
        {
            return new BrailleWord(" ", BrailleCellCode.Blank);
        }

        /// <summary>
        /// �ˬd���w�� BrailleWord �O�_���Ť�C
        /// </summary>
        /// <param name="brWord"></param>
        /// <returns></returns>
        public static bool IsBlank(BrailleWord brWord)
        {
            if (brWord.Equals(BrailleWord.m_Blank))
                return true;
            return false;
        }

		/// <summary>
		/// �ˬd���w�� BrailleWord ����O�_�S���]�t���󦳷N�q����ơ]�Ť�⦳�N�q����ơ^�C
		/// </summary>
		/// <param name="brWord"></param>
		/// <returns></returns>
		public static bool IsEmpty(BrailleWord brWord)
		{
			if (StrHelper.IsEmpty(brWord.Text) && brWord.CellCount < 1) 
			{
				// ��r���Ŧr��A�B�S�������I�r����A�Y�����Ū� BrailleWord ����.
				return true;
			}
			return false;
		}

        /// <summary>
        /// �O�_�����Ҽ��ҡC
        /// </summary>
        public bool IsContextTag
        {
            get { return m_IsContextTag; }
            set
            {
                m_IsContextTag = value;
                if (m_IsContextTag)    // �p�G�O����r�A�N�n�M���I�r��C
                {
                    m_CellList.Clear();
                }
            }
        }

        public bool NoDigitCell
        {
            get { return m_NoDigitCell; }
            set { m_NoDigitCell = value; }
        }

		public bool IsEngPhonetic
		{
			get { return m_IsEngPhonetic; }
			set { m_IsEngPhonetic = value; }
		}

        /// <summary>
        /// �ˬd���w�� BrailleWord �O�_���Ʀr�s�����_�l�I�r�C��Y�H # �}�Y���Ʀr�C
        /// </summary>
        /// <param name="brWord"></param>
        /// <returns></returns>
        public static bool IsOrderedListItem(BrailleWord brWord)
        {
            if (brWord.Cells.Count < 2)
                return false;
            if (brWord.Cells[0].Value == (byte)BrailleCellCode.Digit) // �H�Ʀr�I�}�Y�H
            {
                // ���ۤ���ĤG��O�_���W���I�A�`�N�o�̪��W���I�ƭȨå��ϥάd��A
                // �ӬO�g���b�{���̡C�Ѧ�: BraillTableEng.xml�C
                // TODO: �令�d��C
                byte value = brWord.Cells[1].Value;
                switch (value) 
                {
                    case 0x01:  case 0x03:
                    case 0x09:  case 0x19:
                    case 0x11:  case 0x0B:
                    case 0x1B:  case 0x13:
                    case 0x4A:  case 0x1A:
                        return true;
                    default:
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// �ˬd���w�� BrailleWord �O�_����y���I�Ÿ��C
        /// </summary>
        /// <param name="brWord"></param>
        /// <returns></returns>
        public static bool IsChinesePunctuation(BrailleWord brWord)
        {
            ChineseBrailleTable chtBrlTbl = ChineseBrailleTable.GetInstance();
            string brCode = chtBrlTbl.FindPunctuation(brWord.Text);
            if (String.IsNullOrEmpty(brCode))
                return false;
            return true;
        }
	}
}
