using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using ImageMagick;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using StrategyGame.Model;
using StrategyGame.Model.Game;
using StrategyGame.Model.Menu;

namespace StrategyGame.View
{
    /// <summary>
    /// View paints the elements of the game
    /// </summary>
    public class MyView
    {
        private readonly int fontImageData;
        private int textureWidth;
        private int textureHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyView"/> class.
        /// </summary>
        public MyView()
        {
            GL.ClearColor(Color4.CornflowerBlue);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);

            fontImageData = LoadTexture();
        }

        /// <summary>
        /// Renders a frame
        /// </summary>
        /// <param name="entities">The entities to render</param>
        /// <param name="mousePositionStart">position where the mouse drag started</param>
        /// <param name="mousePosition">position where the mouse currently is</param>
        public void Render(List<IEntity> entities, Vector2? mousePositionStart, Vector2 mousePosition)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            RenderMap();

            entities.ForEach(entity => this.RenderEntity(entity));

            if (mousePositionStart.HasValue)
                RenderQuad(new List<Vector2> { mousePositionStart.Value, new Vector2(mousePositionStart.Value.X, mousePosition.Y), mousePosition, new Vector2(mousePosition.X, mousePositionStart.Value.Y) }, Color.FromArgb(140, 199, 199, 199));
        }

        /// <summary>
        /// Renders a single entity of the frame
        /// </summary>
        /// <param name="entity">The entity to render</param>
        private void RenderEntity(IEntity entity)
        {
            Color color;
            switch (entity.Id)
            {
                case EntityType.MenuStart:
                    color = Color.GreenYellow;
                    break;
                case EntityType.MenuExit:
                    color = Color.OrangeRed;
                    break;
                case EntityType.ArcherPlayer:
                    color = Color.Blue;
                    break;
                case EntityType.KnightPlayer:
                    color = Color.Blue;
                    break;
                case EntityType.CavalryPlayer:
                    color = Color.Blue;
                    break;
                case EntityType.TowerPlayer:
                    color = Color.DarkBlue;
                    break;
                case EntityType.ArcherEnemy:
                    color = Color.Red;
                    break;
                case EntityType.KnightEnemy:
                    color = Color.Red;
                    break;
                case EntityType.CavalryEnemy:
                    color = Color.Red;
                    break;
                case EntityType.TowerEnemy:
                    color = Color.DarkRed;
                    break;
                case EntityType.Projectile:
                    color = Color.Black;
                    break;
                default:
                    color = Color.Yellow;
                    break;
            }

            switch (entity)
            {
                case IGameObject gameObject:
                    if (!gameObject.IsDead)
                    {
                        if (gameObject is Tower)
                        {
                            if (gameObject.Selected)
                                SelectObject(gameObject);
                            RenderTower(gameObject, color);
                        }
                        else
                        {
                            if (gameObject.Selected)
                                SelectObject(gameObject);
                            RenderArmy(gameObject, color);
                        }

                        if (gameObject.HealthChange)
                        {
                            if (gameObject is IArmy)
                            {
                                if (gameObject.IsAttack)
                                {
                                    RenderHealthDisplay(gameObject, gameObject.BarState);
                                    RenderDamageArmy(gameObject, gameObject.DamageAnimation);
                                }
                                else if (gameObject.IsHealing)
                                {
                                    RenderHealthDisplay(gameObject, gameObject.BarState);
                                }
                            }
                            else if (gameObject is Tower)
                            {
                                if (gameObject.IsAttack)
                                {
                                    RenderHealthDisplay(gameObject, gameObject.BarState);
                                    RenderDamageTower(gameObject, gameObject.DamageAnimation);
                                }
                            }
                        }
                    }

                    if (gameObject.IsDead)
                        RenderDead(gameObject);

                    switch (gameObject)
                    {
                        case Archer archer:
                            RenderArcherSymbol(gameObject);
                            break;
                        case Knight knight:
                            RenderKnightSymbol(gameObject);
                            break;
                        case Cavalry cavalry:
                            RenderCavalrySymbol(gameObject);
                            break;
                    }

                    break;
                case MenuButton button:
                    RenderButton(button, color);
                    break;
                case Projectile projectile:
                    GL.Color3(color);
                    GL.Begin(PrimitiveType.Quads);

                    GL.Vertex2(projectile.Position + new Vector2(-0.01f, -0.01f));
                    GL.Vertex2(projectile.Position + new Vector2(0.01f, -0.01f));
                    GL.Vertex2(projectile.Position + new Vector2(0.01f, 0.01f));
                    GL.Vertex2(projectile.Position + new Vector2(-0.01f, 0.01f));

                    GL.End();
                    break;
            }
        }

        private void RenderTower(IGameObject entity, Color color)
        {
            float x = entity.Position.X + entity.Offset.X;
            float y = entity.Position.Y + entity.Offset.Y;
            float r = entity.Radius;
            float height = 0.6f;
            float thickness = 0.7f;

            RenderQuad(new List<Vector2>() { new Vector2(x - (r * thickness), y - r), new Vector2(x - (r * thickness), y + (r * height)), new Vector2(x + (r * thickness), y + (r * height)), new Vector2(x + (r * thickness), y - r) }, color);
            RenderQuad(new List<Vector2>() { new Vector2(x - (r * thickness), y), new Vector2(x - (r * thickness), y + r), new Vector2(x - (r * 0.4f), y + r), new Vector2(x, y) }, color);
            RenderQuad(new List<Vector2>() { new Vector2(x + (r * thickness), y), new Vector2(x + (r * thickness), y + r), new Vector2(x + (r * 0.4f), y + r), new Vector2(x, y) }, color);
        }

        private void RenderArmy(IGameObject entity, Color color)
        {
            GL.Color3(color);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(entity.Position.X + entity.Offset.X, entity.Position.Y + entity.Offset.Y);

            const int corners = 16;
            const float delta = 2f * (float)Math.PI / corners;
            for (int i = 0; i < corners; i++)
            {
                float alpha = i * delta;
                float x = entity.Radius * (float)Math.Cos(alpha);
                float y = entity.Radius * (float)Math.Sin(alpha);
                GL.Vertex2(entity.Position.X + entity.Offset.X + x, entity.Position.Y + entity.Offset.Y + y);
            }

            GL.Vertex2(entity.Position.X + entity.Offset.X + entity.Radius, entity.Position.Y + entity.Offset.Y);
            GL.End();
        }

        private void RenderDead(IGameObject entity)
        {
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(entity.Position + entity.Offset + new Vector2(-entity.Radius + 0.03f, -entity.Radius + 0.03f));
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(entity.Radius - 0.03f, -entity.Radius + 0.03f));
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(entity.Radius - 0.03f, entity.Radius - 0.03f));
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(-entity.Radius + 0.03f, entity.Radius - 0.03f));

            GL.End();
        }

        private void RenderArcherSymbol(IGameObject entity)
        {
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Points);

            const int corners = 128;
            const float delta = 2f * (float)Math.PI / corners;
            for (int i = (int)(-corners * 0.06f); i < (corners / 2) + (corners * 0.06f); i++)
            {
                float alpha = i * delta;
                float x = entity.Radius * 0.6f * (float)Math.Cos(alpha);
                float y = entity.Radius * 0.6f * (float)Math.Sin(alpha);
                GL.Vertex2(entity.Position.X + entity.Offset.X + y, entity.Position.Y + entity.Offset.Y + x);
            }

            GL.End();

            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex2(entity.Position.X + entity.Offset.X, entity.Position.Y + entity.Offset.Y + (entity.Radius * 0.55f));
            GL.Vertex2(entity.Position.X + entity.Offset.X, entity.Position.Y + entity.Offset.Y - (entity.Radius * 0.55f));
            GL.End();

            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex2(entity.Position.X + entity.Offset.X + (entity.Radius * 0.75f), entity.Position.Y + entity.Offset.Y);
            GL.Vertex2(entity.Position.X + entity.Offset.X - (entity.Radius * 0.2f), entity.Position.Y + entity.Offset.Y);
            GL.End();
        }

        private void RenderKnightSymbol(IGameObject entity)
        {
            float x = entity.Position.X + entity.Offset.X;
            float y = entity.Position.Y + entity.Offset.Y;
            float r = entity.Radius;

            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x - (r * 0.45f), y - (r * 0.65f));
            GL.Vertex2(x - (r * 0.65f), y - (r * 0.45f));
            GL.Vertex2(x - (r * 0.35f), y - (r * 0.15f));
            GL.Vertex2(x - (r * 0.15f), y - (r * 0.35f));
            GL.End();

            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex2(x - (r * 0.35f), y - (r * 0.15f));
            GL.Vertex2(x - (r * 0.15f), y - (r * 0.35f));
            GL.Vertex2(x + (r * 0.5f), y + (r * 0.5f));
            GL.End();

            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x - (r * 0.65f), y - (r * 0.05f));
            GL.Vertex2(x - (r * 0.55f), y - (r * -0.05f));
            GL.Vertex2(x - (r * -0.05f), y - (r * 0.55f));
            GL.Vertex2(x - (r * 0.05f), y - (r * 0.65f));

            GL.End();
        }

        private void RenderCavalrySymbol(IGameObject entity)
        {
        }

        private void RenderButton(MenuButton entity, Color color)
        {
            GL.Color3(color);
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(entity.Position + new Vector2(-entity.Size.X / 2, -entity.Size.Y / 2));
            GL.Vertex2(entity.Position + new Vector2(entity.Size.X / 2, -entity.Size.Y / 2));
            GL.Vertex2(entity.Position + new Vector2(entity.Size.X / 2, entity.Size.Y / 2));
            GL.Vertex2(entity.Position + new Vector2(-entity.Size.X / 2, entity.Size.Y / 2));

            GL.End();
        }

        private void RenderMap()
        {
            RenderQuad(new List<Vector2> { new Vector2(-0.65f, 0.90f), new Vector2(-0.86f, 0.50f), new Vector2(-0.30f, 0.50f), new Vector2(-0.16f, 0.83f) }, Color.LimeGreen);
            RenderQuad(new List<Vector2> { new Vector2(-0.86f, 0.50f), new Vector2(-0.89f, 0.13f), new Vector2(-0.33f, 0.24f), new Vector2(-0.30f, 0.50f) }, Color.LimeGreen);
            RenderTriangle(new List<Vector2> { new Vector2(-0.89f, 0.13f), new Vector2(-0.37f, -0.02f), new Vector2(-0.33f, 0.24f) }, Color.LimeGreen);
            RenderQuad(new List<Vector2> { new Vector2(-0.37f, 0.02f), new Vector2(-0.22f, -0.23f), new Vector2(-0.16f, 0.24f), new Vector2(-0.33f, 0.24f) }, Color.LimeGreen);
            RenderQuad(new List<Vector2> { new Vector2(-0.22f, -0.23f), new Vector2(0.03f, -0.20f), new Vector2(0.20f, 0.06f), new Vector2(-0.16f, 0.24f) }, Color.LimeGreen);

            RenderTriangle(new List<Vector2> { new Vector2(-0.22f, -0.23f), new Vector2(-0.20f, -0.53f), new Vector2(0.03f, -0.20f) }, Color.LightGray);
            RenderQuad(new List<Vector2> { new Vector2(-0.20f, -0.53f), new Vector2(0.30f, -0.50f), new Vector2(0.22f, -0.27f), new Vector2(0.03f, -0.20f) }, Color.LightGray);
            RenderQuad(new List<Vector2> { new Vector2(-0.20f, -0.53f), new Vector2(-0.25f, -0.92f), new Vector2(0.22f, -0.90f), new Vector2(0.30f, -0.50f) }, Color.LimeGreen);

            RenderQuad(new List<Vector2> { new Vector2(0.22f, -0.90f), new Vector2(0.80f, -0.60f), new Vector2(0.47f, -0.40f), new Vector2(0.30f, -0.50f) }, Color.LimeGreen);
            RenderQuad(new List<Vector2> { new Vector2(0.80f, -0.60f), new Vector2(0.86f, 0.05f), new Vector2(0.42f, -0.27f), new Vector2(0.47f, -0.40f) }, Color.LimeGreen);
            RenderTriangle(new List<Vector2> { new Vector2(0.86f, 0.05f), new Vector2(0.38f, 0.02f), new Vector2(0.42f, -0.27f) }, Color.LimeGreen);
            RenderTriangle(new List<Vector2> { new Vector2(0.38f, 0.02f), new Vector2(0.86f, 0.05f), new Vector2(0.66f, 0.10f) }, Color.LimeGreen);

            RenderQuad(new List<Vector2> { new Vector2(0.22f, -0.27f), new Vector2(0.30f, -0.50f), new Vector2(0.47f, -0.40f), new Vector2(0.42f, -0.27f) }, Color.LightGray);
            RenderTriangle(new List<Vector2> { new Vector2(0.03f, -0.2f), new Vector2(0.22f, -0.27f), new Vector2(0.20f, 0.06f) }, Color.DarkGreen);
            RenderQuad(new List<Vector2> { new Vector2(0.22f, -0.27f), new Vector2(0.42f, -0.27f), new Vector2(0.38f, 0.02f), new Vector2(0.20f, 0.06f) }, Color.DarkGreen);
            RenderQuad(new List<Vector2> { new Vector2(0.20f, 0.06f), new Vector2(0.38f, 0.02f), new Vector2(0.66f, 0.10f), new Vector2(0.43f, 0.31f) }, Color.LightGray);

            RenderQuad(new List<Vector2> { new Vector2(0.66f, 0.10f), new Vector2(0.86f, 0.05f), new Vector2(0.70f, 0.40f), new Vector2(0.43f, 0.31f) }, Color.LightGray);
            RenderTriangle(new List<Vector2> { new Vector2(0.43f, 0.31f), new Vector2(0.70f, 0.40f), new Vector2(0.80f, 0.77f) }, Color.LimeGreen);
            RenderTriangle(new List<Vector2> { new Vector2(0.43f, 0.31f), new Vector2(0.80f, 0.77f), new Vector2(0.20f, 0.37f) }, Color.LimeGreen);
            RenderQuad(new List<Vector2> { new Vector2(0.20f, 0.37f), new Vector2(0.80f, 0.77f), new Vector2(0.50f, 0.90f), new Vector2(0.15f, 0.70f) }, Color.LimeGreen);

            RenderQuad(new List<Vector2> { new Vector2(0.20f, 0.06f), new Vector2(0.43f, 0.31f), new Vector2(0.20f, 0.37f), new Vector2(0.10f, 0.25f) }, Color.LightGray);
            RenderQuad(new List<Vector2> { new Vector2(0.10f, 0.25f), new Vector2(0.20f, 0.37f), new Vector2(0.15f, 0.70f), new Vector2(-0.12f, 0.55f) }, Color.LightGray);
            RenderTriangle(new List<Vector2> { new Vector2(0.20f, 0.06f), new Vector2(0.10f, 0.25f), new Vector2(-0.16f, 0.24f) }, Color.LimeGreen);
            RenderTriangle(new List<Vector2> { new Vector2(0.10f, 0.25f), new Vector2(-0.06f, 0.46f), new Vector2(-0.16f, 0.24f) }, Color.LimeGreen);

            RenderQuad(new List<Vector2> { new Vector2(-0.16f, 0.24f), new Vector2(-0.33f, 0.24f), new Vector2(-0.30f, 0.50f), new Vector2(-0.06f, 0.46f) }, Color.DarkGreen);
            RenderTriangle(new List<Vector2> { new Vector2(-0.30f, 0.50f), new Vector2(-0.06f, 0.46f), new Vector2(-0.12f, 0.55f) }, Color.LimeGreen);
            RenderTriangle(new List<Vector2> { new Vector2(-0.30f, 0.50f), new Vector2(-0.12f, 0.55f), new Vector2(-0.16f, 0.83f) }, Color.LimeGreen);
            RenderQuad(new List<Vector2> { new Vector2(-0.12f, 0.55f), new Vector2(-0.16f, 0.83f), new Vector2(0.07f, 0.95f), new Vector2(0.15f, 0.70f) }, Color.LimeGreen);

            RenderTriangle(new List<Vector2> { new Vector2(0.15f, 0.70f), new Vector2(0.07f, 0.95f), new Vector2(0.50f, 0.90f) }, Color.LimeGreen);
        }

        private void RenderQuad(List<Vector2> quad, Color color)
        {
            GL.Color4(color);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(quad[0]);
            GL.Vertex2(quad[1]);
            GL.Vertex2(quad[2]);
            GL.Vertex2(quad[3]);
            GL.End();
        }

        private void RenderTriangle(List<Vector2> tri, Color color)
        {
            GL.Color3(color);
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex2(tri[0]);
            GL.Vertex2(tri[1]);
            GL.Vertex2(tri[2]);
            GL.End();
        }

        private void RenderRange(IGameObject entity)
        {
            float corners = 50f;
            float delta = 2f * (float)Math.PI / corners;

            GL.Color4(0, 0, 0, 0.2);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(entity.Position.X + entity.Offset.X, entity.Position.Y + entity.Offset.Y);

            for (int i = 0; i < corners; i++)
            {
                var alpha = i * delta;
                float x = (entity.Radius + entity.Range) * (float)Math.Cos(alpha);
                float y = (entity.Radius + entity.Range) * (float)Math.Sin(alpha);
                GL.Vertex2(entity.Position.X + entity.Offset.X + x, entity.Position.Y + entity.Offset.Y + y);
            }

            GL.Vertex2(entity.Position.X + entity.Offset.X + entity.Radius + entity.Range, entity.Position.Y + entity.Offset.Y);
            GL.End();
        }

        private void RenderHealthDisplay(IGameObject entity, float damage)
        {
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(entity.Position + entity.Offset + new Vector2(0.1f, 0.15f)); // right up 
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(-0.1f, 0.15f)); // left up
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(-0.1f, 0.1f)); // left bottom
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(0.1f, 0.1f)); // right bottom

            GL.Color3(Color.Gray);

            GL.Vertex2(entity.Position + entity.Offset + new Vector2(0.095f, 0.14f));
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(-0.095f, 0.14f));
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(-0.095f, 0.11f));
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(0.095f, 0.11f));

            GL.Color3(Color.YellowGreen);

            GL.Vertex2(entity.Position + entity.Offset + new Vector2(0.095f - damage, 0.14f));
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(-0.095f, 0.14f));
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(-0.095f, 0.11f));
            GL.Vertex2(entity.Position + entity.Offset + new Vector2(0.095f - damage, 0.11f));

            GL.End();
        }

        private void SelectObject(IGameObject entity)
        {
            RenderRange(entity);
            RenderHealthDisplay(entity, entity.BarState);

            // RenderSelected(entity);
        }

        private void RenderDamageArmy(IGameObject entity, float timeleft)
        {
            float corners = 30f;
            float delta = 2f * (float)Math.PI / corners;

                if (timeleft % 2 == 0)
                {
                    GL.Color3(Color.Yellow);
                }
                else
                {
                    GL.Color4(0, 0, 0, 0);
                }

                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex2(entity.Position.X + entity.Offset.X, entity.Position.Y + entity.Offset.Y);

                for (int i = 0; i < corners; i++)
                {
                var alpha = i * delta;
                float x = (entity.Radius - 0.01f) * (float)Math.Cos(alpha);
                float y = (entity.Radius - 0.01f) * (float)Math.Sin(alpha);
                GL.Vertex2(entity.Position.X + entity.Offset.X + x, entity.Position.Y + entity.Offset.Y + y);
                }

                GL.Vertex2(entity.Position.X + entity.Offset.X + entity.Radius - 0.01f, entity.Position.Y + entity.Offset.Y);
                GL.End();
        }

        private void RenderDamageTower(IGameObject entity, float timeleft)
        {
            float x = entity.Position.X + entity.Offset.X;
            float y = entity.Position.Y + entity.Offset.Y;
            float r = entity.Radius;
            float offset = -0.01f;
            float height = 0.6f;
            float thickness = 0.7f;

            if (timeleft % 2 == 0)
            {
                Color color = Color.Yellow;

                RenderQuad(new List<Vector2>() { new Vector2(x - ((r + offset) * thickness), y - (r + offset)), new Vector2(x - ((r + offset) * thickness), y + ((r + offset) * height)), new Vector2(x + ((r + offset) * thickness), y + ((r + offset) * height)), new Vector2(x + ((r + offset) * thickness), y - (r + offset)) }, color);
                RenderQuad(new List<Vector2>() { new Vector2(x - ((r + offset) * thickness), y), new Vector2(x - ((r + offset) * thickness), y + (r + offset)), new Vector2(x - ((r - offset) * 0.4f), y + (r + offset)), new Vector2(x + offset, y - offset) }, color);
                RenderQuad(new List<Vector2>() { new Vector2(x + ((r + offset) * thickness), y), new Vector2(x + ((r + offset) * thickness), y + (r + offset)), new Vector2(x + ((r - offset) * 0.4f), y + (r + offset)), new Vector2(x - offset, y - offset) }, color);
            }
            else
            {
                Color color = Color.Transparent;

                RenderQuad(new List<Vector2>() { new Vector2(x - ((r + offset) * thickness), y - (r + offset)), new Vector2(x - ((r + offset) * thickness), y + ((r + offset) * height)), new Vector2(x + ((r + offset) * thickness), y + ((r + offset) * height)), new Vector2(x + ((r + offset) * thickness), y - (r + offset)) }, color);
                RenderQuad(new List<Vector2>() { new Vector2(x - ((r + offset) * thickness), y), new Vector2(x - ((r + offset) * thickness), y + (r + offset)), new Vector2(x - ((r - offset) * 0.4f), y + (r + offset)), new Vector2(x + offset, y - offset) }, color);
                RenderQuad(new List<Vector2>() { new Vector2(x + ((r + offset) * thickness), y), new Vector2(x + ((r + offset) * thickness), y + (r + offset)), new Vector2(x + ((r - offset) * 0.4f), y + (r + offset)), new Vector2(x - offset, y - offset) }, color);
            }
        }

        private void RenderUIButton(Button ui, Color color)
        {
            float x = ui.Position.X - (ui.Size.X / 2);
            float y = ui.Position.Y + (ui.Size.Y / 2);

            GL.Color3(color);

            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(ui.Position + new Vector2(-ui.Size.X / 2, -ui.Size.Y / 2));
            GL.Vertex2(ui.Position + new Vector2(ui.Size.X / 2, -ui.Size.Y / 2));
            GL.Vertex2(ui.Position + new Vector2(ui.Size.X / 2, ui.Size.Y / 2));
            GL.Vertex2(ui.Position + new Vector2(-ui.Size.X / 2, ui.Size.Y / 2));

            GL.End();

            GL.Color4(Color.White);
            GL.BindTexture(TextureTarget.Texture2D, fontImageData);

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0f, 0f);
            GL.Vertex2(x, y);
            GL.TexCoord2(1f, 0f);
            GL.Vertex2(x, y - 0.6f);
            GL.TexCoord2(1f, 1f);
            GL.Vertex2(x + 0.6f, y - 0.6f);
            GL.TexCoord2(0f, 1f);
            GL.Vertex2(x + 0.6f, y);

            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private int LoadTexture()
        {
            var image = new MagickImage(StrategyGame.Properties.Resources.FontBitmap);
            var format = PixelFormat.Rgb;
            var internalFormat = PixelInternalFormat.Rgb;
            if (image.ChannelCount == 4)
            {
                internalFormat = PixelInternalFormat.Rgba;
                format = PixelFormat.Rgba;
            }

            image.Rotate(-90);
            image.Flip();
            var bytes = image.GetPixelsUnsafe().ToArray();

            textureWidth = image.Width;
            textureHeight = image.Height;

            int texObj = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texObj);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, image.Width, image.Height, 0, format, PixelType.UnsignedByte, bytes);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            return texObj;
        }
    }
}
