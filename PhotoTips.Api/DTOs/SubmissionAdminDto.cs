using System;
using PhotoTips.Core.Models;

namespace PhotoTips.Api.DTOs
{
    public class SubmissionAdminDto
    {
        public string Id { get; set; }
        public long ModuleEntryId { get; set; }
        public PhotoDto Photo { get; set; }
        public Submission.SubmissionStatus Status { get; set; }
        public int Mark { get; set; }
        public string Comment { get; set; }
        public DateTime Time { get; set; }
    }
}