using Raylib_cs;
using System.Numerics;
using System.Threading.Channels;

namespace GUIComponentSystem
{
    interface IComponentFunctions // Bases functions for all UI
    {
        bool MouseClicked();
        bool MouseHovered();
        void DrawComponent(float thickness, bool makeWhite=false);
    }
    class Component(Rectangle bounds) : IComponentFunctions // uses the component functions
    {
        protected Rectangle Bounds = bounds; // where the 
        public virtual bool MouseClicked()
        {
            // if the mouse is clicked and its hovered 
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && MouseHovered())
            {
                return true; // its clicked   
            }
            return false;
        }
        public bool MouseHovered()
        {
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Bounds))
            {
                return true;
            }
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
        public class ToggleButton(Rectangle bounds, string label) : Button(bounds, label)
        {
            private bool returnedToggle;
            public virtual bool MouseClicked(ref bool toggleValue)
            {
                if (base.MouseClicked()) // call the base function
                {
                    toggleValue = !toggleValue; // flip the toggle value
                    returnedToggle = toggleValue;
                    return true;
                }
                return false; 
            }
            public override void DrawComponent(float thickness, bool makeWhite = false)
            {
                if (returnedToggle) ButtonColor = Color.DarkGray; 
                base.DrawComponent(thickness, makeWhite); // call the base function from the Button
            }
        }
    
    }
}