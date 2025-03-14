﻿using AIForOrcas.Client.BL.Services;
using AIForOrcas.DTO;
using AIForOrcas.DTO.API;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace AIForOrcas.Client.Web.Pages.User
{
	public partial class UserActivity
	{
		[Inject]
		IMetricsService Service { get; set; }

		[Inject]
		IJSRuntime JSRuntime { get; set; }

		[Inject]
		AuthenticationStateProvider AuthenticationStateProvider { get; set; }

		private ModeratorMetrics metrics = null;

		private ModeratorMetricsFilterDTO filterOptions =
			new ModeratorMetricsFilterDTO() { Timeframe = "all" };

		private string messageStyle = "d-none";
		private string message = string.Empty;
		private string displayStyle = "d-none";

		protected override async Task OnInitializedAsync()
		{
			await LoadMetrics();
		}

		private async Task LoadMetrics()
		{
			var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;

			filterOptions.Moderator = user.Identity.Name;


			displayStyle = "d-none";
			messageStyle = "";
			message = "Loading metrics...";

			metrics = await Service.GetModeratorMetricsAsync(filterOptions);

			if (!metrics.HasContent)
			{
				message = "No metrics found for the selected filter options. Please select a different set of filter options...";
				displayStyle = "d-none";
			}
			else
			{
				messageStyle = "d-none";
				displayStyle = "";
			}

			StateHasChanged();

			await JSRuntime.InvokeVoidAsync("DrawDetectionsChart", metrics.DetectionsArray);
			await JSRuntime.InvokeVoidAsync("DrawDetectionResultsChart", metrics.DetectionResultsArray);
		}

		private async Task ActOnApplyFilterCallback(MetricsFilterDTO returnedFilterOptions)
		{
			filterOptions.Timeframe = returnedFilterOptions.Timeframe;
			await LoadMetrics();
		}
	}
}
