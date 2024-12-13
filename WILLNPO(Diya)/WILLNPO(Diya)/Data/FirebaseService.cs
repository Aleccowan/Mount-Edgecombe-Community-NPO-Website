using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WILLNPO_Diya_.Models;
namespace WILLNPO_Diya_.Data
{
   

    public class FirebaseService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirebaseService(IConfiguration configuration)
        {
            var projectId = configuration["Firebase:ProjectId"];
            var serviceAccountPath = configuration["Firebase:ServiceAccountPath"];
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", serviceAccountPath);

            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            List<Event> events = new List<Event>();

            CollectionReference eventsCollection = _firestoreDb.Collection("events");
            QuerySnapshot snapshot = await eventsCollection.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Event evt = document.ConvertTo<Event>();
                    events.Add(evt);
                }
            }

            return events;
        }


        //testing
        // Add a new event
        public async Task AddEventAsync(Event newEvent)
        {
            CollectionReference eventsCollection = _firestoreDb.Collection("events");
            await eventsCollection.AddAsync(newEvent);
        }

        // Delete an event by name
        public async Task DeleteEventByNameAsync(string name)
        {
            CollectionReference eventsCollection = _firestoreDb.Collection("events");
            Query query = eventsCollection.WhereEqualTo("name", name);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                await document.Reference.DeleteAsync();
            }
        }



    }

    

}
