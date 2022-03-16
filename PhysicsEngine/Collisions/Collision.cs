using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicsEngine.Structures;

namespace PhysicsEngine.Collisions
{
    public interface Collision
    {
        bool Intersects(Collision collision, Vector2 prediction = null);
        bool Intersects(Vector2 pos);
        Vector2 GetCenter();
        void UpdatePosition(Vector2 newPos);
    }
}