using Engine.Game;

namespace Engine
{
    class Program
    {
        public static void Main(string[] args)
        {
            Game.Game game = new TestGame(800, 600, "Test game!");
            game.Run();
        }
    }
}