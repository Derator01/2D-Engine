using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicsEngine.Structures;

namespace PhysicsEngine.Collisions
{
    public class BoxCollision : Collision
    {
        public Vector2 position;
        public float width, height;
        public float halfWidth { get { return width / 2; } }
        public float halfHeight { get { return height / 2; } }

        public BoxCollision(Vector2 position, float width = 1, float height = 1)
        {
            this.position = position;
            this.width = width;
            this.height = height;
        }

        public bool Intersects(Collision collision, Vector2 prediction)
        {
            if(collision.GetType().Equals(typeof(BoxCollision))) //Box Collision
            {
                BoxCollision boxCollision = (BoxCollision)collision;

                bool flag = position.x < boxCollision.position.x + boxCollision.width &&
                       position.x + width > boxCollision.position.x &&
                       position.y < boxCollision.position.y + boxCollision.height &&
                       position.y + height > boxCollision.position.y;

                bool flag1 = (prediction == null ? false : (prediction.x < boxCollision.position.x + boxCollision.width &&
                       prediction.x + width > boxCollision.position.x &&
                       prediction.y < boxCollision.position.y + boxCollision.height &&
                       prediction.y + height > boxCollision.position.y));

                return flag || flag1;
            }
            else if(collision.GetType().Equals(typeof(CircleCollision)))
            {
                CircleCollision circleCollision = (CircleCollision)collision;

                float testX = circleCollision.position.x;
                float testY = circleCollision.position.y;
                if (circleCollision.position.x < position.x) testX = position.x;
                else if (circleCollision.position.x > position.x + width) testX = position.x + width;
                if (circleCollision.position.y < position.y) testY = position.y;
                else if (circleCollision.position.y > position.y + height) testY = position.y + height;
                float dist = circleCollision.position.Distance(new Vector2(testX, testY));
                bool flag = dist <= circleCollision.radius;

                bool flag1 = false;

                if (prediction != null)
                {
                    float testX1 = prediction.x;
                    float testY1 = prediction.y;
                    if (prediction.x < position.x) testX1 = position.x;
                    else if (prediction.x > position.x + width) testX1 = position.x + width;
                    if (prediction.y < position.y) testY1 = position.y;
                    else if (prediction.y > position.y + height) testY1 = position.y + height;
                    float dist1 = circleCollision.position.Distance(new Vector2(testX1, testY1));
                    flag1 = dist1 <= circleCollision.radius;
                }

                return flag || flag1;
            }
            return false;   
        }

        public bool Intersects(Vector2 pos)
        {
            return pos.x >= position.x && pos.y >= position.y && pos.x < position.x + width && pos.y < position.y + height;
        }

        public Vector2 GetCenter()
        {
            return new Vector2(position.x + halfWidth, position.y + halfHeight);
        }

        public void UpdatePosition(Vector2 newPos)
        {
            this.position = newPos;
        }
    }
}
