using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tesr;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        _TimerGame = new();
        _TimerGame.Tick += MainGame;
        _TimerGame.Interval = TimeSpan.FromMilliseconds(20);
        GameRun();
    }

    private void MainGame(object? sender, EventArgs e)
    {
        ScoreLabel.Content = $"Tu puntuación es: {Score}";

        var playerTop = Canvas.GetTop(Player);
        var playerLeft = Canvas.GetLeft(Player);

        if (playerTop > 400 || playerTop < 0)
        {
            EndGame();
        }

        _PlayerBox = new(playerLeft, playerTop, Player.Width, Player.Height);

        Canvas.SetTop(Player, playerTop + _Gravedad);

        foreach (var item in GameLayout.Children.OfType<Image>())
        {
            if ((string)item.Tag == "Ob1" || (string)item.Tag == "Ob2" || (string)item.Tag == "Ob3")
            {
                Canvas.SetLeft(item, Canvas.GetLeft(item) - 5);

                if (Canvas.GetLeft(item) < -100)
                {
                    Canvas.SetLeft(item, 800);

                    Score += 5;
                }

                Rect BoxCollision = new(Canvas.GetLeft(item), Canvas.GetTop(item), item.Width, item.Height);

                if (_PlayerBox.IntersectsWith(BoxCollision))
                {
                    EndGame();
                }
            }
        }
    }

    private void EndGame()
    {
        _TimerGame.Stop();

        ScoreLabel.Content = $"Fin del juego tu puntuación fue de: {Score} \n Presiono (R) para volver a itentarlo";
    }

    private void GameLayout_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Space)
        {
            Player.RenderTransform = new RotateTransform(-20, Player.Width / 2, Player.Height / 2);
            _Gravedad = -8;
        }
        if(e.Key == System.Windows.Input.Key.R)
        {
            GameRun();
        }
    }

    private void GameLayout_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        Player.RenderTransform = new RotateTransform(5, Player.Width / 2, Player.Height / 2);
        _Gravedad = 8;
    }

    private void GameRun()
    {
        GameLayout.Focus();

        Score = 0;

        Canvas.SetTop(Player, 200);

        foreach (var item in GameLayout.Children.OfType<Image>())
        {
            if ((string)item.Tag == "Ob1")
            {
                Canvas.SetLeft(item, 500);
            }
            else if ((string)item.Tag == "Ob2")
            {
                Canvas.SetLeft(item, 800);
            }
            else if ((string)item.Tag == "Ob3")
            {
                Canvas.SetLeft(item, 1100);
            }
        }

        _TimerGame.Start();
    }


    private Rect _PlayerBox { get; set; }
    private int _Gravedad = -8;
    public int Score { get; set; }
    private readonly DispatcherTimer _TimerGame;
}
