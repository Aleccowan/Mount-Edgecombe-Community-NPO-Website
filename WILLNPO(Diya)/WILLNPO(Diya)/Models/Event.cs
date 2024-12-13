using Google.Cloud.Firestore;

namespace WILLNPO_Diya_.Models
{
    [FirestoreData]
    public class Event
    {
        /*[FirestoreDocumentId]
        public string Id { get; set; }*/

        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public string description { get; set; }

        [FirestoreProperty]
        public string dateTime { get; set; }
        [FirestoreProperty]
        public string location { get; set; }
        [FirestoreProperty]
        public string imageUrl { get; set; }

        // Parameterless constructor required for Firestore
        public Event() { }
    }
}
