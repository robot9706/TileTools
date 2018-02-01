namespace TileTools
{
	public struct TileID
	{
		public static readonly TileID Empty = new TileID(-1, -1, -1);

		public int Tileset;
		public int Tile;
		public int GID;

		public TileID(int tileset, int tile, int gid)
		{
			Tileset = tileset;
			Tile = tile;
			GID = gid;
		}
	}
}
