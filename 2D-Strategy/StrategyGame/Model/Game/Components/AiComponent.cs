using StrategyGame.Model.IService.AI;
using StrategyGame.Service.AI;

namespace StrategyGame.Model.Game.Components
{
    public class AIComponent : IComponent
    {
        /// <summary>
        /// Gets reference to the entity which the component belongs to.
        /// </summary>
        public IEntity Parent { get; }

        /// <summary>
        /// Gets or sets the Tower to wich the entity is currently moving.
        /// </summary>
        public int TowerToDefend { get; set; }

        /// <summary>
        /// Gets how the enemy should consider the entity based on <see cref="AiType"/>.
        /// </summary>
        public AiType Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AIComponent"/> class.
        /// </summary>
        /// <param name="parent">Entity to wich the component belongs.</param>
        /// <param name="type">Definition of how to treat the entity based on <see cref="AiType"/>.</param>
        public AIComponent(IEntity parent, AiType type)
        {
            Parent = parent;
            parent?.AddComponent(this);
            TowerToDefend = -1;
            Type = type;
        }

        /// <summary>
        /// Function that is called each frame and tells the ai that the entity should be 
        /// considert when choosing what to do by registering it at <see cref="IAIService"/>.
        /// </summary>
        public void Update()
        {
            Parent.Scene.ServiceProvider.GetService<IAIService>().Register(Parent);
        }
    }
}
