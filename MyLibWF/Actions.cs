using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace MyLibWF
{
  public static class ActionsWF
  {
    public const double fromTop = 30, fromLeft = 60;
    public static int tabindex = 8;
    public static Size size = new Size(40, 20);
    public static List<TextBox> textBoxes = new List<TextBox>();
    public static List<Label> labels = new List<Label>();

    public static void AddBox(double posx, double posy)
    {
      TextBox newTextBox = new TextBox();
      newTextBox.Location = new Point(40 + Convert.ToInt32(Math.Round(fromLeft * posx, 0)), 60 + Convert.ToInt32(Math.Round(fromTop * posy, 0)));
      newTextBox.Size = size;
      newTextBox.TabIndex = tabindex;
      tabindex++;
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
      TtB(x);
      tabindex = 8;
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
      TtB(x);
      tabindex = 8;
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
      TtB(x);
      tabindex = 8;
    }

    public static void TtB(int[] x)
    {
      int j, boxIndex = 0, temp;
      for (int i = 0; i < x.Length; i++)
      {
        if (x[i] != 0)
          textBoxes[boxIndex].Text = Convert.ToString(x[i]);
        boxIndex++;
      }
    }

    public static void TtB(int[,] x)
    {
      int j, boxIndex = 0, temp;
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

    public static void TtB(int[][] x)
    {
      int boxIndex = 0, temp;
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
    public static int[] BtA(int[] x)
    {
      int j, boxIndex = 0, temp;
      for (int i = 0; i < x.Length; i++)
      {
        if (x[i] != 0)
          textBoxes[boxIndex].Text = Convert.ToString(x[i]);
        boxIndex++;
      }
      return x;
    }

    public static int[,] BtA(int[,] x)
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
    public static int[][] BtA(int[][] x)
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

    private static void OneDimSubPrint(int temp, int x, TextBox text, int[] y)
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
  }
}