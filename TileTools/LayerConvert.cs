using TiledSharp;

namespace TileTools
{
    public class LayerConvert
    {
		public static uint[,] ConvertLayerInt(TmxMap map, TmxLayer layer)
		{
			uint[,] gid = new uint[map.Width, map.Height];

			foreach (TmxLayerTile tile in layer.Tiles)
			{
				gid[tile.X, tile.Y] = (uint)tile.Gid;
			}

			return gid;
		}

		public static TileID[,] ConvertLayerTileID(TmxMap map, TmxLayer layer)
		{
			TileID[,] gid = new TileID[map.Width, map.Height];

			for (int x = 0; x < map.Width; x++)
			{
				for (int y = 0; y < map.Height; y++)
				{
					gid[x, y] = TileID.Empty;
				}
			}

			//First GIDs are in descending order
			foreach (TmxLayerTile tile in layer.Tiles)
			{
				for (int ts = map.Tilesets.Count - 1; ts >= 0; ts--)
				{
					TmxTileset set = map.Tilesets[ts];

					if (tile.Gid >= set.FirstGid)
					{
						gid[tile.X, tile.Y] = new TileID(ts, tile.Gid - set.FirstGid, tile.Gid);

						break;
					}
				}
			}

			return gid;
		}
	}
}
