using Engine.Game;

namespace Engine
{
    class Program
    {
        public static void Main(string[] args)
        {
            Game.Game game = new TestGame(GLFW.Glfw.PrimaryMonitor.WorkArea.Width, GLFW.Glfw.PrimaryMonitor.WorkArea.Height, "Engine");
            game.Run();
        }
    }
}