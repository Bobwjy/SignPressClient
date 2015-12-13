using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Office.MyWord.MyWord
{
    /// <summary>
    /// 其实，这里的MyWordWithTemplate和MyWord不能算是一种继承上的关系
    /// 这只不过是获得表格不一样的途径而已，但是，我觉得未来不让MyWOrd这个类过于庞大
    /// 所以我使用了继承,这里的模板只能有一个表格，也可稍加修改，变得更加通用一些
    /// </summary>
    public class MyWordWithTemplate : MyWord
    {
        private string _TemplateFilePath;
        /// <summary>
        /// 通过模板构造word文件
        /// </summary>
        /// <param name="templateFilePath">保存word路径</param>
        public MyWordWithTemplate(string templateFilePath)
            : base()
        {
            this._TemplateFilePath = templateFilePath;
        }
        /// <summary>
        /// 通过模板构造word文件
        /// </summary>
        /// <param name="modelFileName">word模板路径</param>
        /// <param name="saveFullWordName">保存word路径</param>
        public MyWordWithTemplate(string templateFilePath, string saveFilePath)
            : base(saveFilePath)
        {
            this._TemplateFilePath = templateFilePath;
        }
        /// <summary>
        /// 打开word模板
        /// </summary>
        public void OpenWordTemplate()
        {
            _MainDoc = OpenWordTemplateFile(_TemplateFilePath);
            _WordTable = new MyWordTable.WordTable();
            _WordTable.TableOne = _MainDoc.Tables[1];
        }

        private Microsoft.Office.Interop.Word.Document OpenWordTemplateFile(string FileNameStr)
        {
            Microsoft.Office.Interop.Word.Document oDoc = new Microsoft.Office.Interop.Word.Document();

            object Obj_FileName = FileNameStr;
            object Visible = false;
            object ReadOnly = false;
            object missing = System.Reflection.Missing.Value;
            _WordApplicMain = new Microsoft.Office.Interop.Word.Application();
            Object Nothing = System.Reflection.Missing.Value;
            oDoc = _WordApplicMain.Documents.Open(ref Obj_FileName, ref missing, ref ReadOnly, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref Visible,
                ref missing, ref missing, ref missing,
                ref missing);
            oDoc.Activate();

            return oDoc;
        }
        /// <summary>
        /// 替换模板中的标签
        /// </summary>
        /// <param name="replaceKeysWithValue">利用字典key对应的value值来替换key值</param>
        public void ReplaceLabel(Dictionary<string, string> replaceKeysWithValue)
        {
            foreach (var key in replaceKeysWithValue.Keys)
                _ReplaceLabelWithString(key, replaceKeysWithValue[key]);

        }

        private void _ReplaceLabelWithString(string key, string replaceContent)
        {
            object FindText, ReplaceText, ReplaceType;
            object MissingValue = Type.Missing;

            ReplaceType = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;

            FindText = key;
            ReplaceText = replaceContent;

            _MainDoc.Content.Find.Execute(ref   FindText, ref   MissingValue, ref   MissingValue,
                                                         ref   MissingValue, ref   MissingValue,
                                                         ref   MissingValue, ref   MissingValue,
                                                         ref   MissingValue, ref   MissingValue,
                                                         ref   ReplaceText, ref   ReplaceType,
                                                         ref   MissingValue, ref   MissingValue,
                                                         ref   MissingValue, ref   MissingValue);

        }

        /// <summary>
        /// 保存word文件
        /// </summary>
        public void SaveTemplateWord()
        {
            base.SaveWord();
        }


        /// <summary>
        /// 复制一个新的表格
        /// </summary>
        public void CopyAndPasteTemplateTable()
        {
            _MainDoc.Tables[1].Select();
            _WordApplicMain.Selection.Copy();
            Feed();
            _WordApplicMain.Selection.Paste();
        }

        /// <summary>
        /// 设置当前使用的表格
        /// </summary>
        /// <param name="tableNumber"></param>
        public void SetCurrentTable(int tableNumber)
        {
            _WordTable.TableOne = _MainDoc.Tables[tableNumber];
        }

    }
}
