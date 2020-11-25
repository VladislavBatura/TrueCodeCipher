using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TrueCodes
{
    class Cipher
    {
        string path = @"CipherWords.txt"; //Путь к словам
        string cipherPath = @"CipherKey.txt"; //Путь к ключу
        public List<string> cipherWords = new List<string>(); //Лист со словами
        public int cipherCode; //Ключ

        public Cipher()
        {
            try //Ловим ошибки через try-catch
            {
                
                if (File.Exists(cipherPath) && File.Exists(path)) //Проверяем на существование файлов
                {
                    using (StreamReader sr = new StreamReader(cipherPath)) //берём из файла ключ
                    {
                        cipherCode = int.Parse(sr.ReadLine());
                    }

                    using (StreamReader sr = new StreamReader(path)) //Для ещё большей защиты используем using
                    {
                        string line; //Строка для записи в лист
                        while ((line = sr.ReadLine()) != null) //цикл для записи из файла
                        {
                            cipherWords.Add(line);
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Файл с шифром либо отсутствует, либо его имя изменено. Пожалуйста, исправьте эту ошибку и перезапустите приложение, либо создайте новый шифр.");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void WriteNewCypher(int ind)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(cipherPath))
                {
                    sr.WriteLine(ind.ToString());
                    cipherCode = ind;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
