using System;
using System.Windows.Forms;
using ProjetoBasquete.Views;

namespace ProjetoBasquete
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Tela de login primeiro
            using var login = new FrmLogin();
            if (login.ShowDialog() == DialogResult.OK)
            {
                // Após login bem-sucedido, abre o formulário principal
                Application.Run(new FrmCadastroJogador());
            }
        }
    }
}