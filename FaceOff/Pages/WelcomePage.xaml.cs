﻿using System;

using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FaceOff.Shared;
using EntryCustomReturn.Forms.Plugin.Abstractions;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FaceOff
{
	public partial class WelcomePage : ContentPage
	{
		#region Constructors
		public WelcomePage()
		{
			InitializeComponent();
			BindingContext = new WelcomeViewModel();

			PopulateAutomationIDs();
			PopulatePlaceholderText();
			ConfigureCustomReturnEffect();

			NavigationPage.SetBackButtonTitle(this, "");
		}
		#endregion

		#region Methods
		protected override void OnAppearing()
		{
			base.OnAppearing();

			StartGameButton.Clicked += HandleStartGameButtonClicked;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			StartGameButton.Clicked -= HandleStartGameButtonClicked;
		}

		void ExecutePlayer1EntryReturnCommand(object sender, EventArgs e)
		{
			Player2Entry.Focus();
		}

		async void HandleStartGameButtonClicked(object sender, EventArgs e)
		{
			var isPlayer1EntryTextEmpty = string.IsNullOrWhiteSpace(Player1Entry.Text);
			var isPlayer2EntryTextEmpty = string.IsNullOrWhiteSpace(Player2Entry.Text);

			if (isPlayer1EntryTextEmpty)
			{
				Insights.Track(InsightsConstants.StartGameButtonTapped, InsightsConstants.StartGameButtonTappedStatus, InsightsConstants.Player1NameEmpty);
				DisplayEmptyPlayerNameAlert(1);
			}
			else if (isPlayer2EntryTextEmpty)
			{
				Insights.Track(InsightsConstants.StartGameButtonTapped, InsightsConstants.StartGameButtonTappedStatus, InsightsConstants.Player2NameEmpty);
				DisplayEmptyPlayerNameAlert(2);
			}
			else
			{
				Insights.Track(InsightsConstants.StartGameButtonTapped, InsightsConstants.StartGameButtonTappedStatus, InsightsConstants.GameStarted);
				await Navigation.PushAsync(new PicturePage());
			}
		}

		void DisplayEmptyPlayerNameAlert(int playerNumber)
		{
			Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error", $"Player {playerNumber} Name is Blank", "OK"));
		}

		void PopulateAutomationIDs()
		{
			Player1Entry.AutomationId = AutomationIdConstants.Player1Entry;
			Player2Entry.AutomationId = AutomationIdConstants.Player2Entry;
			StartGameButton.AutomationId = AutomationIdConstants.StartGameButton;
		}

		void PopulatePlaceholderText()
		{
			Player1Entry.Placeholder = PlaceholderConstants.WelcomePagePlaceholderText;
			Player2Entry.Placeholder = PlaceholderConstants.WelcomePagePlaceholderText;
		}

		void ConfigureCustomReturnEffect()
		{
			CustomReturnEffect.SetReturnType(Player1Entry, ReturnType.Next);
			CustomReturnEffect.SetReturnCommand(Player1Entry, new Command(() => Player2Entry.Focus()));

			CustomReturnEffect.SetReturnType(Player2Entry, ReturnType.Go);
			CustomReturnEffect.SetReturnCommand(Player2Entry, new Command(() => HandleStartGameButtonClicked(StartGameButton, EventArgs.Empty)));
		}
		#endregion
	}
}
