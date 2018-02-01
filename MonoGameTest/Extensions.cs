using Microsoft.Xna.Framework;

namespace MonoGameTest
{
	public static class Extensions
	{
		public static float DeltaTime(this GameTime time)
		{
			return (float)time.ElapsedGameTime.TotalSeconds;
		}

		public static Vector2 ToVector(this Point point)
		{
			return new Vector2(point.X, point.Y);
		}

		public static Point ToPointFloor(this Vector2 vector)
		{
			return new Point((int)vector.X, (int)vector.Y);
		}
	}
}
