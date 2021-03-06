﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreMultiplatform.Data
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Begins { get; set; }

        public DateTime Ends { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public ApplicationUser Owner { get; set; }
    }
}
