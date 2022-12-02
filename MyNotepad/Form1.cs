using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MyNotepad
{
    public partial class Form1 : Form
    {
        string path;
        public Form1()
        {

            InitializeComponent();
        }
        // Файл

        // Создать
        private async void New_Click(object sender, EventArgs e)
        {
            path = string.Empty;
            richTextBox1.Clear();
        }

        // Создать в новом окне
        private void NewWindow_Click(object sender, EventArgs e)
        {
            Form1 mn = new Form1();
            mn.Show();
        }

        // Открыть
        private void Open_Click(object sender, EventArgs e) 
        {
            using (OpenFileDialog ofd = new OpenFileDialog() 
            { Filter = "Text Documents|*.txt", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(ofd.FileName))
                        {
                            path = ofd.FileName;
                            Task<string> text = sr.ReadToEndAsync();
                            richTextBox1.Text = text.Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Сохранить 
        private async void Save_Click(object sender, EventArgs e) 
        {
            if (string.IsNullOrEmpty(path))
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            path = sfd.FileName;
                            using (StreamWriter sw = new StreamWriter(sfd.FileName))
                            {
                                await sw.WriteAsync(richTextBox1.Text);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        await sw.WriteAsync(richTextBox1.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Сохранить как...
        private async void SaveAs_Click(object sender, EventArgs e) 
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            await sw.WriteAsync(richTextBox1.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        // Печать
        private void Print_Click(object sender, EventArgs e)
        {
            DialogResult dr = printDialog1.ShowDialog();
        }
        // Выход
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Измненить

        // Отменить
        private void Undo_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanUndo == true)
            {
                richTextBox1.Undo();
            }
        }

        // Вернуть(Повторить)
        private void Redo_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanRedo == true)
            {
                richTextBox1.Redo();
            }
        }

        // Вырезать
        private void Cut_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                richTextBox1.Cut();
            }
        }

        // Копировать
        private void Copy_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        // Вставить
        private void Paste_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                richTextBox1.Paste();
            }
        }

        // Выделить все
        private void SelectAll_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        // Формат
        // Шрифт
        private void Font_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog1.Font;
            }
        }

        // Цвет
        private void Colour_Click(object sender, EventArgs e)
        {
            ColorDialog fc = new ColorDialog();
            if (fc.ShowDialog() == DialogResult.OK)
                richTextBox1.ForeColor = fc.Color;
        }

        // Перенос по словам
        private void WordWrap_Click(object sender, EventArgs e)
        {
            if (WordWrap.Checked)
            {
                richTextBox1.WordWrap = true;
            }

            else
            {
                richTextBox1.WordWrap = false;
            }
        }

        // Вернутся в начало текста
        private void JumpToTop_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.ScrollToCaret();
        }

        // Вернутся в конец текста
        private void JumpToBottom_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        // Найти
        private void Find_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.Focus();
        }

        // Найти и заменить
        private void FindAndReplace_Click(object sender, EventArgs e)
        {
            toolStripTextBox2.Focus();
        }

        // Замена
        private void Replace_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox2.Text != null && !string.IsNullOrWhiteSpace(toolStripTextBox2.Text) 
                && toolStripTextBox3.Text != null && !string.IsNullOrWhiteSpace(toolStripTextBox3.Text)) /// что это?
            {
                richTextBox1.Text = richTextBox1.Text.Replace(toolStripTextBox2.Text, toolStripTextBox3.Text);
                toolStripTextBox2.Text = "";
                toolStripTextBox3.Text = "";
            }
        }

        // Поиск 
        private void Search(object sender, EventArgs e)
        {
            string keywords = toolStripTextBox1.Text;
            MatchCollection keywordMatches = Regex.Matches(richTextBox1.Text, keywords);

            int originalIndex = richTextBox1.SelectionStart;
            int originalLenght = richTextBox1.SelectionLength;

            toolStripTextBox1.Focus();

            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionBackColor = Color.White;

            foreach (Match m in keywordMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionBackColor = Color.DodgerBlue;
            }

            richTextBox1.SelectionStart = originalIndex;
            richTextBox1.SelectionLength = originalLenght;
            richTextBox1.SelectionBackColor = Color.White;
        }

        // О программе
        private void AboutTheProgram_Click(object sender, EventArgs e)
        {
            using (AboutForm frm = new AboutForm())
            {
                frm.ShowDialog();
            }
        }
    }
}