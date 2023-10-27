using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace MyLibWF
{
  public static class ActionsWF
  {
    const double fromTop = 30, fromLeft = 60;
    static Size size = new Size(40, 20);
    public static List<TextBox> textBoxes = new List<TextBox>();
    public static List<Label> labels = new List<Label>();

    static int Partition(int[] array, int minIndex, int maxIndex)
    {
      var pen = minIndex - 1;
      for (var i = minIndex; i < maxIndex; i++)
      {
        if (array[i] > array[maxIndex])
        {
          pen++;
          (array[pen], array[i]) = (array[i],array[pen]);
        }
      }

      pen++;
      (array[pen], array[maxIndex]) = (array[maxIndex], array[pen]);
      return pen;
    }

    public static int[] HoarahSort(int[] array, int indexLeft, int indexRight)
    {
      if (indexLeft >= indexRight)
      {
        return array; //финальная сдача массива
      }

      var pivotIndex = Partition(array, indexLeft, indexRight); // определение опорного элемента в массиве и перестановка
      HoarahSort(array, indexLeft, pivotIndex - 1); // сорт по краям от опорного элемента
      HoarahSort(array, pivotIndex + 1, indexRight); // сорт по краям от опорного элемента

      return array;
    }

    public static void FormInit(Form form)
    {
      form.ShowDialog();
      form.Dispose();
      textBoxes.Clear();
      labels.Clear();
    }

    public static void AddBox(double posx, double posy)
    {
      TextBox newTextBox = new TextBox();
      newTextBox.Location = new Point(40 + Convert.ToInt32(Math.Round(fromLeft * posx, 0)), 60 + Convert.ToInt32(Math.Round(fromTop * posy, 0)));
      newTextBox.Size = size;
      newTextBox.MaxLength = 5;
      newTextBox.TextAlign = HorizontalAlignment.Center;
      textBoxes.Add(newTextBox);
    }

    public static void AddLabel(double posx, double posy, int number)
    {
      Label newLabel = new Label();
      newLabel.Location = new Point(40 + Convert.ToInt32(Math.Round(fromLeft * posx, 0)), 60 + Convert.ToInt32(Math.Round(fromTop * posy, 0)));
      newLabel.Text = Convert.ToString(number);
      newLabel.TextAlign = ContentAlignment.MiddleCenter;
      newLabel.AutoSize = true;
      labels.Add(newLabel);
    }

    public static void Print(int[] x)
    {
      AddLabel(-0.5, 0, 0 + 1);
      for (int i = 0; i < x.Length; i++)
      {
        AddBox(i, 0);
        AddLabel(i, -0.8, i + 1);
      }
      TextToBoxes(x);
    }

    public static void Print(int[,] x)
    {
      for (int i = 0; i < x.GetLength(0); i++)
      {
        AddLabel(-0.5, i, i + 1); //принтит номера рядов
        for (int j = 0; j < x.GetLength(1); j++)
          AddBox(j, i);
      }
      for (int i = 0; i < x.GetLength(1); i++) //принтит номера столбцов
        AddLabel(i, -0.8, i + 1);
      TextToBoxes(x);
    }

    public static void Print(int[][] x)
    {
      int length = 0;
      for (int i = 0; i < x.Length; i++)
      {
        AddLabel(-0.5, i, i + 1);
        if (length < x[i].Length)
          length = x[i].Length;
        for (int j = 0; j < x[i].Length; j++)
          AddBox(j, i);
      }
      for (int i = 0; i < length; i++)
        AddLabel(i, -0.8, i + 1);
      TextToBoxes(x);
    }

    public static void TextToBoxes(int[] x)
    {
      int boxIndex = 0;
      for (int i = 0; i < x.Length; i++)
      {
        if (x[i] != 0)
          textBoxes[boxIndex].Text = Convert.ToString(x[i]);
        boxIndex++;
      }
    }

    public static void TextToBoxes(int[,] x)
    {
      int j, boxIndex = 0;
      for (int i = 0; i < x.GetLength(0); i++)
      {
        for (j = 0; j < x.GetLength(1); j++)
        {
          if (x[i, j] != 0)
            textBoxes[boxIndex].Text = Convert.ToString(x[i, j]);
          boxIndex++;
        }
      }
    }

    public static void TextToBoxes(int[][] x)
    {
      int boxIndex = 0;
      for (int i = 0; i < x.Length; i++)
      {
        for (int j = 0; j < x[i].Length; j++)
        {
          if (x[i][j] != 0)
            ActionsWF.textBoxes[boxIndex].Text = Convert.ToString(x[i][j]);
          boxIndex++;
        }
      }
    }

    public static int[] BoxesToArray(int[] x)
    {
      int boxIndex = 0;
      for (int i = 0; i < x.Length; i++)
      {
        if (x[i] != 0)
          textBoxes[boxIndex].Text = Convert.ToString(x[i]);
        boxIndex++;
      }
      return x;
    }

    public static int[,] BoxesToArray(int[,] x)
    {
      int j, boxIndex = 0, temp;
      for (int i = 0; i < x.GetLength(0); i++)
      {
        for (j = 0; j < x.GetLength(1); j++)
        {
          if (int.TryParse(textBoxes[boxIndex].Text, out temp))
            x[i, j] = temp;
          else
            x[i, j] = 0;
          boxIndex++;
        }
      }
      return x;
    }

    public static int[][] BoxesToArray(int[][] x)
    {
      int boxIndex = 0, temp;
      for (int i = 0; i < x.Length; i++)
      {
        for (int j = 0; j < x[i].Length; j++)
        {
          if (int.TryParse(textBoxes[boxIndex].Text, out temp))
            x[i][j] = temp;
          else
            x[i][j] = 0;
          boxIndex++;
        }
      }
      return x;
    }

    public static void BoxPrint(bool check, TextBox text, int[] x)
    {
      text.Text = "Одномерный массив:";
      if (check)
      {
        int temp = 0;
        do
        {
          text.Text += Environment.NewLine;
          if (x.Length % 10 != 0)
            switch (x.Length - temp > 10)
            {
              case (true):
                OneDimSubPrint(temp, 10, text, x);
                break;

              case (false):
                OneDimSubPrint(temp, x.Length - temp, text, x);
                break;
            }
          else
            OneDimSubPrint(temp, 10, text, x);
          temp += 10;
        }
        while (temp < x.Length);
      }
      else
        MessageBox.Show("Массив пока не инициализирован.", "Ошибка");
    }

    static void OneDimSubPrint(int temp, int x, TextBox text, int[] y)
    {
      for (int i = temp; i < temp + x; i++)
        text.Text += y[i] + " ";
    }

    public static void BoxPrint(bool check, TextBox text, int[,] x)
    {
      text.Text = "Двумерный массив:";
      if (check)
      {
        for (int i = 0; i < x.GetLength(0); i++)
        {
          text.Text += Environment.NewLine;
          for (int j = 0; j < x.GetLength(1); j++)
            text.Text += Convert.ToString(x[i, j]) + " ";
        }
      }
      else
        MessageBox.Show("Массив пока не инициализирован.", "Ошибка");
    }

    public static void BoxPrint(bool check, TextBox text, int[][] x)
    {
      text.Text = "Рваный массив:";
      if (check)
      {
        for (int i = 0; i < x.Length; i++)
        {
          text.Text += Environment.NewLine;
          for (int j = 0; j < x[i].Length; j++)
            text.Text += x[i][j] + " ";
        }
      }
      else
        MessageBox.Show("Массив не инициализирован.", "Ошибка");
    }

    public static int[] Task1(int[] x, ref bool check, TextBox text)
    {
      int i;
      for (i = 0; i < x.Length; i++)
        if (x[i] != 0)
          if (x[i] % 2 == 0)
          {
            x[i] = 0;
            for (i += 1; i < x.Length; i++)
              (x[i - 1], x[i]) = (x[i], x[i - 1]);
            if (x.Length == 1)
              check = false;
            Array.Resize(ref x, x.Length - 1);
            BoxPrint(check, text, x);
            break;
          }
      if (i == x.Length)
        MessageBox.Show("Чётных чисел не осталось.", "Предупрежедение");
      return x;
    }

    public static int[,] Task2(int[,] x, int lineNumber)
    {
      lineNumber -= 1;
      int index1 = x.GetLength(0), index2 = x.GetLength(1), z = 0;
      int[,] temp = new int[index1 + 1, index2];
      for (int i = 0; i < index1; i++) //i - строки, j - содержимое строк
      {
        if (z == lineNumber)
          z++;
        for (int j = 0; j < index2; j++)
        {
          temp[z, j] = x[i, j];
        }
        if (z == lineNumber)
          z++;
        z++;
      }
      return temp;
    }

    public static int[][] Task3(int[][] arrayMain, int idx)
    {
      int[][] arrayTemp = new int[arrayMain.Length - 1][];
      int k = 0;
      for (int i = 0; i < arrayMain.Length; i++)
      {
        if (i != idx)
          arrayTemp[k++] = arrayMain[i];
      }
      return arrayTemp;
    }

    public static string FileReader()
    {
      var fileContent = string.Empty;
      var filePath = string.Empty;

      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.InitialDirectory = "E:\\stuff\\Проекты VS\\Лабораторная работа №5\\Лабораторная работа №5\\bin\\Debug\\net7.0-windows";
        openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 2;
        openFileDialog.RestoreDirectory = true;

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
          filePath = openFileDialog.FileName;
          var fileStream = openFileDialog.OpenFile();
          using (StreamReader reader = new StreamReader(fileStream))
          {
            fileContent = reader.ReadToEnd();
          }
        }
      }
      return fileContent;
    }

    public static bool IsFileCorrect(string text, int[] x)
    {
      string[] arrStrings = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
      if (arrStrings.Length == 1)
        return true;
      return false;
    }

    public static bool IsFileCorrect(string text, int[,] x)
    {
      string[] arrStrings = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
      int length = 0, lastLength = 0;
      foreach (string s in arrStrings)
      {
        string[] temp = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        length = temp.Length;
        if (Array.IndexOf(arrStrings, s) == 0) //Чтобы в самом начале приравнять последнюю длину к длине
          lastLength = length;
        if (length != lastLength)
          return false;
      }
      if (length == lastLength)
        return true;
      return false;
    }

    public static bool IsFileCorrect(string text, int[][] x)
    {
      string[] arrStrings = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
      int length, lastLength = 0;
      foreach (string s in arrStrings)
      {
        string[] temp = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        length = temp.Length;
        if (Array.IndexOf(arrStrings, s) == 0) //Чтобы в самом начале приравнять последнюю длину к длине
          lastLength = length;
        if (length != lastLength)
          return true;
      }
      return false;
    }

    static void FileInit(string path)
    {
      if (!File.Exists(path))
      {
        var fie = File.Create(path);
        fie.Close();
      }
      else
        File.WriteAllText(path, string.Empty);
    }

    public static void Saver(int[] array)
    {
      string path = "One Dimensional Array.txt";
      FileInit(path);
      StreamWriter file = new StreamWriter(path);
      for (int i = 0; i < array.Length; i++)
      {
        if (i != array.Length - 1)
          file.Write(array[i] + " ");
        else
          file.Write(array[i]);
      }
      file.Close();
      MessageBox.Show("Массив записан");
    }

    public static void Saver(int[,] array)
    {
      string path = "Two Dimensional Array.txt";
      FileInit(path);
      StreamWriter file = new StreamWriter(path);
      for (int i = 0; i < array.GetLength(0); i++)
      {
        for (int j = 0; j < array.GetLength(1); j++)
          if (j != array.GetLength(1) - 1)
            file.Write(array[i, j] + " ");
          else
            file.WriteLine(array[i, j]);
      }
      file.Close();
      MessageBox.Show("Массив записан");
    }

    public static void Saver(int[][] array)
    {
      string path = "Torn Array.txt";
      FileInit(path);
      StreamWriter file = new StreamWriter(path);
      for (int i = 0; i < array.Length; i++)
      {
        for (int j = 0; j < array[i].Length; j++)
          if (j != array[i].Length - 1)
            file.Write(array[i][j] + " ");
          else
            file.WriteLine(array[i][j]);
      }
      file.Close();
      MessageBox.Show("Массив записан");
    }

    public static int Loader(string fileContent, ref int[] arrayMain)
    {
      int errorNumber = 0;
      string[] contentLines = fileContent.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      int[] arrayLocal = new int[contentLines.Length];
      for (int i = 0; i < contentLines.Length; i++)
      {
        int x;
        if (int.TryParse(contentLines[i], out x))
          arrayLocal[i] = x;
        else
          errorNumber++;
      }
      arrayMain = arrayLocal;
      return errorNumber;
    }

    public static int Loader(string fileContent, ref int[,] arrayMain)
    {
      int errorNumber = 0;
      string[] contentLines = fileContent.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
      string[] contentColumns = contentLines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      int[,] arrayLocal = new int[contentLines.Length, contentColumns.Length];

      for (int i = 0; i < contentLines.Length; i++)
      {
        contentColumns = contentLines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int j = 0; j < contentColumns.Length; j++)
        {
          int x;
          if (int.TryParse(contentColumns[j], out x))
            arrayLocal[i, j] = x;
          else
            errorNumber++;
        }
      }
      arrayMain = arrayLocal;
      return errorNumber;
    }

    public static int Loader(string fileContent, ref int[][] arrayMain)
    {
      int errorNumber = 0;
      string[] lineNumber = fileContent.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
      string[] columnNubmer;
      int[][] arrayLocal = new int[lineNumber.Length][];
      for (int i = 0; i < lineNumber.Length; i++)
      {
        columnNubmer = lineNumber[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        arrayLocal[i] = new int[columnNubmer.Length];
        for (int j = 0; j < arrayLocal[i].Length; j++)
        {
          int x;
          if (int.TryParse(columnNubmer[j], out x))
            arrayLocal[i][j] = x;
          else
            errorNumber++;
        }
      }
      arrayMain = arrayLocal;
      return errorNumber;
    }
  }
}