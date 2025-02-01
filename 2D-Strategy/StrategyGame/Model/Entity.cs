using System;
using System.Collections.Generic;
using OpenTK;
using StrategyGame.Model.IService;
using StrategyGame.Service;

namespace StrategyGame.Model
{
    /// <summary>
    /// The implementation of the IEntity interface.
    /// </summary>
    public class Entity : IEntity
    {
        private readonly List<IComponent> addComponents = new List<IComponent>();
        private readonly List<Type> removeComponents = new List<Type>();

        public IScene Scene { get; }

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        private List<IEntity> children;
        private Dictionary<Type, IComponent> components;
        private bool componentsLocked;

        public Entity(IScene scene, Vector2 position, Vector2 size, bool isGameObject)
        {
            Scene = scene;
            scene?.AddEntity(this);
            Position = position;
            Size = size;
            children = new List<IEntity>();
            components = new Dictionary<Type, IComponent>();

            if (isGameObject)
                scene?.ServiceProvider.GetService<IGameObjectService>().AddEntity(this);
        }

        /// <summary>
        /// Updates all components of the entity.
        /// </summary>
        public void Update()
        {
            componentsLocked = true;
            foreach (var component in components.Values)
            {
                component.Update();
            }

            foreach (var type in removeComponents)
            {
                components.Remove(type);
            }

            removeComponents.Clear();

            foreach (var component in addComponents)
            {
                if (components.ContainsKey(component.GetType()))
                    components.Remove(component.GetType());
                components.Add(component.GetType(), component);
            }

            addComponents.Clear();

            componentsLocked = false;
        }

        public void AddChild(IEntity entity)
        {
            children.Add(entity);
        }

        public void RemoveChild(IEntity entity)
        {
            children.Remove(entity);
        }

        public void AddComponent(IComponent component)
        {
            if (!componentsLocked)
            {
                if (components.ContainsKey(component.GetType()))
                    components.Remove(component.GetType());
                components.Add(component.GetType(), component);
            }
            else
            {
                addComponents.Add(component);
            }
        }

        public void RemoveComponent<T>()
            where T : IComponent
        {
            if (!componentsLocked)
            {
                components.Remove(typeof(T));
            }
            else
            {
                removeComponents.Add(typeof(T));
            }
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            return (T)components[typeof(T)];
        }

        public bool HasComponent<T>()
            where T : IComponent
        {
            return components.ContainsKey(typeof(T));
        }

        public void Destroy()
        {
            foreach (var child in children)
            {
                child.Destroy();
            }

            Scene.ServiceProvider.GetService<IGameObjectService>().RemoveEntity(this);
            Scene.RemoveEntity(this);
        }
    }
}
