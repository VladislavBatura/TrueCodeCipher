using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;

namespace TrueCodes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Инициализируем класс шифра и поток для создания шифра. К потоку прикрепляем функцию смены шифра
        Cipher cipher = new Cipher();

        Thread thread = new Thread(ChangeCypher);


        private void button1_Click(object sender, EventArgs e)
        {
            //По клику на кнопку шифровки, идёт проверка сообщения. Если его длина меньше двух букв, то программа даст понять пользователю, что сообщения нет
            if (textBox2.Text.Length < 2)
            {
                MessageBox.Show("Введите что-нибудь для начала");
            }
            else //Если же сообщение есть, то начинаем процесс шифровки
            {
                CipherMessage(textBox2.Text.ToLower());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void CipherMessage(string text)
        {   //Сначала отсеиваем знаки препинания из текста
            char[] ch = new char[] { '.', ',' }; //хранилище знаков
            int index;

            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //Делим текст на слова по пробелам

            for(int i = 0; i < words.Length; i++) //Прогоняемся по словам и удаляем знаки препинания
            {
                for(int j = 0; j < ch.Length; j++)
                {
                    index = words[i].IndexOf(ch[j]);
                    if(index != -1)
                    {
                        words[i] = words[i].Remove(index, 1);
                    }
                }
            }

            ArrayList wordsAfterCipher = new ArrayList(); 

            int code = cipher.cipherCode / 2 + 21; //ключ для шифровки и дешифровки. Считается по этой формуле.
            int promCode; //Промежуточная переменная

            foreach(string word in words) //Перебираем слова в тексте
            {
                if(cipher.cipherWords.Contains(word)) //Если слово содержится в заготовленном словаре, то идёт подбор нового слова
                {
                    if(cipher.cipherWords.IndexOf(word) + code >= cipher.cipherWords.Count) //Делаем проверку на выход за границы массива
                    {
                        //если такое происходит, то отсчитываем с начала словаря нужное количество слов
                        promCode = cipher.cipherWords.IndexOf(word) + code;
                        promCode -= cipher.cipherWords.Count;
                        wordsAfterCipher.Add(cipher.cipherWords[promCode]);
                    }
                    else //если нет, то просто ищем слово по индексу + ключ
                    {
                        wordsAfterCipher.Add(cipher.cipherWords[cipher.cipherWords.IndexOf(word) + code]);
                    }
                }
                else //если слова нет в словаре, то оставляем его и добавляем в поле после шифровки. Я думаю, что злоумышленник всё равно не сможет понять смысл сообщения по отдельным словам.
                {
                    wordsAfterCipher.Add(word);
                }
            }

            textBox1.Text = ""; //обнуляем поле для шифровки

            foreach(string word in wordsAfterCipher) // Прогоняемся по составленному тексту и записываем в текстовое поле
            {
                textBox1.Text += word + ' ';
            }
        }

        void Decypher(string text) //Функция для дешифровки. Процесс тот же, что и для шифровки, только инвертирован.
        {
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); 
            ArrayList wordsAfterDecipher = new ArrayList();

            int code = cipher.cipherCode / 2 + 21;
            int promCode;

            foreach (string word in words)
            {
                if (cipher.cipherWords.Contains(word))
                {
                    if (cipher.cipherWords.IndexOf(word) - code < 0)
                    {
                        promCode = code - cipher.cipherWords.IndexOf(word);

                        promCode = cipher.cipherWords.Count - promCode;
                        wordsAfterDecipher.Add(cipher.cipherWords[promCode]);
                    }
                    else
                    {
                        wordsAfterDecipher.Add(cipher.cipherWords[cipher.cipherWords.IndexOf(word) - code]);
                    }
                }
                else
                {
                    wordsAfterDecipher.Add(word);
                }
            }

            textBox2.Text = "";

            foreach (string word in wordsAfterDecipher)
            {
                textBox2.Text += word + ' ';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Decypher(textBox1.Text.ToLower()); //Кнопка дешифровки
        }

        private void ChangeCipher_Click(object sender, EventArgs e)
        {
            thread.Start(); //По клику на кнопку "Смена шифра", запускаем поток для смены шифра.
        }

        static void ChangeCypher() //Функция для смены шифра.
        {
            int index; //Индекс для рандомного подбора ключа
            var rand = new Random(); 
            Cipher cyp = new Cipher(); //Новый экземпляр класса. Не совсем понимаю, почему оно так, но это точно связано со static.
            index = rand.Next(0, 39337);

            cyp.WriteNewCypher(index); //вызываем функцию для записи шифра.


            MessageBox.Show("Мы поменяли шифр и записали его в файл");
        }
    }
}
