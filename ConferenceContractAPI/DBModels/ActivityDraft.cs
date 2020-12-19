﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ActivityDraft
    {
        [Key]
        public Guid ActivityDraftID { get; set; }

        [MaxLength(50)]
        public string ActivityID { get; set; }

        [MaxLength(100)]
        public Guid? ActivityTypeID { get; set; }

        [MaxLength(100)]
        public string TimeLength { get; set; }

        public int Sort { get; set; }

        [MaxLength(100)]
        public string StartDate { get; set; }

        [MaxLength(200)]
        public string StartTime { get; set; }

        [MaxLength(200)]
        public string EndTime { get; set; }

        [MaxLength(2000)]
        public string Translation { get; set; }


    }
}
