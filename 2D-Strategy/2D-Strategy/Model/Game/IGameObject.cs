using System.Collections.Generic;
using OpenTK;
using StrategyGame.Model.Game;

namespace StrategyGame.Model
{
    /// <summary>
    /// A game object on the map
    /// </summary>
    public interface IGameObject : IEntity
    {
        /// <summary>
        /// Gets the size of the entity
        /// </summary>
        float Radius { get; }

        Vector2 Offset { get; }

        /// <summary>
        /// Gets the owner of the entity
        /// </summary>
        PlayerType Owner { get; }

        /// <summary>
        /// Gets the range of the entity
        /// </summary>
        float Range { get; }

        /// <summary>
        /// Gets or sets the health of the entity
        /// </summary>
        float Health { get; set; }

        /// <summary>
        /// Gets the attack damage of the entity
        /// </summary>
        int AttackDamage { get; }

        /// <summary>
        /// Gets the attack speed of the entity
        /// </summary>
        float AttackSpeed { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is selected
        /// </summary>
        bool Selected { get; set; }

        IGameObject Target { get; }

        bool IsDead { get; }

        bool HealthChange { get; }

        bool IsAttack { get; }

        bool IsHealing { get; }

        float BarState { get; set; }

        float DamageAnimation { get; }

        List<IEntity> UpdateAttack(float time, List<IGameObject> others);

        List<IEntity> UpdateHealing(float time, List<IGameObject> others);

        void Damage(int damage);

        bool Healing(float heal);

        float HealthLoss(float damage);

        void Update(float time);
    }
}
