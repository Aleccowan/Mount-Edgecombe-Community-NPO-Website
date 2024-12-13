using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using WILLNPO_Diya_.Data;
using WILLNPO_Diya_.Models;

namespace WILLNPO_Diya_.Controllers
{
    public class EventsController : Controller
    {
        private readonly FirebaseService _firebaseService;
        private const string BucketName = "mountedgecombecommunitywil.appspot.com";

        public EventsController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public async Task<IActionResult> Index()
        {
            List<Event> events = await _firebaseService.GetEventsAsync();
            return View(events);
        }

        [Authorize]
        public async Task<IActionResult> AdminPanel()
        {
            var events = await _firebaseService.GetEventsAsync();
            return View(events);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddEvent(IFormFile imageFile, string name, string description, string dateTime, string location)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imageUrl = null;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Generate a unique path for the image
                        string storagePath = $"events/{Guid.NewGuid()}_{imageFile.FileName}";
                        imageUrl = await UploadImageToFirebaseStorage(imageFile, storagePath);
                    }

                    // Create a new Event object
                    var newEvent = new Event
                    {
                        name = name,
                        description = description,
                        dateTime = dateTime,
                        location = location,
                        imageUrl = imageUrl // Use the uploaded image URL
                    };

                    // Save the event to Firestore
                    await _firebaseService.AddEventAsync(newEvent);

                    TempData["Message"] = "Event added successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error adding event: {ex.Message}";
                }
            }

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteEvent(string name)
        {
            await _firebaseService.DeleteEventByNameAsync(name);
            return RedirectToAction("AdminPanel");
        }

        // Helper method to upload an image to Firebase Storage
        private async Task<string> UploadImageToFirebaseStorage(IFormFile file, string storagePath)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mountedgecombecommunitywil-firebase-adminsdk-a6qvl-cac9af6cec.json");
            var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(jsonPath);

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var storageClient = StorageClient.Create(credential);

            // Upload the file with public read access
            await storageClient.UploadObjectAsync(BucketName, storagePath, file.ContentType, memoryStream, new UploadObjectOptions
            {
                PredefinedAcl = PredefinedObjectAcl.PublicRead
            });

            // Return the public URL of the uploaded file
            return $"https://storage.googleapis.com/{BucketName}/{storagePath}";
        }
    }
}
