using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using StrategyGame.Model;
using StrategyGame.Model.Game;
using StrategyGame.Model.Menu;
using StrategyGame.View;

namespace StrategyGame.Controller
{
    /// <summary>
    /// Controller of the game manages key inputs, screenchange and view
    /// </summary>
    public class MyController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyController"/> class.
        /// </summary>
        public MyController()
        {
            GameWindow window = new GameWindow()
            {
                WindowState = WindowState.Maximized
            };

            IKeyboard keyboard = new KeyState(window);

            Dictionary<SceneType, IModel> models = new Dictionary<SceneType, IModel>
            {
                { SceneType.Menu, new StartMenuModel() }, { SceneType.Game, new GameModel() }
            };
            IModel currentModel = models[SceneType.Menu];
            MyView view = new MyView();

            window.UpdateFrame += (s, e) =>
            {
                currentModel.Update(keyboard, (float)e.Time, (sceneType) =>
                {
                    foreach (IModel model in models.Values)
                    {
                        model.SceneChangedEvent(sceneType);
                    }

                    if (sceneType == SceneType.Exit)
                    {
                        window.Exit();
                        return;
                    }
                    currentModel = models[sceneType];
                });
                keyboard.Update();
            };
            window.RenderFrame += (s, e) =>
            {
                view.Render(currentModel.Entities, currentModel.StartPosition, keyboard.MousePosition);
                window.SwapBuffers();
            };
            window.Resize += (s, e) =>
            {
                GL.Viewport(0, 0, window.Width, window.Height);
                GL.LoadIdentity();
                GL.Scale((float)window.Height / window.Width, 1f, 1f);
            };
            window.Run();
        }
    }
}
