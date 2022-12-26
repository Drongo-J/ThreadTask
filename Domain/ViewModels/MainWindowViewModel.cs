using SP_Task2.Domain.Commands;
using SP_Task2.Domain.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SP_Task2.Domain.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public RelayCommand AddWordCommand { get; set; }
        public RelayCommand PlayCommand { get; set; }
        public RelayCommand StopCommand { get; set; }

        private string newWord = string.Empty;

        public string NewWord
        {
            get { return newWord; }
            set { newWord = value; OnPropertyChanged(); }
        }


        private ObservableCollection<string> words = new ObservableCollection<string>();

        public ObservableCollection<string> Words
        {
            get { return words; }
            set { words = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> encryptedWords = new ObservableCollection<string>();

        public ObservableCollection<string> EncryptedWords
        {
            get { return encryptedWords; }
            set { encryptedWords = value; OnPropertyChanged(); }
        }

        bool ThreadIsPlaying = false;

        public MainWindowViewModel()
        {
            Words.Add("Apple");
            Words.Add("Data");
            Words.Add("Pineapple");
            Words.Add("Animal");
            Words.Add("House");
            Words.Add("Bird");
            Words.Add("School");
            Words.Add("Student");

            Thread encryptWordsThread = null;

            AddWordCommand = new RelayCommand((a) =>
            {
                Words.Add(NewWord);
                NewWord = string.Empty;
            },
            (p) =>
            {
                if (NewWord.Trim().Length == 0)
                {
                    return false;
                }
                return true;
            });

            PlayCommand = new RelayCommand((p) =>
            {
                if (!ThreadIsPlaying)
                {
                    encryptWordsThread = new Thread(() =>
                    {
                        if (Words.Count == 0)
                        {
                            MessageBox.Show("Add words!");
                            return;
                        }

                        while (Words.Count != 0)
                        {
                            Thread.Sleep(700);
                            if (Words != null && Words.Count > 0)
                            {
                                List<string> enc_words2 = EncryptedWords.ToList();
                                List<string> words2 = Words.ToList();

                                string word = Words.Last();
                                var enc_word = word.Encrypt("random");

                                enc_words2.Add(enc_word);
                                words2.Remove(word);

                                Words = new ObservableCollection<string>(words2);
                                EncryptedWords = new ObservableCollection<string>(enc_words2);
                            }
                        }
                        MessageBox.Show("Words finished! Thread aborted!");
                        ThreadIsPlaying = false;
                    });
                    //encryptWordsThread.IsBackground = true;
                    ThreadIsPlaying = true;
                    encryptWordsThread.Start();
                }
            });

            StopCommand = new RelayCommand((s) =>
            {
                ThreadIsPlaying = false;
                encryptWordsThread.Abort();
            });
        }
    }
}
