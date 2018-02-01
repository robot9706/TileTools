using Microsoft.Xna.Framework;

namespace TileTools
{
	public class Entity
	{
		public Tilemap Map;

		public virtual void OnAddedToWorld() { }
		public virtual void OnRemovedFromWorld() { }

		public virtual void OnUpdate(GameTime time) { }
	}
}
