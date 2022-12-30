﻿using System;
using System.Windows.Forms;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlaneAlerter.Forms;
using PlaneAlerter.Services;

namespace PlaneAlerter {
	static class Program {
		public static IServiceProvider ServiceProvider { get; private set; }

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.SetCompatibleTextRenderingDefault(false);

			var host = CreateHostBuilder().Build();
			ServiceProvider = host.Services;

			Application.Run(ServiceProvider.GetRequiredService<MainForm>());

			Environment.Exit(0);
		}

		

		private static IHostBuilder CreateHostBuilder()
		{
			return Host.CreateDefaultBuilder()
				.UseDefaultServiceProvider((context, options) =>
				{
					options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
					options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
				})
				.ConfigureServices((context, services) =>
				{
					//Forms
					services.AddTransient<MainForm>();
					services.AddTransient<ConditionEditorForm>();
					services.AddTransient<ConditionListForm>();
					services.AddTransient<EmailContentConfigForm>();
					services.AddTransient<PinPromptDialog>();
					services.AddTransient<PropertyInfoForm>();
					services.AddTransient<SettingsForm>();

					//Services
					services.AddSingleton<ITwitterService, TwitterService>();
				});
		}
	}
}
