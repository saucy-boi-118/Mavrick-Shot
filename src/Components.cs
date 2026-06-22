using Raylib_cs;
using System.Numerics;

namespace GUIComponentSystem
{
    interface IComponentFunctions // Bases functions for all UI
    {
        bool MouseClicked(Action clickAction);
        bool MouseHovered();
        void DrawComponent(float thickness, bool makeWhite=false);
        void UpdateComponent(bool makeWhite=false, Action? clickAction = null);
    }
    class Component(Rectangle bounds) : IComponentFunctions // uses the component functions
    {
        protected Rectangle Bounds = bounds; // where the 
        public virtual bool MouseClicked(Action? clickAction = null)
        {
            // if the mouse is clicked and its hovered 
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && MouseHovered())
            {
                clickAction?.Invoke();
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
        public virtual void DrawComponent(float thickness, bool makeWhite=false){}
        public virtual void UpdateComponent(bool makeWhite=false, Action? clickAction = null){}

        // Inheritance / derived classes
        public class Button(Rectangle bounds, string label) : Component(bounds)
        {
            protected Color ButtonColor = Color.Black;
            private readonly int FontSize = (int)((bounds.Width-(10)) / label.Length);
            public override void DrawComponent(float thickness, bool makeWhite=false) // default black outline
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
                Raylib.DrawText(label, (int)(Bounds.Center.X-(Raylib.MeasureText(label,FontSize)/2)), (int)(Bounds.Center.Y-(FontSize/2)), FontSize, ButtonColor);
            }
            public override void UpdateComponent(bool makeWhite = false, Action? clickAction = null)
            {
                MouseClicked(clickAction);
                DrawComponent(5,makeWhite);
            }
        }
        public class ToggleButton(Rectangle bounds, string label, ref bool toggleValue) : Button(bounds, label)
        {
            private bool returnedToggle = toggleValue;
            public virtual bool MouseClicked(ref bool toggleValue, Action? clickAction = null)
            {
                if (base.MouseClicked(clickAction)) // call the base function
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

            public virtual void UpdateComponent(ref bool toggleValue, bool makeWhite = false, Action? clickAction = null)
            {
                MouseClicked(ref toggleValue, clickAction);
                DrawComponent(5,makeWhite);
            }
        }
    
    }
}