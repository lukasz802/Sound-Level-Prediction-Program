using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Compute_Engine;

namespace Sound_Level_Prediction_Program.ViewModels
{
    public class CalcdBViewModel : INotifyPropertyChanged
    {
        private string result = "0";
        private string equation = string.Empty;
        private List<string> tempList = new List<string>();
        private ObservableCollection<KeyValuePair<string,string>> historyList = new ObservableCollection<KeyValuePair<string, string>>();
        private string lastClicked;

        public ICommand FunctionalClickCommand { get; private set; }
        public ICommand CharClickCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand ClearEntryCommand { get; private set; }
        public ICommand InsertCommaCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand ResultCommand { get; private set; }
        public ICommand ChangeValueCommand { get; private set; }

        public CalcdBViewModel()
        {
            this.ClearCommand = new RelayCommand((o) => Clear());
            this.ClearEntryCommand = new RelayCommand((o) => ClearEntry());
            this.CharClickCommand = new RelayCommand(execute: (o) => CharClick(o));
            this.FunctionalClickCommand = new RelayCommand((o) => FunctionalButtonClick(o));
            this.InsertCommaCommand = new RelayCommand((o) => InsertComma(o));
            this.RemoveCommand = new RelayCommand((o) => Remove());
            this.ResultCommand = new RelayCommand((o) => EqualResult(o));
            this.ChangeValueCommand = new RelayCommand((o) => ChangeValue());
        }

        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }

        public string Equation
        {
            get
            {
                return equation;
            }
            set
            {
                equation = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<KeyValuePair<string, string>> ResultsList
        {
            get { return historyList; }
        }

        private void ChangeValue()
        {
            this.Result = (- Convert.ToDouble(this.Result)).ToString();
        }

        private void EqualResult(object obj)
        {
            if (lastClicked == null) { lastClicked = "0"; }
            string loc = lastClicked;
            FunctionalButtonClick(obj);
            Regex regex = new Regex(@"dB");
            tempList[tempList.Count - 1] = regex.Match(tempList[tempList.Count - 1]).ToString();

            if (tempList.Count >= 1 && !Regex.IsMatch(loc, @"[0-9r=]"))
            {
                tempList.Insert(tempList.Count, tempList[tempList.Count - 2]);
                if (loc.Contains("×"))
                {
                    tempList[tempList.Count - 2] += " × ";
                    tempList.Add(" =");
                }
                else if (loc.Contains("÷"))
                {
                    tempList[tempList.Count - 2] += " ÷ ";
                    tempList.Add(" =");
                }
                else if (loc.Contains("+"))
                {
                    tempList[tempList.Count - 2] += " + ";
                    tempList.Add("dB =");
                }
                else if (loc.Contains("-"))
                {
                    tempList[tempList.Count - 2] += " - ";
                    tempList.Add("dB =");
                }
            }
            else
            {
                tempList[tempList.Count - 1] += " =";
            }

            string result = string.Empty;
            foreach (string element in tempList)
            {
                result += element;
            }

            this.Result = Math.Round(MathOperation.CalcResult(tempList), 7).ToString();
            historyList.Insert(0, new KeyValuePair<string, string>(this.Result + "dB", result));
            if (historyList.Count > 10)
            {
                historyList.RemoveAt(historyList.Count - 1);
            }

            tempList.Clear();
            this.Equation = string.Empty;
        }

        private void InsertComma(object obj)
        {
            string var = obj as string;
            if (!this.Result.Contains(",")) { this.Result += var; }
        }

        private void Remove()
        {
            if (this.Result != "0" && this.Result.Length > 1) { this.Result = this.Result.Remove(this.Result.Length - 1); }
            else { this.Result = "0"; }

            lastClicked = "r";
        } 

        private void CharClick(object obj)
        {
            Regex regex = new Regex(@"[0-9]");
            if (lastClicked != null && regex.IsMatch(lastClicked) == false) { this.Result = string.Empty; }

            string var = obj as string;
            lastClicked = obj as string;

            if (this.Result != "0" && this.Result.Length <= 8)
            {
                this.Result += var;
            }
            else if (this.Result.Length == 1)
            {
                this.Result = this.Result.Replace("0", var);
            }
        }

        private void Clear()
        {
            this.Result = "0";
            this.Equation = string.Empty;
            tempList.Clear();
        }

        private void ClearEntry()
        {
            this.Result = "0";
            lastClicked = "r";
        }

        private void FunctionalButtonClick(object obj)
        {
            ClearField();
            string var = obj as string;
            lastClicked = var;
            tempList.Add(this.Result = Convert.ToDouble(this.Result).ToString());
            if (tempList.Count >= 2 && (tempList[tempList.Count - 2].Contains("×") || tempList[tempList.Count - 2].Contains("÷")))
            {
                tempList.Add($" {var} ");
            }
            else
            {
                tempList.Add($"dB {var} ");
            }
            FillField();
        }

        private void ClearField()
        {
            Regex regex = new Regex(@"[0-9r=]");
            if (this.Equation != string.Empty && !regex.IsMatch(lastClicked))
            {
                tempList.RemoveRange(tempList.Count - 2, 2);
            }
        }

        private void FillField()
        {
            if (this.Equation.Count() != 0) { this.Equation = string.Empty; }
            foreach (string item in tempList)
            {
                this.Equation += item;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
