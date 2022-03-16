using ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicsEngine.Structures;
using Physics2D;

namespace PhysicsEngine
{
    public class PhysicsSpace
    {
        PhysicObject[] objects = null;

        public PhysicsSpace()
        {
            this.objects = new PhysicObject[0];
        }

        public PhysicObject[] GetObjects()
        {
            return objects;
        }

        public void AddObject(PhysicObject obj)
        {
            Array.Resize(ref objects, objects.Length + 1);
            objects[objects.Length - 1] = obj;
        }

        public void RemoveObject(PhysicObject obj)
        {
            for(int i = 0; i < objects.Length; i++)
            {
                if(objects[i] == obj)
                {
                    RemoveObject(i);
                    break;
                }
            }
        }

        public void RemoveObject(int id)
        {
            objects = objects.Where((source, index) => index != id).ToArray();
        }

        public void ClearObjects()
        {
            objects = new PhysicObject[0];
        }

        public void Update(float elapsedTime)
        {
            for(int i = 0; i < objects.Length; i++)
            {
                PhysicObject obj = objects[i];
                obj.MoveTo(obj.position += obj.velocity);
                obj.velocity *= 0.93f;
                /*if(obj.hasPhysics && !obj.isKinematic)
                {
                    obj.velocity.y += 9.81f * 0.05f;
                }*/

                if (obj.velocity.x < 0.001f && obj.velocity.x > -0.001f)
                    obj.velocity.x = 0f;
                if (obj.velocity.y < 0.001f && obj.velocity.y > -0.001f)
                    obj.velocity.y = 0f;

                for (int j = 0; j < objects.Length; j++)
                {
                    PhysicObject colObj = objects[j];
                    if (colObj.Equals(obj))
                        continue;
                    if (colObj.collision.Intersects(obj.collision, colObj.position + colObj.velocity))
                    {
                        Vector2 colObjVelocity = colObj.velocity;
                        Vector2 objVelocity = obj.velocity;
                        if(colObj.hasPhysics && !colObj.isKinematic)
                        {
                            float[] rotations1 = colObj.collision.GetCenter().GetRotations(obj.collision.GetCenter());
                            colObj.velocity = objVelocity + new Vector2(rotations1[0], rotations1[1]).Normalize() * -1f;
                        }

                        if(obj.hasPhysics && !obj.isKinematic)
                        {
                            float[] rotations2 = obj.collision.GetCenter().GetRotations(colObj.collision.GetCenter());
                            obj.velocity = colObjVelocity + new Vector2(rotations2[0], rotations2[1]).Normalize() * -1f;
                        }
                    }
                }
            }
        }

        public void Render(EngineChar engine)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                PhysicObject obj = objects[i];

                obj.Render(engine);
            }
        }
    }
}
