using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine.Structures
{
    public class Vector2
    {
        public static Vector2 Zero = new Vector2();
        public static Vector2 One = new Vector2(1f, 1f);
        public static Vector2 Right = new Vector2(1f);
        public static Vector2 Up = new Vector2(0f, 1f);

        public float x = 0, y = 0;

        public Vector2(float x = 0f, float y = 0f)
        {
            this.x = x;
            this.y = y;
        }

        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        public float Distance(Vector2 to)
        {
            return (to - this).Length();
        }

        public Vector2 Normalize()
        {
            float l = Length();
            return new Vector2(x / l, y / l);
        }

        public float[] GetRotations(Vector2 to)
        {
            float sin = (to - this).x / Distance(to);
            float cos = (to - this).y / Distance(to);

            return new float[] { sin, cos };
        }

        #region Operators
        #region Vectors
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator * (Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }

        public static Vector2 operator %(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x % b.x, a.y % b.y);
        }
        #endregion

        #region Floats
        public static Vector2 operator +(Vector2 a, float b)
        {
            return new Vector2(a.x + b, a.y + b);
        }

        public static Vector2 operator -(Vector2 a, float b)
        {
            return new Vector2(a.x - b, a.y - b);
        }

        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }

        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.x / b, a.y / b);
        }

        public static Vector2 operator %(Vector2 a, float b)
        {
            return new Vector2(a.x % b, a.y % b);
        }
        #endregion
        #endregion

        public override string ToString()
        {
            return "(" + this.x + ", " + this.y + ")";
        }
    }
}
