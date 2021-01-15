using System;
using PhotoTips.Core.Models;

namespace PhotoTips.Api.DTOs
{
    public class SubmissionDto
    {
        public string Id { get; set; }
        public long ModuleEntryId { get; set; }
        public string PhotoId { get; set; }
        public Submission.SubmissionStatus Status { get; set; }
        public DateTime Time { get; set; }
    }
}