using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace TileTools
{
	public class Tilemap
	{
		public class TilesetRender
		{
			private Texture2D _texture;
			private Point _tileCount;
			private Vector2 _tileSize;

			public TilesetRender(Texture2D texture, int tilesWidth, int tilesHeight)
			{
				_texture = texture;
				_tileCount = new Point(tilesWidth, tilesHeight);
				_tileSize = new Vector2(_texture.Width, _texture.Height) / new Vector2(tilesWidth, tilesHeight);
			}

			public void DrawTile(SpriteBatch batch, int tile, Vector2 pos)
			{
				int tx = tile % _tileCount.X;
				int ty = tile / _tileCount.X;

				batch.Draw(_texture, pos, new Rectangle((int)(_tileSize.X * tx), (int)(_tileSize.Y * ty), (int)_tileSize.X, (int)_tileSize.Y), Color.White);
			}
		}

		private List<Entity> _entityList;

		private TileID[][,] _layers;
		private Dictionary<string, int> _layerLookup;
		private int _solidLayer = -1;

		private Point _tilesOnScreen;
		private Point _tileSize;
		private Point _mapSize;

		private TilesetRender[] _renderers;

		public Vector2 ViewOffset;

		public Tilemap(Point screenSize, Point tileSize)
		{
			_entityList = new List<Entity>();
			_tileSize = tileSize;

			_tilesOnScreen = new Point((int)Math.Ceiling(screenSize.X / (float)tileSize.X + 0.5f), (int)Math.Ceiling(screenSize.Y / (float)tileSize.Y + 0.5f));
		}

		public void LoadMap(string tmx, TilesetRender[] tilesetRenderers)
		{
			_renderers = tilesetRenderers;

			TmxMap map = new TmxMap(tmx);
			_mapSize = new Point(map.Width, map.Height);

			_layers = new TileID[map.Layers.Count][,];
			_layerLookup = new Dictionary<string, int>();
			for (int x = 0; x < map.Layers.Count; x++)
			{
				TmxLayer layer = map.Layers[x];

				_layerLookup.Add(layer.Name, x);

				_layers[x] = LayerConvert.ConvertLayerTileID(map, layer);

				if (layer.Name.Equals("solidlayer", StringComparison.InvariantCulture))
					_solidLayer = x;
			}
		}

		public bool IsInMap(int x, int y)
		{
			return (x >= 0 && y >= 0 && x < _mapSize.X && y < _mapSize.Y);
		}

		public TileID[,] GetLayer(string name)
		{
			return _layers[_layerLookup[name]];
		}

		public bool IsSolidAt(int wx, int wy)
		{
			if (_solidLayer == -1)
				return false;

			if (!IsInMap(wx, wy))
				return false;

			return (_layers[_solidLayer][wx, wy].Tile != -1);
		}

		public TileID GetTileAt(string layer, int wx, int wy)
		{
			if (!IsInMap(wx, wy))
				return TileID.Empty;

			return _layers[_layerLookup[layer]][wx, wy];
		}

		public void AddEntity(Entity e)
		{
			_entityList.Add(e);
			e.Map = this;

			e.OnAddedToWorld();
		}

		public void RemoveEntity(Entity e)
		{
			_entityList.Remove(e);

			e.OnRemovedFromWorld();
		}

		public void Update(GameTime time)
		{
			for (int e = _entityList.Count - 1; e >= 0; e--)
			{
				if (e >= _entityList.Count) continue;

				_entityList[e].OnUpdate(time);
			}
		}

		public void Draw(SpriteBatch batch)
		{
			Point view = new Point((int)Math.Floor(ViewOffset.X / (float)_tileSize.X - 0.5f), (int)Math.Floor(ViewOffset.Y / (float)_tileSize.Y - 0.5f));

			for (int l = 0; l < _layers.Length; l++)
			{
				TileID[,] tiles = _layers[l];

				for (int y = view.Y; y < view.Y + _tilesOnScreen.Y + 1; y++)
				{
					if (y < 0 || y >= _mapSize.Y)
						continue;

					for (int x = view.X; x < view.X + _tilesOnScreen.X + 1; x++)
					{
						if (x < 0 || x >= _mapSize.X)
							continue;

						TileID tile = tiles[x, y];

						if (tile.Tileset == -1)
							continue;

						Vector2 pos = new Vector2(x * _tileSize.X, y * _tileSize.Y) - ViewOffset;

						_renderers[tile.Tileset].DrawTile(batch, tile.Tile, pos);
					}
				}
			}
		}
	}
}
