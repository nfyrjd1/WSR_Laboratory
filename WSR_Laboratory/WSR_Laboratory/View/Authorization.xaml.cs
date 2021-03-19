﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WSR_Laboratory.Model;
using WSR_Laboratory.Service;
using WSR_Laboratory.View;

namespace WSR_Laboratory.View
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            Manager.Auth = this;
        }

        private void Auth_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            string Password = PasswordBox.Visibility == Visibility.Visible ? PasswordBox.Password : PasswordTB.Text;

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text)) {
                errors.AppendLine("Вы не ввели логин пользователя!");
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                errors.AppendLine("Вы не ввели пароль пользователя!");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string Login = LoginTextBox.Text;
            user user = Laboratory.GetContext().user.Where(p => p.login == Login).FirstOrDefault();
            if (user == null)
            {
                history historyError = new history();
                historyError.login = Login;
                historyError.has_entered = false;
                historyError.date_time = DateTime.Now;
                Laboratory.GetContext().history.Add(historyError);
                Laboratory.GetContext().SaveChanges();
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            history history = new history();
            history.login = Login;
            history.user = user;
            history.has_entered = user.password == Password;
            history.date_time = DateTime.Now;
            Laboratory.GetContext().history.Add(history);
            Laboratory.GetContext().SaveChanges();

            if (history.has_entered)
            {
                Manager.CurrentUser = user;
                MainWindow window = new MainWindow();
                window.Show();
                Hide();

                PasswordBox.Password = "";
                PasswordTB.Text = "";
                LoginTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Visibility == Visibility.Visible)
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTB.Visibility = Visibility.Visible;
                PasswordTB.Text = PasswordBox.Password;
            } else
            {
                PasswordBox.Visibility = Visibility.Visible;
                PasswordTB.Visibility = Visibility.Collapsed;
                PasswordBox.Password = PasswordTB.Text;
            }
        }

        private void ImportBloodService()
        {
            List<string> data = File.ReadLines(@"F:\ЯндексДиск\работяга\Четвёртый курс\Второй семак\WSR\WSR_Laboratory\ImportData\blood_service.txt").ToList();
            data.ForEach((item) =>
            {
                string[] itemData = item.Split('\t');

                string AnalyzerName = itemData[6];
                analyzer analyzer = Laboratory.GetContext().analyzer.Where(p => p.name == AnalyzerName).FirstOrDefault();
                if (analyzer == null)
                {
                    analyzer = new analyzer();
                    analyzer.name = AnalyzerName;
                    Laboratory.GetContext().analyzer.Add(analyzer);
                    Laboratory.GetContext().SaveChanges();
                }

                blood_service blood_service = new blood_service();
                decimal service_code = decimal.Parse(itemData[1]);
                blood_service.service = Laboratory.GetContext().service.Where(p => p.code == service_code).FirstOrDefault();
                int IdBlood = int.Parse(itemData[0]);
                blood blood = Laboratory.GetContext().blood.Where(p => p.id_blood == IdBlood).FirstOrDefault();
                if (blood == null)
                {
                    return;
                }
                blood_service.blood = blood;
                blood_service.result = decimal.Parse(itemData[2]);
                blood_service.date_finished = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(itemData[3])).DateTime;
                blood_service.accepted = bool.Parse(itemData[4]);
                blood_service.id_status = itemData[5] == "Finished" ? 1 : 2;
                int UserId = int.Parse(itemData[7]);
                blood_service.employee = Laboratory.GetContext().employee.Where(p => p.id_user == UserId).FirstOrDefault();
                Laboratory.GetContext().blood_service.Add(blood_service);

                analyzer_blood_service analyzer_Blood = new analyzer_blood_service();
                analyzer_Blood.analyzer = analyzer;
                analyzer_Blood.blood_service = blood_service;
                analyzer_Blood.date_reception = blood.date_create;
                analyzer_Blood.date_finished = blood_service.date_finished;
                Laboratory.GetContext().analyzer_blood_service.Add(analyzer_Blood);
            });

            Laboratory.GetContext().SaveChanges();
        }

        private void ImportBlood()
        {
            List<string> data = File.ReadLines(@"F:\ЯндексДиск\работяга\Четвёртый курс\Второй семак\WSR\WSR_Laboratory\ImportData\blood.txt").ToList();
            data.ForEach((item) =>
            {
                string[] itemData = item.Split('\t');
                blood blood = new blood();
                blood.id_patient = int.Parse(itemData[1]);
                blood.barcode = decimal.Parse(itemData[2]);
                blood.date_create = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(itemData[3].Replace("-", ""))).DateTime;
                blood.id_status = 1;
                Laboratory.GetContext().blood.Add(blood);
            });

            Laboratory.GetContext().SaveChanges();
        }

        private void ImportPatients()
        {
            List<string> data = File.ReadLines(@"F:\ЯндексДиск\работяга\Четвёртый курс\Второй семак\WSR\WSR_Laboratory\ImportData\patients.txt").ToList();
            data.ForEach((item) =>
            {
                string[] itemData = item.Split('\t');

                string InsuranceName = itemData[14];
                string InsuranceAddress = itemData[15];
                insurance insurance = Laboratory.GetContext().insurance.Where(p => p.name == InsuranceName && p.address == InsuranceAddress).FirstOrDefault();
                if (insurance == null)
                {
                    insurance = new insurance();
                    insurance.name = itemData[14];
                    insurance.address = itemData[15];
                    insurance.inn = decimal.Parse(itemData[16]);
                    insurance.payment_account = decimal.Parse(itemData[18]);
                    insurance.bik = decimal.Parse(itemData[19]);
                    Laboratory.GetContext().insurance.Add(insurance);
                    Laboratory.GetContext().SaveChanges();
                }

                user user = new user();
                user.login = itemData[1];
                user.password = itemData[2];
                user.id_user_type = 5;
                Laboratory.GetContext().user.Add(user);

                patient patient = new patient();
                patient.full_name = itemData[0];
                patient.email = itemData[4];
                patient.social_number = decimal.Parse(itemData[5]);
                patient.ein = itemData[6];
                patient.social_type = itemData[7] == "oms";
                patient.phone = itemData[8];
                patient.passport_series = decimal.Parse(itemData[9]);
                patient.passport_number = decimal.Parse(itemData[10]);
                patient.birthday = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(itemData[11].Replace("-", ""))).DateTime;
                patient.country = itemData[13];
                patient.user = user;
                patient.insurance = insurance;
                Laboratory.GetContext().patient.Add(patient);

                history history = new history();
                history.user = user;
                history.ip = itemData[17];
                history.login = user.login;
                history.has_entered = true;
                history.date_time = DateTime.Now;
                Laboratory.GetContext().history.Add(history);
            });

            Laboratory.GetContext().SaveChanges();
        }

        private void ImportEmployee()
        {
            List<string> data = File.ReadLines(@"F:\ЯндексДиск\работяга\Четвёртый курс\Второй семак\WSR\WSR_Laboratory\ImportData\users.txt").ToList();
            data.ForEach((item) =>
            {
                string[] itemData = item.Split('\t');
                user user = new user();
                user.login = itemData[2];
                user.password = itemData[3];
                user.id_user_type = int.Parse(itemData[7]);
                Laboratory.GetContext().user.Add(user);

                employee employee = new employee();
                employee.full_name = itemData[1];
                employee.user = user;
                Laboratory.GetContext().employee.Add(employee);

                history history = new history();
                history.user = user;
                history.ip = itemData[4];
                history.has_entered = true;
                history.login = user.login;
                string[] date = itemData[5].Split('.');
                history.date_time = DateTime.Parse($"{date[1]}.{date[0]}.{date[2]}");
                Laboratory.GetContext().history.Add(history);

                string[] services = itemData[6].Replace("\"", "").Split(';');
                if (services.Length == 0)
                {
                    services = new string[] { itemData[6].Replace("\"", "") };
                }

                foreach (string serviceCode in services)
                {
                    decimal code = decimal.Parse(serviceCode);
                    service service = Laboratory.GetContext().service.Where(p => p.code == code).FirstOrDefault();
                    if (service == null)
                    {
                        continue;
                    }
                    employee_service employee_service = new employee_service();
                    employee_service.employee = employee;
                    employee_service.service = service;
                    Laboratory.GetContext().employee_service.Add(employee_service);
                }
            });

            Laboratory.GetContext().SaveChanges();
        }
    }
}
