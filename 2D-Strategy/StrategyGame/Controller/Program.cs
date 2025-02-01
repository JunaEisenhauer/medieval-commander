using System;
using System.Collections.Generic;
using System.IO;
using ImageMagick;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using StrategyGame.Model;
using StrategyGame.Model.Game;
using StrategyGame.Model.IService;
using StrategyGame.Model.Menu;
using StrategyGame.Model.Menu.Level_Selection;
using StrategyGame.Service;
using IServiceProvider = StrategyGame.Model.IService.IServiceProvider;

namespace StrategyGame.Controller
{
    /// <summary>
    /// The entrypoint of the game. Handles the GameWindow. Instantiate scenes and changes them.
    /// </summary>
    public class Program
    {
        private readonly GameWindow gameWindow;
        private readonly Dictionary<string, IScene> scenes;
        private readonly IServiceProvider serviceProvider;
        private IScene currentScene;

        public Program()
        {
            gameWindow = new GameWindow()
            {
                WindowState = WindowState.Maximized,
                Title = "Medieval Commander",
            };

            scenes = new Dictionary<string, IScene>();

            serviceProvider = new ServiceProvider(gameWindow, scene =>
            {
                if (scene == null)
                {
                    gameWindow.Exit();
                    return;
                }

                IScene nextScene;
                if (scenes.ContainsKey(scene))
                {
                    nextScene = scenes[scene];
                }
                else
                {
                    nextScene = new GameScene(serviceProvider, scene);
                }

                nextScene.OnChange();

                // Reset OpenGL Viewport for next scene
                GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
                GL.LoadIdentity();
                if (gameWindow.Height > gameWindow.Width)
                {
                    GL.Scale(1f, (float)gameWindow.Width / gameWindow.Height, 1f);
                }
                else
                {
                    GL.Scale((float)gameWindow.Height / gameWindow.Width, 1f, 1f);
                }

                nextScene.Resize(gameWindow);

                currentScene = nextScene;
            });

            StartMenuScene startMenuScene = new StartMenuScene(serviceProvider);
            GameOverScene gameOverScene = new GameOverScene(serviceProvider);
            LevelSelection levelSelection = new LevelSelection(serviceProvider);
            scenes.Add("MainMenu", startMenuScene);
            scenes.Add("GameOver", gameOverScene);
            scenes.Add("LevelSelection", levelSelection);
            currentScene = startMenuScene;

            gameWindow.UpdateFrame += (s, e) => currentScene.Update();

            gameWindow.Resize += (s, e) =>
            {
                GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
                GL.LoadIdentity();
                if (gameWindow.Height > gameWindow.Width)
                {
                    GL.Scale(1f, (float)gameWindow.Width / gameWindow.Height, 1f);
                }
                else
                {
                    GL.Scale((float)gameWindow.Height / gameWindow.Width, 1f, 1f);
                }

                currentScene.Resize(gameWindow);
            };

            gameWindow.UpdateFrame += (s, e) => serviceProvider.Update();

            Stream cursorImage = serviceProvider.GetService<IFileResourceService>().Open("cursor_shiny.png");
            using (var image = new MagickImage(cursorImage))
            {
                gameWindow.Cursor = new MouseCursor(0, 0, 32, 32, image.GetPixelsUnsafe().ToArray());
            }

            gameWindow.Run();
        }

        public static void Main(string[] args)
        {
            new Program();
        }
    }
}
