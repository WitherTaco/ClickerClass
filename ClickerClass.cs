using ClickerClass.Effects;
using ClickerClass.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ClickerClass
{
	public class ClickerClass : Mod
	{
		public static ModHotKey AutoClickKey;
		internal static ClickerClass mod;

		/// <summary>
		/// To prevent certain methods being called when they shouldn't
		/// </summary>
		internal static bool finalizedRegisterCompat = false;

		public override void Load()
		{
			finalizedRegisterCompat = false;
			mod = this;
			AutoClickKey = RegisterHotKey("Clicker Accessory", "G"); //Can't localize this
			ClickerSystem.Load();
			ClickEffect.LoadMiscEffects();

			if (!Main.dedServ)
			{
				LoadClient();
			}
		}

		public override void Unload()
		{
			finalizedRegisterCompat = false;
			ShaderManager.Unload();
			ClickerSystem.Unload();
			ClickEffect.Unload();
			ClickerInterfaceResources.Unload();
			AutoClickKey = null;
			mod = null;
		}

		private void LoadClient()
		{
			ShaderManager.Load();
			ClickerInterfaceResources.Load();
		}

		private GameTime _lastUpdateUIGameTime;

		public override void UpdateUI(GameTime gameTime)
		{
			_lastUpdateUIGameTime = gameTime;
			ClickerInterfaceResources.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			ClickerInterfaceResources.AddDrawLayers(layers);

			//Remove Mouse Cursor
			if (Main.cursorOverride == -1 && ClickerConfigClient.Instance.ShowCustomCursors)
			{
				Player player = Main.LocalPlayer;
				if (ClickerCursor.CanDrawCursor(player.HeldItem))
				{
					for (int i = 0; i < layers.Count; i++)
					{
						if (layers[i].Name.Equals("Vanilla: Cursor"))
						{
							layers.RemoveAt(i);
							break;
						}
					}
				}
			}
		}

		public override object Call(params object[] args)
		{
			return ClickerModCalls.Call(args);
		}

		public override void AddRecipes()
		{
			ClickerRecipes.AddRecipes();
		}

		public override void PostAddRecipes()
		{
			finalizedRegisterCompat = true;
			ClickerSystem.FinalizeLocalization();
		}

		public override void AddRecipeGroups()
		{
			ClickerRecipes.AddRecipeGroups();
		}
	}
}
