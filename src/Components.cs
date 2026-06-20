using Raylib_cs;
using System.Numerics;
using System.Threading.Channels;

namespace GUIComponentSystem
{
    public delegate void ClickFunction(); // runs when the UI is clicked
    interface IComponentFunctions // Bases functions for all UI
    {
        bool MouseClicked(ClickFunction clickFunction);
        bool MouseHovered();
        void DrawComponent(float thickness, bool makeWhite=false);
    }
    class Component(Rectangle bounds) : IComponentFunctions // uses the component functions
    {
        protected Rectangle Bounds = bounds; // where the 
        public virtual bool MouseClicked(ClickFunction clickFunction)
        {
            // if the mouse is clicked and its hovered 
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && MouseHovered())
            {
                clickFunction(); // call the click function
                return true; // its clicked   
            }
            return false;
        }
        public bool MouseHovered()
        {
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Bounds))
            {
                Console.WriteLine("YEESSS");
                return true;
            }
            Console.WriteLine("NOOO");
            return false;
        }
        public virtual void DrawComponent(float thickness, bool makeWhite){Raylib.DrawText("NO",10,10,10,Color.Black);}

        // Inheritance / derived classes
        public class Button(Rectangle bounds, string label) : Component(bounds)
        {
            public readonly Vector2 DrawPos = bounds.Position - (2*bounds.Center); // draw by center
            protected Color ButtonColor = Color.Black;
            public override void DrawComponent(float thickness, bool makeWhite) // default black outline
            {
                // Set the hover color
                if (MouseHovered())
                {
                    ButtonColor = Color.LightGray;
                } 
                else if (makeWhite == true)
                {
                    ButtonColor = Color.White;
                }
                else {ButtonColor = Color.Black;}


                // Draw the Curved Black or White Outline Button
                Raylib.DrawRectangleRoundedLinesEx(Bounds, 0.5f, 5, thickness, ButtonColor);

                // Drawing the Label TEMPORARY
                Raylib.DrawText(label, (int)(Bounds.Position.X+Bounds.Width/2), (int)(Bounds.Position.Y+Bounds.Height/2), 10, ButtonColor);
            }
        }
        public class ToggleButton(Rectangle bounds, string label, bool toggleValue) : Button(bounds, label)
        {
            public bool ToggleValue = toggleValue;
            public override bool MouseClicked(ClickFunction clickFunction)
            {
                ToggleValue = !ToggleValue; // flip the toggle value
                return base.MouseClicked(clickFunction); // call the base function
            }
            public override void DrawComponent(float thickness, bool makeWhite = false)
            {
                if (ToggleValue) ButtonColor = Color.DarkGray;
                base.DrawComponent(thickness, makeWhite); // call the base function from the Button
            }
        }
    
    }
}