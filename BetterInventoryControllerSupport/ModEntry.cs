using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace BetterInventoryControllerSupport
{
	public class ModEntry : Mod
	{

		public override void Entry(IModHelper helper)
		{
			ControlEvents.ControllerButtonPressed += this.ReceiveKeyPress;
		}

		private void ReceiveKeyPress(object sender, EventArgsControllerButtonPressed e)
		{
			if (e.ButtonPressed != Buttons.DPadUp &&
				e.ButtonPressed != Buttons.DPadDown &&
				e.ButtonPressed != Buttons.DPadLeft &&
				e.ButtonPressed != Buttons.DPadRight)
				return;

			IReflectionHelper reflection = this.Helper.Reflection;

			if (Game1.activeClickableMenu == null)
				return;

			this.Monitor.Log("activeClickableMenu not null");


			if (Game1.activeClickableMenu is GameMenu)
			{

				GameMenu gameMenu = (GameMenu)Game1.activeClickableMenu;
				List<IClickableMenu> pages = reflection.GetPrivateField<List<IClickableMenu>>(gameMenu, "pages").GetValue();
				InventoryPage inventoryPage = (InventoryPage)pages[0];

				if (inventoryPage == null)
				{
					this.Monitor.Log("InventoryPage null");
					return;
				}

				InventoryMenu menu = reflection.GetPrivateField<InventoryMenu>(inventoryPage, "inventory").GetValue();

				ClickableComponent hoveringComponent = getClickableComponentFromMousePosition(menu.inventory, Mouse.GetState().X, Mouse.GetState().Y);

				this.Monitor.Log($"Tile size: {Game1.tileSize}, vertical gap: {menu.verticalGap}, horizontal gap: {menu.horizontalGap}");

				if (e.ButtonPressed == Buttons.DPadRight)
					Mouse.SetPosition(hoveringComponent.bounds.X + Game1.tileSize + menu.horizontalGap, hoveringComponent.bounds.Y + 32);

				if (e.ButtonPressed == Buttons.DPadLeft)
					Mouse.SetPosition(hoveringComponent.bounds.X - Game1.tileSize - menu.horizontalGap, hoveringComponent.bounds.Y + 32);

				if (e.ButtonPressed == Buttons.DPadUp)
					Mouse.SetPosition(hoveringComponent.bounds.X, hoveringComponent.bounds.Y - Game1.tileSize - menu.verticalGap);

				if (e.ButtonPressed == Buttons.DPadDown)
					Mouse.SetPosition(hoveringComponent.bounds.X, hoveringComponent.bounds.Y + Game1.tileSize + 32);
			}
		}

		private ClickableComponent getClickableComponentFromMousePosition(List<ClickableComponent> inventory, int x, int y)
		{
			for (int i = 0; i < inventory.Count; i++)
			{
				if (inventory[i] != null && inventory[i].bounds.Contains(x, y))
					return inventory[i];
			}

			return null;
		}

	}
}
