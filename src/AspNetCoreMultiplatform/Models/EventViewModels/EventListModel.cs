using System;

namespace AspNetCoreMultiplatform.Models.EventViewModels
{
    public class EventListModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Begins { get; set; }

        public DateTime Ends { get; set; }

        public string Location { get; set; }
    }
}
