using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABSoftware;
using ConsoleEngine;
using PhysicsEngine;
using PhysicsEngine.Structures;
using PhysicsEngine.Collisions;
using System.Diagnostics;

namespace Physics2D
{
    class Program : EngineChar
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            instance = p;
            p.ChangeFont(I_PIXEL_WIDTH, I_PIXEL_HEIGHT);
            p.Construct(300, 300);
            p.Start();
        }

        public static Program instance;

        public const int I_PIXEL_WIDTH = 2;
        public const int I_PIXEL_HEIGHT = 2;

        public PhysicsSpace space;

        bool dragging = false;
        PhysicObject draggingObject;

        bool moving = false;
        PhysicObject movingObject;
        Point movePrevCursorPos = new Point(0, 0);
        int movingX = 0, movingY = 0;

        Timer previewTimer = new Timer(1000);
        float createObjectPreview = 3f;
        int createObjectId = 0;
        string[] objectIds = new string[] { "box", "circle" };

        string chars = "0123456789";

        public override void OnCreate()
        {
            QuickEditMode(false);
            Keyboard.OnKeyUp += Keyboard_OnKeyUp;
            Keyboard.OnKeyDown += Keyboard_OnKeyDown;
            space = new PhysicsSpace();
            SpawnObjects();
            Move(GetWindowRectangle().left, GetWindowRectangle().top, GetConsoleWidth() + 8, GetConsoleHeight() + 8);
            //space.AddObject(new Box(new Vector2(0, ScreenHeight - 10), ScreenWidth, 10f, '█') { isKinematic = true });
        }

        public void SpawnObjects()
        {
            for (int i = 0; i < 10; i++)
            {
                space.AddObject(new Box(new Vector2(Randomizer.RandomFloat(10f, ScreenWidth - 10f), Randomizer.RandomFloat(10f, ScreenHeight - 10f)), 10f, 10f, (char)chars[Randomizer.RandomInt(0, chars.Length - 1)]));
            }
        }

        private void Keyboard_OnKeyUp(string VK_Name, int VK_KEY)
        {
            if(isFocused && VK_KEY.Equals(Keyboard.Keys.VK_UP)) //Scroll objects up
            {
                if(createObjectPreview <= 0f)
                {
                    createObjectPreview = 3f;
                    return;
                }
                else
                {
                    createObjectPreview = 3f;
                    createObjectId++;
                    if (createObjectId >= objectIds.Length)
                        createObjectId = 0;
                }
            }
            if (isFocused && VK_KEY.Equals(Keyboard.Keys.VK_DOWN)) //Scroll objects down
            {
                if (createObjectPreview <= 0f)
                {
                    createObjectPreview = 3f;
                    return;
                }
                else
                {
                    createObjectPreview = 3f;
                    createObjectId--;
                    if (createObjectId < 0)
                        createObjectId = objectIds.Length - 1;
                }
            }
            if (isFocused && VK_KEY.Equals(Keyboard.Keys.VK_DELETE))
            {
                PhysicObject[] objs = space.GetObjects();
                Point cursor = CursorPos();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].collision.Intersects(new Vector2(cursor.x, cursor.y)))
                    {
                        space.RemoveObject(objs[i]);
                        break;
                    }
                }
            }
            if (isFocused && VK_KEY.Equals(Keyboard.Keys.C))
            {
                space.ClearObjects();
            }
            if (isFocused && VK_KEY.Equals(Keyboard.Keys.E))
            {
                PhysicObject[] objs = space.GetObjects();
                Point cursor = CursorPos();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].collision.Intersects(new Vector2(cursor.x, cursor.y)))
                    {
                        return;
                    }
                }
                switch(objectIds[createObjectId])
                {
                    case "box":
                        {
                            space.AddObject(new Box(new Vector2(cursor.x - 5f, cursor.y - 5f), 10f, 10f, (char)chars[Randomizer.RandomInt(0, chars.Length - 1)]));
                        }
                        break;
                    case "circle":
                        {
                            space.AddObject(new Circle(new Vector2(cursor.x, cursor.y), 5f, (char)chars[Randomizer.RandomInt(0, chars.Length - 1)]));
                        }
                        break;
                }
            }
            if (isFocused && VK_KEY.Equals(Keyboard.Keys.VK_LBUTTON))
            {
                if(dragging && draggingObject != null)
                {
                    //grabbedObject.hasPhysics = true;
                    Point cursor = CursorPos();
                    Vector2 cursPos = new Vector2(cursor.x, cursor.y);
                    Vector2 center = draggingObject.collision.GetCenter();
                    draggingObject.velocity = (cursPos - center) * 0.07f;
                }
                dragging = false;
                draggingObject = null;
            }
            if (isFocused && VK_KEY.Equals(Keyboard.Keys.VK_RBUTTON))
            {
                if (moving && movingObject != null)
                {
                    movingObject.hasPhysics = true;
                    Point cursor = CursorPos();
                    Vector2 cursPos = new Vector2(cursor.x, cursor.y);
                    Vector2 prevPos = new Vector2(movePrevCursorPos.x, movePrevCursorPos.y);
                    movingObject.velocity = (cursPos - prevPos) * 0.2f;
                }
                moving = false;
                movingObject = null;
            }
            if (isFocused && VK_KEY.Equals(Keyboard.Keys.R))
            {
                space.ClearObjects();
                SpawnObjects();
            }
        }

        private void Keyboard_OnKeyDown(string VK_Name, int VK_KEY)
        {
            if(isFocused && VK_KEY.Equals(Keyboard.Keys.VK_LBUTTON) && !moving)
            {
                PhysicObject[] objs = space.GetObjects();
                Point cursor = CursorPos();
                for(int i = 0; i < objs.Length; i++)
                {
                    if(objs[i].collision.Intersects(new Vector2(cursor.x, cursor.y)))
                    {
                        dragging = true;
                        draggingObject = objs[i];
                        break;
                    }
                }
            }
            if (isFocused && VK_KEY.Equals(Keyboard.Keys.VK_RBUTTON) && !dragging)
            {
                PhysicObject[] objs = space.GetObjects();
                Point cursor = CursorPos();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].collision.Intersects(new Vector2(cursor.x, cursor.y)))
                    {
                        movingObject = objs[i];
                        movingObject.hasPhysics = false;
                        moving = true;
                        movingObject = objs[i];
                        movingX = cursor.x;
                        movingY = cursor.y;
                        movePrevCursorPos = new Point(cursor.x, cursor.y);
                        movingObject.velocity = Vector2.Zero;
                        break;
                    }
                }
            }
        }

        public override void OnUpdate(float elapsedTime)
        {
            AppName = $"FPS: {1000f/elapsedTime}";
            if (previewTimer.Tick())
                createObjectPreview -= 1f;
            if (createObjectPreview < 0f)
                createObjectPreview = 0f;
            Clear(' ');
            Keyboard.UpdateKeys();
            space.Update(elapsedTime);
            space.Render(this);
            /*if(box1.collision.Intersects(box2.collision))
            {
                float[] rotations = box1.position.GetRotations(box2.position);
                box1.velocity = new Vector2(rotations[0], rotations[1]) * -2f;
            }
            if (box2.collision.Intersects(box1.collision))
            {
                float[] rotations = box2.position.GetRotations(box1.position);
                box2.velocity = new Vector2(rotations[0], rotations[1]) * -2f;
            }*/

            if (dragging && draggingObject != null)
            {
                DragObject(draggingObject);
            }
            if(moving && movingObject != null)
            {
                MoveObject(movingObject);
            }

            if(createObjectPreview > 0) //draw the object preview
            {
                switch(objectIds[createObjectId])
                {
                    case "box":
                        {
                            FillRect(ScreenWidth - 10, ScreenHeight - 10, 10, 10, '█');
                        }
                        break;
                    case "circle":
                        {
                            FillCircle(ScreenWidth - 6, ScreenHeight - 6, 5, 5, '█');
                        }
                        break;
                }
            }
        }

        public void DragObject(PhysicObject obj)
        {
            Point cursor = CursorPos();
            Vector2 center = obj.collision.GetCenter();
            DrawLine((int)center.x, (int)center.y, cursor.x, cursor.y, '█');
        }

        public void MoveObject(PhysicObject obj)
        {
            Point cursor = CursorPos();
            movePrevCursorPos = new Point(movingX, movingY);
            obj.MoveTo(obj.position + new Vector2((cursor.x - movingX), (cursor.y - movingY)));
            movingX = cursor.x;
            movingY = cursor.y;
        }

        public Point CursorPos()
        {
            Windows.RECT rect = GetWindowRectangle();
            Point p = GetCursor();
            int x = (p.x - rect.left - 8) / I_PIXEL_WIDTH;
            int y = (p.y - rect.top - 30) / I_PIXEL_HEIGHT;
            return new Point(x, y);
        }

        public class Box : PhysicObject
        {
            public char color = ' ';
            public float width, height;

            public Box(Vector2 position, float width, float height, char color) : base(position, new BoxCollision(position, width, height))
            {
                this.color = color;
                this.width = width;
                this.height = height;
            }

            public override void Render(EngineChar engine)
            {
                engine.FillRect((int)(position.x), (int)(position.y), (int)width, (int)height, color);
            }
        }

        public class Circle : PhysicObject
        {
            public char color = ' ';
            public float radius;

            public Circle(Vector2 position, float radius, char color) : base(position, new CircleCollision(position, radius))
            {
                this.color = color;
                this.radius = radius;
            }

            public override void Render(EngineChar engine)
            {
                engine.FillCircle((int)position.x, (int)position.y, (int)radius, (int)radius, color);
            }
        }
    }
}