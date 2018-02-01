using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileTools
{
	public class Entity
	{
		public Tilemap Map;

		public virtual void OnAddedToWorld() { }
		public virtual void OnRemovedFromWorld() { }

		public virtual void OnUpdate(GameTime time) { }
		public virtual void OnDraw(SpriteBatch batch) { }
	}
}
