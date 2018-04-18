using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using Huanlin.Text;
using Huanlin.Collections;
using Huanlin.Cryptography;

namespace Example
{
    public partial class CryptoForm : Form
    {
        private DigitalSignatureDemo m_DS;

        public CryptoForm()
        {
            InitializeComponent();
        }

        private SymmetricCrypto CreateSymmetricCrypto()
        {
            SymmetricCrypto.Provider provider;
            bool useDefaultInitVector = true;

            switch (cboSymmAlgorithm.SelectedIndex)
            {
                case 0:
                    provider = SymmetricCrypto.Provider.DES;
                    break;
                case 1:
                    provider = SymmetricCrypto.Provider.RC2;
                    break;
                case 2:
                    provider = SymmetricCrypto.Provider.Rijndael;
                    break;
                case 3:
                    provider = SymmetricCrypto.Provider.TripleDES;
                    break;
                default:
                    throw new Exception("沒有對應的加密演算法!");
            }
            return new SymmetricCrypto(provider, useDefaultInitVector);
        }

        private void btnSymmetricEncrypt_Click(object sender, EventArgs e)
        {
            SymmetricCrypto crypto = CreateSymmetricCrypto();

            ByteArray key = new ByteArray(txtSymmetricKey.Text);
            ByteArray data = new ByteArray(txtRawData.Text);

            ByteArray encrypted = crypto.Encrypt(data, key);
            txtSymmetricEncrypted.Text = encrypted.Base64String;
        }        

        private void btnSymmetricDecrypt_Click(object sender, EventArgs e)
        {
            SymmetricCrypto crypto = CreateSymmetricCrypto(); 
            
            ByteArray key = new ByteArray(txtSymmetricKey.Text);

            byte[] encrypted = Convert.FromBase64String(txtSymmetricEncrypted.Text);
            ByteArray data = new ByteArray(encrypted);

            ByteArray decrypted = crypto.Decrypt(data, key);
            txtSymmetricDecrypted.Text = decrypted.Text;
        }

        private void btnAsymmEncrypt_Click(object sender, EventArgs e)
        {
            AsymmetricCrypto crypto = new AsymmetricCrypto();

            // 產生公鑰與私鑰
            AsymmetricCrypto.PublicKey pubKey = new AsymmetricCrypto.PublicKey();
            AsymmetricCrypto.PrivateKey priKey = new AsymmetricCrypto.PrivateKey();
            crypto.GenerateNewKeyset(ref pubKey, ref priKey);

            // 用公鑰加密
            ByteArray inputData = new ByteArray(txtRawData.Text);
            ByteArray encrypted = crypto.Encrypt(inputData, pubKey);
            txtAsymmEncrypted.Text = encrypted.Base64String;

            // 用私鑰解密
            ByteArray decrypted = crypto.Decrypt(encrypted, priKey);
            txtAsymmDecrypted.Text = decrypted.Text;
        }

        private void CryptoForm_Load(object sender, EventArgs e)
        {
            cboSymmAlgorithm.SelectedIndex = 0;

            m_DS = new DigitalSignatureDemo();
        }


        private void btnSign_Click(object sender, EventArgs e)
        {
            ByteArray signature = m_DS.Sign(txtRawData.Text);
            txtSignature.Text = signature.Base64String;
        }

        private void btnVerifySignature_Click(object sender, EventArgs e)
        {
            ByteArray data = new ByteArray(txtRawData.Text);
            ByteArray signature = new ByteArray(Convert.FromBase64String(txtSignature.Text));

            if (m_DS.Verify(data, signature))
            {
                MessageBox.Show("驗證成功!");
            }
            else
            {
                MessageBox.Show("簽章無效!");
            }
        }

    }
}