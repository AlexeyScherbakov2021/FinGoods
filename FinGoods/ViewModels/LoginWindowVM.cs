using FinGoods.Commands;
using FinGoods.Models;
using FinGoods.Repository;
using FinGoods.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace FinGoods.ViewModels
{
    internal class LoginWindowVM
    {
        private readonly RepositoryMSSQL<Users> repo = new RepositoryMSSQL<Users>();
        private readonly RepositoryFP repoFP = new RepositoryFP();
        private readonly LoginWindow winLogin;

        public static Users CurrentUser;

        public Users SelectUser { get; set; }
        public IEnumerable<Users> ListUser { get; set; }

        public LoginWindowVM()
        {
            winLogin = App.Current.Windows.OfType<LoginWindow>().First();

            //repo = AllRepo.GetRepoUser();
            //repo = new RepositoryMSSQL<Users>();
            //List<Users> listUserFP = repoFP.GetListUsers();

            ListUser = repo.Items.ToList();
            foreach (var item in ListUser)
            {
                Users us = repoFP.GetUser(item.id);
                if(us != null)
                {
                    item.UserPass = us.UserPass;
                    item.UserName = us.UserName;
                    item.UserFullName = us.UserFullName;
                    repo.Save();
                }
                else
                {
                    repo.Delete(item.id, true);
                }
            }

            ListUser = ListUser.OrderBy(o => o.UserName).ToArray();

            string login = "";
            RegistryKey SoftKey = Registry.CurrentUser.OpenSubKey("SOFTWARE");

            RegistryKey ProgKey = SoftKey.OpenSubKey("FinGoods");

            if (ProgKey != null)
            {
                login = ProgKey.GetValue("login", "").ToString();
                ProgKey.Close();
            }
            SoftKey.Close();

            SelectUser = ListUser.FirstOrDefault(it => it.UserName == login);
        }

        #region Команды
        //--------------------------------------------------------------------------------
        // Команда 
        //--------------------------------------------------------------------------------
        //private readonly ICommand _OkCommand = null;
        public ICommand OkCommand => new LambdaCommand(OnOkCommandExecuted, CanOkCommand);
        private bool CanOkCommand(object p) => p != null && !string.IsNullOrEmpty((p as PasswordBox).Password);
        private void OnOkCommandExecuted(object p)
        {
            if (p is PasswordBox pass)
            {
                //bool res = false;

                //if (SelectUser?.UserPass != null)
                //{
                //    //string hash = Encrypt.Crypt(pass.Password);
                //    res = pass.Password == SelectUser?.u_pass2;
                //}
                //else
                
                bool res = pass.Password == SelectUser?.UserPass;

                if (res)
                {
                    CurrentUser = SelectUser;

                    // записываем в реестр
                    RegistryKey SoftKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                    RegistryKey ProgKey = SoftKey.CreateSubKey("FinGoods");
                    ProgKey.SetValue("login", SelectUser.UserName);
                    ProgKey.Close();
                    SoftKey.Close();

                    // если пользователь, то запускаем 
                    MainWindow win = new MainWindow();
                    win.Show();
                    App.Current.MainWindow = win;
                    winLogin.Close();
                }
                else
                    MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        //--------------------------------------------------------------------------------
        // Команда 
        //--------------------------------------------------------------------------------
        //private readonly ICommand _OkCommand = null;
        public ICommand CancelCommand => new LambdaCommand(OnCancelCommandExecuted, CanCancelCommand);
        private bool CanCancelCommand(object p) => true;
        private void OnCancelCommandExecuted(object p)
        {
            winLogin.Close();
        }
        #endregion

    }
}
