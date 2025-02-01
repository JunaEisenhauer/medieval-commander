using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using StrategyGame.Model.IService;
using StrategyGame.Service.Sound;

namespace StrategyGame.Model.Game.Components
{
    public class BuyArmyComponent : IComponent
    {
        public IEntity Parent { get; }

        private readonly List<IEntity> buyButtons;

        public BuyArmyComponent(IEntity parent)
        {
            Parent = parent;
            parent?.AddComponent(this);
            buyButtons = new List<IEntity>();
        }

        public void Update()
        {
            if (Parent.Scene.ServiceProvider.GetService<ITimeService>().TimeScale == 0)
                return;

            if (Parent.GetComponent<OwnerComponent>().Owner != 0)
                return;

            if (!Parent.GetComponent<SelectComponent>().Selected)
            {
                ClearBuyButtons();
                return;
            }

            IGameObjectService gameObjectService = Parent.Scene.ServiceProvider.GetService<IGameObjectService>();
            foreach (var entity in gameObjectService.GetEntities())
            {
                if (!entity.HasComponent<BuyArmyComponent>())
                    continue;

                // return if other entity with BuyArmyComponent is selected
                if (entity != Parent && entity.GetComponent<SelectComponent>().Selected)
                {
                    ClearBuyButtons();
                    return;
                }
            }

            CreateBuyButtons();
        }

        public void BuyArmy(ArmyPrice armyPrice)
        {
            if (Parent.GetComponent<OwnerComponent>().Owner == 0)
                Parent.GetComponent<SelectComponent>().Selected = true;

            int owner = Parent.GetComponent<OwnerComponent>().Owner;
            IGoldService goldService = Parent.Scene.ServiceProvider.GetService<IGoldService>();
            int gold = goldService.GetGold(owner);

            if (gold < (int)armyPrice)
                return;

            goldService.RemoveGold(owner, (int)armyPrice);

            IGameEntityFactory factory = (IGameEntityFactory)Parent.Scene.EntityFactory;
            IEntity entity = null;
            switch (armyPrice)
            {
                case ArmyPrice.Archer:
                    if (Parent.GetComponent<OwnerComponent>().Owner == 0)
                        Parent.Scene.ServiceProvider.GetService<IAudioService>().PlaySound("intro");
                    entity = factory.CreateArcher(Parent.Position, owner, 0);
                    break;
                case ArmyPrice.Knight:
                    if (Parent.GetComponent<OwnerComponent>().Owner == 0)
                        Parent.Scene.ServiceProvider.GetService<IAudioService>().PlaySound("intro");
                    entity = factory.CreateKnight(Parent.Position, owner, 0);
                    break;
                case ArmyPrice.Catapult:
                    if (Parent.GetComponent<OwnerComponent>().Owner == 0)
                        Parent.Scene.ServiceProvider.GetService<IAudioService>().PlaySound("intro");
                    entity = factory.CreateCatapult(Parent.Position, owner, 0);
                    break;
            }

            if (entity != null)
                new MoveRandomComponent(entity);
        }

        private void CreateBuyButtons()
        {
            if (buyButtons.Count != 0)
                return;

            IMapService mapService = Parent.Scene.ServiceProvider.GetService<IMapService>();

            var startPos = new Vector2(mapService.Width * 0.03125f, mapService.Height * 0.3f);
            var offSet = new Vector2(0, mapService.Height * 0.26f);

            CreateBuyButton(startPos, "ArcherButton", ArmyPrice.Archer);
            startPos += offSet;
            CreateBuyButton(startPos, "KnightButton", ArmyPrice.Knight);
            startPos += offSet;
            CreateBuyButton(startPos, "CatapultButton", ArmyPrice.Catapult);

            foreach (var buyButton in buyButtons)
            {
                Parent.AddChild(buyButton);
            }
        }

        private void CreateBuyButton(Vector2 position, string texture, ArmyPrice armyPrice)
        {
            IGameEntityFactory factory = (IGameEntityFactory)Parent.Scene.EntityFactory;
            IMapService mapService = Parent.Scene.ServiceProvider.GetService<IMapService>();

            var buyButton = factory.CreateBuyButton(position, new Vector2(mapService.Width * 0.085f), texture, () => BuyArmy(armyPrice));
            buyButton.AddChild(factory.CreateIngot(buyButton.Position - new Vector2(mapService.Width * 0.02f, mapService.Height * -0.1f), new Vector2(mapService.Width * 0.035f)));
            buyButton.AddChild(factory.CreateTextDisplay(buyButton.Position - new Vector2(mapService.Width * -0.01f, mapService.Height * -0.1f), new Vector2(mapService.Width * 0.035f), (int)armyPrice + string.Empty, Color.Black));
            buyButtons.Add(buyButton);
        }

        private void ClearBuyButtons()
        {
            if (buyButtons.Count == 0)
                return;

            foreach (var buyButton in buyButtons)
            {
                Parent.RemoveChild(buyButton);
                buyButton.Destroy();
            }

            buyButtons.Clear();
        }
    }

    public enum ArmyPrice
    {
        Archer = 50,
        Knight = 60,
        Catapult = 100,
    }
}
