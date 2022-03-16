using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicsEngine.Structures;

namespace PhysicsEngine.Collisions
{
    public class CircleCollision : Collision
    {
        public Vector2 position;
        public float radius;

        public CircleCollision(Vector2 position, float radius = 1)
        {
            this.position = position;
            this.radius = radius;
        }

        public Vector2 GetCenter()
        {
            return position;
        }

        public bool Intersects(Collision collision, Vector2 prediction = null)
        {
            if(collision.GetType().Equals(typeof(BoxCollision)))
            {
                BoxCollision boxCollision = (BoxCollision)collision;

                float testX = position.x;
                float testY = position.y;
                if (position.x < boxCollision.position.x) testX = boxCollision.position.x;
                else if (position.x > boxCollision.position.x + boxCollision.width) testX = boxCollision.position.x + boxCollision.width;
                if (position.y < boxCollision.position.y) testY = boxCollision.position.y;
                else if (position.y > boxCollision.position.y + boxCollision.height) testY = boxCollision.position.y + boxCollision.height;
                float dist = position.Distance(new Vector2(testX, testY));
                bool flag = dist <= radius;

                float testX1 = prediction.x;
                float testY1 = prediction.y;
                if (prediction.x < boxCollision.position.x) testX1 = boxCollision.position.x;
                else if (prediction.x > boxCollision.position.x + boxCollision.width) testX1 = boxCollision.position.x + boxCollision.width;
                if (prediction.y < boxCollision.position.y) testY1 = boxCollision.position.y;
                else if (prediction.y > boxCollision.position.y + boxCollision.height) testY1 = boxCollision.position.y + boxCollision.height;
                float dist1 = prediction.Distance(new Vector2(testX1, testY1));
                bool flag1 = dist1 <= radius;

                return flag || flag1;
            }
            else if (collision.GetType().Equals(typeof(CircleCollision)))
            {
                CircleCollision circleCollision = (CircleCollision)collision;

                bool flag = circleCollision.position.Distance(position) <= circleCollision.radius + radius;
                bool flag1 = (prediction == null ? false : circleCollision.position.Distance(prediction) <= circleCollision.radius + radius);

                return flag || flag1;
            }
            return false;
        }

        public bool Intersects(Vector2 pos)
        {
            return pos.Distance(position) < radius;
        }

        public void UpdatePosition(Vector2 newPos)
        {
            this.position = newPos;
        }
    }
}
