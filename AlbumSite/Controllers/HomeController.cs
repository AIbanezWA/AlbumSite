using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlbumSite.Models;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;
using AlbumSite.Configuration;

namespace AlbumSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JsonSettings _settings;

        public HomeController(IOptions<JsonSettings> settings,
                              ILogger<HomeController> logger)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = new AlbumsModel();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(_settings.AlbumsUri);
                var apiResponse = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<AlbumsModel>(apiResponse); 
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPhotos(int albumId)
        {
            IEnumerable<Photo> result = new List<Photo>();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(_settings.PhotosUri);
                var apiResponse = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<IEnumerable<Photo>>(apiResponse);

                result = result.Where(p => p.AlbumId == albumId);
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int postId)
        {
            IEnumerable<Comment> result = new List<Comment>();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(_settings.CommentsUri);
                var apiResponse = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<IEnumerable<Comment>>(apiResponse);

                result = result.Where(p => p.PostId == postId);
            }
            return Json(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
