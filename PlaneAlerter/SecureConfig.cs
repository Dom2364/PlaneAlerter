using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Tweetinvi.Models;

namespace PlaneAlerter {
	/// <summary>
	/// Class for storing things in config.bin
	/// </summary>
	static class SecureConfig {
		/// <summary>
		/// Class that gets saved to config.bin
		/// </summary>
		private class Config {
			/// <summary>
			/// Consumer tokens for PlaneAlerter
			/// </summary>
			public IConsumerCredentials AppCredentials;
			/// <summary>
			/// Dictionary of authorized accounts containing access tokens
			/// </summary>
			public Dictionary<string, ITwitterCredentials> UserCredentials;

			public Config(IConsumerCredentials appCredentials, Dictionary<string, ITwitterCredentials> userCredentials) {
				AppCredentials = appCredentials;
				UserCredentials = userCredentials;
			}
		}

		/// <summary>
		/// Save config
		/// </summary>
		public static void SaveConfig() {
			//Create instance of config with data to be saved
			Config config = new Config(
				Twitter.AppCredentials,
				Twitter.UserCredentials
			);

			//Serialise config as binary
			byte[] data;
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream()) {
				bf.Serialize(ms, config);
				data = ms.ToArray();
			}

			//Save to config.bin
			File.WriteAllBytes("config.bin", data);
		}
		/// <summary>
		/// Load Config
		/// </summary>
		public static void LoadConfig() {
			//Read data from config.bin
			byte[] data = File.ReadAllBytes("config.bin");
			
			//Deserialise data into config class
			Config config;
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream(data)) {
				config = (Config)bf.Deserialize(ms);
			}

			//Transfer saved info into variables
			Twitter.AppCredentials = config.AppCredentials;
			Twitter.UserCredentials = config.UserCredentials;
		}
	}
}
