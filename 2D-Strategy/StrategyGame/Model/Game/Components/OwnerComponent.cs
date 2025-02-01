namespace StrategyGame.Model.Game.Components
{
    public class OwnerComponent : IComponent
    {
        public IEntity Parent { get; }

        public int Owner { get; set; }

        public OwnerComponent(IEntity parent, int owner)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Owner = owner;
        }

        public void Update()
        {
        }
    }
}
