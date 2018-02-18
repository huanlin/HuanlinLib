using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Huanlin.Braille.Data;

namespace Huanlin.Braille.Converters
{
    public abstract class WordConverter
    {
        internal abstract BrailleTableBase BrailleTable
        {
            get;
        }

        public abstract List<BrailleWord> Convert(Stack<char> charStack, ContextTagManager context);

        public virtual string Convert(string text)
        {
            return BrailleTable.Find(text);
        }

        /// <summary>
        /// �N�ǤJ���r��]�r���^�ഫ���I�r�C
        /// </summary>
        /// <param name="text">�r��]�@�Ӧr���^�C</param>
        /// <returns>�Y���w���r���ഫ���\�A�h�Ǧ^�ഫ���᪺�I�r����A�_�h�Ǧ^ null�C</returns>
        protected virtual BrailleWord ConvertToBrailleWord(string text)
        {
            BrailleWord brWord = new BrailleWord();
            brWord.Text = text;

            string brCode;

            brCode = BrailleTable.Find(text);
            if (!String.IsNullOrEmpty(brCode))
            {
                brWord.AddCell(brCode);
                return brWord;
            }

            brWord.Clear();
            brWord = null;
            return null;
        }

    }
}
