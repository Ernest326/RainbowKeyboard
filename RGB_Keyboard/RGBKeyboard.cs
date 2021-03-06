using BepInEx;
using BepInEx.Configuration;
using Photon.Pun;
using System.IO;
using System.Net;
using UnityEngine;

namespace RainbowMonke
{
	[BepInPlugin("org.ernest326.gorillatag.rainbowkeyboard", "Rainbow Keyboard", "1.0.0")]
	public class RGBKeyboard : BaseUnityPlugin
	{
		public static ConfigEntry<bool> enabled;
		public static ConfigEntry<bool> randomColor;
		public static ConfigEntry<float> cycleSpeed;
		public static ConfigEntry<float> glowAmount;

		private Color color = new Color(0, 0, 0);
		private float hue = 0f;
		private float timer = 0f;

		private void Awake()
		{

			Debug.Log("Starting Rainbow Monkey");
			ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "RainbowKeyboard.cfg"), true);

			enabled = config.Bind<bool>("Config", "Enabled", true, "Whether the plugin is enabled or not");
			randomColor = config.Bind<bool>("Config", "RandomColor", false, "Whether to cycle through colours of rainbow or choose random colors");
			cycleSpeed = config.Bind<float>("Config", "CycleSpeed", 0.0025f, "The speed the color cycles at each frame (1=Full colour cycle). If random colour is enabled, this is the time in seconds before switching color");
			glowAmount = config.Bind<float>("Config", "GlowAmount", 1.0f, "The brightness of your monkey. The higher the value, the more emissive your monkey is");

		}

		public void Update()
		{

			if (randomColor.Value)
			{
				if (Time.time > timer)
				{
					color = Random.ColorHSV(0, 1, glowAmount.Value, glowAmount.Value, glowAmount.Value, glowAmount.Value);
					timer = Time.time + cycleSpeed.Value;
				}

			} else
			{
				if (hue >= 1)
				{
					hue = 0;
				}

				hue += cycleSpeed.Value;
				color = Color.HSVToRGB(hue, 1.0f * glowAmount.Value, 1.0f * glowAmount.Value);
			}

			foreach(GorillaKeyboardButton key in GameObject.FindObjectsOfType<GorillaKeyboardButton>())
			{
				key.GetComponent<Renderer>().material.color = color;
			}

		}

	}
}
