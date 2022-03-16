using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleEngine;
using PhysicsEngine.Collisions;
using PhysicsEngine.Structures;

namespace PhysicsEngine
{
    public class PhysicObject
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public Collision collision;
        public bool hasPhysics = true;
        public bool isKinematic = false;

        public PhysicObject(Vector2 position, Collision collision)
        {
            if(position != null)
                this.position = position;
            this.collision = collision;
        }

        public void MoveTo(Vector2 pos)
        {
            this.position = pos;
            collision.UpdatePosition(pos);
        }

        public virtual void Render(EngineChar engine) { /*NoRender*/ }
    }
}
