using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class FlightController : Controller
	{
		private readonly static string _apiUrl = "http://localhost:49471/api/Flights";

		[HttpGet]
		public async Task<IActionResult> Index(string from, string to)
		{
			List<Flight> flights = new List<Flight>();

			if(String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
			{
				using(HttpClient client = new HttpClient())
				{
					HttpResponseMessage response = await client.GetAsync(_apiUrl);
					if(response.IsSuccessStatusCode)
					{
						flights = await response.Content.ReadAsAsync<List<Flight>>();
					}
				}
			}
			else
			{
				using(HttpClient client = new HttpClient())
				{
					HttpResponseMessage response = await client.GetAsync(
						string.Format(_apiUrl + "/FromAndTo?from={0}&to={1}", from, to)
					);
					if(response.IsSuccessStatusCode)
					{
						flights = await response.Content.ReadAsAsync<List<Flight>>();
					}
					else
					{
						
					}
				}
			}

			return View(flights);
		}
	

		[HttpGet]
		public async Task<IActionResult> EditDeparture(int id)
		{
			using(HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(_apiUrl + "/" + id);
				if(response.IsSuccessStatusCode)
				{
					return View(await response.Content.ReadAsAsync<Flight>());
				}
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> EditDeparture(Flight flight)
		{
			if(ModelState.IsValid)
			{
				using(HttpClient client = new HttpClient())
				{
					HttpResponseMessage response = await client.PutAsJsonAsync(_apiUrl + "/" + flight.FlightId, flight);
					if(response.IsSuccessStatusCode)
					{
						return RedirectToAction("Index", "Flight");
					}
					else
					{
						ModelState.AddModelError(String.Empty, "Fejl! Kunne ikke opdatere flyafgangen. StatusCode: " + response.StatusCode);
					}
				}
			}

			return View(flight);
		}
	}
}