using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using StrategyGame.Model.IService;
using StrategyGame.Service.Sound;

namespace StrategyGame.Model.Game.Components
{
    public class SelectComponent : IComponent
    {
        private readonly IKeystateService keyService;

        public IEntity Parent { get; }

        public bool Selected { get; set; }

        public IEntity SelectEntity { get; private set; }

        private static Vector2? startPoint;
        private static IEntity selectBox;

        public SelectComponent(IEntity parent)
        {
            Parent = parent;
            parent?.AddComponent(this);
            keyService = Parent.Scene.ServiceProvider.GetService<IKeystateService>();
        }

        public void Update()
        {
            if (Parent.Scene.ServiceProvider.GetService<ITimeService>().TimeScale == 0)
                return;

            if (keyService.IsButtonPressed(MouseButton.Left))
            {
                if (!startPoint.HasValue)
                    startPoint = keyService.MousePosition;

                var clicked = new List<IEntity>();
                foreach (var other in Parent.Scene.ServiceProvider.GetService<IGameObjectService>().GetEntities())
                {
                    if ((keyService.MousePosition - other.Position).Length > (other.Size.X / 2))
                        continue;

                    clicked.Add(other);
                }

                if (clicked.Count != 0)
                {
                    clicked.Sort((obj1, obj2) => (int)((keyService.MousePosition - obj1.Position).Length - (keyService.MousePosition - obj2.Position).Length));
                    if (Parent == clicked[0])
                    {
                        Selected = !Selected;
                        if (Selected == true && Parent.GetComponent<OwnerComponent>().Owner == 0)
                        {
                            Parent.Scene.ServiceProvider.GetService<IAudioService>().PlaySound("selected");
                        }
                    }
                    else
                    {
                        Selected = false;
                    }
                }
                else
                {
                    Selected = false;
                }
            }

            if (keyService.IsButtonReleased(MouseButton.Left) && startPoint.HasValue)
            {
                startPoint = null;
                if (selectBox != null)
                {
                    for (int i = 0; i < Parent.Scene.Entities.Count; i++)
                    {
                        if (Parent.Scene.Entities[i].HasComponent<SelectComponent>())
                        {
                            if (Parent.Scene.Entities[i].HasComponent<OwnerComponent>() && Parent.Scene.Entities[i].GetComponent<OwnerComponent>().Owner == 0)
                            {
                                if (Parent.Scene.Entities[i].GetComponent<SelectComponent>().Selected)
                                {
                                    Parent.Scene.ServiceProvider.GetService<IAudioService>().PlaySound("selected");
                                    break;
                                }
                            }
                        }
                    }

                    selectBox.Destroy();
                    selectBox = null;
                }
            }

            if (keyService.IsButtonDown(MouseButton.Left) && startPoint.HasValue && (startPoint.Value - keyService.MousePosition).Length > 0.05f)
            {
                if (!Parent.HasComponent<HealerComponent>())
                {
                    if (Parent.HasComponent<OwnerComponent>())
                    {
                        if (Parent.GetComponent<OwnerComponent>().Owner == 0)
                        {
                            Selected = true;

                            float minX = Math.Min(startPoint.Value.X, keyService.MousePosition.X);
                            float maxX = Math.Max(startPoint.Value.X, keyService.MousePosition.X);
                            float minY = Math.Min(startPoint.Value.Y, keyService.MousePosition.Y);
                            float maxY = Math.Max(startPoint.Value.Y, keyService.MousePosition.Y);

                            if (minX > (Parent.Position.X + (Parent.Size.X / 2)) || maxX < (Parent.Position.X - (Parent.Size.X / 2)) || minY > (Parent.Position.Y + (Parent.Size.Y / 2)) || maxY < (Parent.Position.Y - (Parent.Size.Y / 2)))
                            {
                                Selected = false;
                            }
                        }
                    }
                }
                else
                {
                    Selected = false;
                }

                UpdateSelectBox();
            }

            if (Selected && SelectEntity == null)
            {
                if (Parent.HasComponent<AttackComponent>() && Parent.GetComponent<AttackComponent>().Type == AttackComponent.AttackType.Far)
                {
                    SelectEntity = ((IGameEntityFactory)Parent.Scene.EntityFactory).CreateRangeDisplay(Parent, Parent.GetComponent<AttackComponent>().Range + (Parent.Size.X / 2));
                }
                else
                {
                    SelectEntity = ((IGameEntityFactory)Parent.Scene.EntityFactory).CreateSelect(Parent);
                }
            }

            if (!Selected && SelectEntity != null)
            {
                SelectEntity?.Destroy();
                SelectEntity = null;
            }
        }

        private void UpdateSelectBox()
        {
            if (selectBox == null && startPoint.HasValue)
            {
                selectBox = ((IGameEntityFactory)Parent.Scene.EntityFactory).CreateMouseBox(startPoint.Value, keyService.MousePosition);
            }

            if (selectBox != null && startPoint.HasValue)
            {
                selectBox.Position = ((keyService.MousePosition - startPoint.Value) * 0.5f) + startPoint.Value;
                selectBox.Size = new Vector2(startPoint.Value.X - keyService.MousePosition.X, startPoint.Value.Y - keyService.MousePosition.Y);
            }
        }
    }
}
