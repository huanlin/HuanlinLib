using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Huanlin.Windows.Forms
{
    /// <summary>
    /// 提供簡易文字輸入的對話窗。
    /// </summary>
    public class InputBox : System.Windows.Forms.Form
    {
        protected System.Windows.Forms.Button btnOk;
        protected System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.Label lblPrompt;
        protected System.Windows.Forms.TextBox txtText;
        protected System.Windows.Forms.ErrorProvider errorProviderText;
        private IContainer components;

        /// <summary>
        /// Delegate used to validate the object
        /// </summary>
        private InputBoxValidatingHandler m_Validator;

        private InputBox()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtText = new System.Windows.Forms.TextBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.errorProviderText = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderText)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(274, 77);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 32);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "確定";
            this.btnOk.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(355, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(16, 37);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(416, 25);
            this.txtText.TabIndex = 1;
            this.txtText.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxText_Validating);
            this.txtText.TextChanged += new System.EventHandler(this.textBoxText_TextChanged);
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(13, 19);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(48, 15);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "prompt";
            // 
            // errorProviderText
            // 
            this.errorProviderText.ContainerControl = this;
            this.errorProviderText.DataMember = "";
            // 
            // InputBox
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 18);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(446, 121);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Title";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderText)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Validator = null;
            this.Close();
        }


        /// <summary>
        /// 顯示文字輸入對話窗。
        /// </summary>
        /// <param name="prompt">提示文字。</param>
        /// <param name="title">對話窗標題。</param>
        /// <param name="validator">驗證輸入文字的事件處理常式。</param>
        /// <param name="xpos">對話窗的 X 座標。若為 -1 則採用預設值（螢幕中央）。</param>
        /// <param name="ypos">對話窗的 Y 座標。若為 -1 則採用預設值（螢幕中央）。</param>
        /// <param name="maxLength">最大長度。</param>
        /// <param name="value">文字方塊的預設值／傳回值。</param>
        /// <returns>傳回 DialogResult，若為 DialogResult.OK，則一併傳回 value 參數。</returns>
        public static DialogResult ShowDialog(string prompt, string title, InputBoxValidatingHandler validator, int xpos, int ypos, int maxLength, ref string value)
        {
            using (InputBox form = new InputBox())
            {
                form.lblPrompt.Text = prompt;
                form.Text = title;
                form.txtText.MaxLength = maxLength;
                form.txtText.Text = value;
                if (xpos >= 0 && ypos >= 0)
                {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Left = xpos;
                    form.Top = ypos;
                }
                form.Validator = validator;

                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    value = form.txtText.Text;
                }
                return result;
            }
        }

        /// <summary>
        /// Displays a prompt in a dialog box, waits for the user to input text or click a button.
        /// </summary>
        /// <param name="prompt">String expression displayed as the message in the dialog box</param>
        /// <param name="title">String expression displayed in the title bar of the dialog box</param>
        /// <param name="validator">Delegate used to validate the text</param>
        /// <param name="value">文字方塊的預設值／傳回值。</param>
        /// <returns>傳回 DialogResult，若為 DialogResult.OK，則一併傳回 value 參數。</returns>
        public static DialogResult ShowDialog(string prompt, string title, InputBoxValidatingHandler validator, ref string value)
        {
            return ShowDialog(prompt, title, validator, -1, -1, -1, ref value);
        }

        public static DialogResult ShowDialog(string prompt, string title, int maxLength, ref string value)
        {
            return ShowDialog(prompt, title, null, -1, -1, maxLength, ref value);
        }


        /// <summary>
        /// Reset the ErrorProvider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxText_TextChanged(object sender, System.EventArgs e)
        {
            errorProviderText.SetError(txtText, "");
        }

        /// <summary>
        /// Validate the Text using the Validator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxText_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Validator != null)
            {
                InputBoxValidatingArgs args = new InputBoxValidatingArgs();
                args.Text = txtText.Text;
                Validator(this, args);
                if (args.Cancel)
                {
                    e.Cancel = true;
                    errorProviderText.SetError(txtText, args.Message);
                }
            }
        }

        protected InputBoxValidatingHandler Validator
        {
            get
            {
                return (this.m_Validator);
            }
            set
            {
                this.m_Validator = value;
            }
        }

    }

    /// <summary>
    /// EventArgs used to Validate an InputBox
    /// </summary>
    public class InputBoxValidatingArgs : EventArgs
    {
        public string Text;
        public string Message;
        public bool Cancel;
    }

    /// <summary>
    /// Delegate used to Validate an InputBox
    /// </summary>
    public delegate void InputBoxValidatingHandler(object sender, InputBoxValidatingArgs e);

}
